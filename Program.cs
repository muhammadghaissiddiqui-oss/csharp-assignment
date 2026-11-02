
using System;
using System.Collections.Generic;

namespace ToolVehicleTaxManagement
{
    // Base Abstract Class
    abstract class ToolVehicle
    {
        public int VehicleID { get; private set; }
        public string RegNo { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public decimal BasePrice { get; set; }
        public string VehicleType { get; protected set; }

        // Static Properties
        public static int TotalVehicles { get; private set; }
        public static int TotalTaxPayingVehicles { get; private set; }
        public static int TotalNonTaxPayingVehicles { get; private set; }
        public static decimal TotalTaxCollected { get; private set; }

        private bool isProcessed = false;

        // Constructor
        protected ToolVehicle(
            int vehicleID,
            string regNo,
            string model,
            string brand,
            decimal basePrice,
            string vehicleType)
        {
            VehicleID = vehicleID;
            RegNo = regNo;
            Model = model;
            Brand = brand;
            BasePrice = basePrice;
            VehicleType = vehicleType;

            TotalVehicles++;
        }

        // Abstract Method
        public abstract void PayTax();

        // Pass Without Paying
        public void PassWithoutPaying()
        {
            if (isProcessed)
            {
                Console.WriteLine("This vehicle has already been processed!");
                return;
            }

            TotalNonTaxPayingVehicles++;
            isProcessed = true;

            Console.WriteLine("Vehicle passed without paying tax!");
        }

        // Common Tax Processing Method
        protected void CollectTax(decimal taxAmount)
        {
            if (isProcessed)
            {
                Console.WriteLine("This vehicle has already been processed!");
                return;
            }

            TotalTaxCollected += taxAmount;
            TotalTaxPayingVehicles++;
            isProcessed = true;

            Console.WriteLine("Tax paid successfully!");
            Console.WriteLine($"Tax Amount: ${taxAmount}");
        }

        // Display Vehicle Details
        public void DisplayDetails()
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Vehicle ID   : {VehicleID}");
            Console.WriteLine($"Registration : {RegNo}");
            Console.WriteLine($"Model        : {Model}");
            Console.WriteLine($"Brand        : {Brand}");
            Console.WriteLine($"Base Price   : ${BasePrice}");
            Console.WriteLine($"Vehicle Type : {VehicleType}");
            Console.WriteLine("--------------------------------");
        }

        // Display Static Report
        public static void DisplayReport()
        {
            Console.WriteLine("\n========== TAX REPORT ==========");
            Console.WriteLine($"Total Vehicles             : {TotalVehicles}");
            Console.WriteLine($"Tax Paying Vehicles        : {TotalTaxPayingVehicles}");
            Console.WriteLine($"Non-Tax Paying Vehicles    : {TotalNonTaxPayingVehicles}");
            Console.WriteLine($"Total Tax Collected        : ${TotalTaxCollected}");
            Console.WriteLine("================================");
        }
    }

    // Car Class
    class Car : ToolVehicle
    {
        public Car(int id, string regNo, string model,
                   string brand, decimal price)
            : base(id, regNo, model, brand, price, "Car")
        {
        }

        public override void PayTax()
        {
            CollectTax(2m);
        }
    }

    // Bike Class
    class Bike : ToolVehicle
    {
        public Bike(int id, string regNo, string model,
                    string brand, decimal price)
            : base(id, regNo, model, brand, price, "Bike")
        {
        }

        public override void PayTax()
        {
            CollectTax(1m);
        }
    }

    // Heavy Vehicle Class
    class HeavyVehicle : ToolVehicle
    {
        public HeavyVehicle(int id, string regNo, string model,
                            string brand, decimal price)
            : base(id, regNo, model, brand, price, "Heavy Vehicle")
        {
        }

        public override void PayTax()
        {
            CollectTax(4m);
        }
    }

    // Main Program
    class Program
    {
        static List<ToolVehicle> vehicles = new List<ToolVehicle>();
        static int nextID = 1;

        static void Main(string[] args)
        {
            bool running = true;

            Console.WriteLine("========================================");
            Console.WriteLine("   TOOL VEHICLE TAX MANAGEMENT SYSTEM");
            Console.WriteLine("========================================");

            while (running)
            {
                Console.WriteLine("\n========== MAIN MENU ==========");
                Console.WriteLine("1. Add Car");
                Console.WriteLine("2. Add Bike");
                Console.WriteLine("3. Add Heavy Vehicle");
                Console.WriteLine("4. Pay Vehicle Tax");
                Console.WriteLine("5. Pass Without Paying");
                Console.WriteLine("6. View All Vehicles");
                Console.WriteLine("7. View Tax Report");
                Console.WriteLine("8. Exit");
                Console.WriteLine("================================");

                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddVehicle("Car");
                        break;

                    case "2":
                        AddVehicle("Bike");
                        break;

                    case "3":
                        AddVehicle("Heavy Vehicle");
                        break;

                    case "4":
                        ProcessVehicle(true);
                        break;

                    case "5":
                        ProcessVehicle(false);
                        break;

                    case "6":
                        DisplayAllVehicles();
                        break;

                    case "7":
                        ToolVehicle.DisplayReport();
                        break;

                    case "8":
                        running = false;
                        Console.WriteLine("Thank you for using the system!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        // Add Vehicle
        static void AddVehicle(string type)
        {
            Console.WriteLine($"\nEnter {type} Details:");

            Console.Write("Registration Number: ");
            string regNo = Console.ReadLine() ?? "";

            Console.Write("Model: ");
            string model = Console.ReadLine() ?? "";

            Console.Write("Brand: ");
            string brand = Console.ReadLine() ?? "";

            decimal price;

            while (true)
            {
                Console.Write("Base Price: ");

                if (decimal.TryParse(Console.ReadLine(), out price)
                    && price >= 0)
                {
                    break;
                }

                Console.WriteLine("Please enter a valid positive price.");
            }

            ToolVehicle vehicle;

            if (type == "Car")
            {
                vehicle = new Car(nextID, regNo, model, brand, price);
            }
            else if (type == "Bike")
            {
                vehicle = new Bike(nextID, regNo, model, brand, price);
            }
            else
            {
                vehicle = new HeavyVehicle(nextID, regNo, model, brand, price);
            }

            vehicles.Add(vehicle);
            nextID++;

            Console.WriteLine("\nVehicle added successfully!");
            Console.WriteLine($"Vehicle ID: {vehicle.VehicleID}");
        }

        // Pay Tax or Pass Without Paying
        static void ProcessVehicle(bool payTax)
        {
            if (vehicles.Count == 0)
            {
                Console.WriteLine("No vehicles available!");
                return;
            }

            Console.Write("Enter Vehicle ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid Vehicle ID!");
                return;
            }

            ToolVehicle vehicle = vehicles.Find(v => v.VehicleID == id);

            if (vehicle == null)
            {
                Console.WriteLine("Vehicle not found!");
                return;
            }

            if (payTax)
            {
                vehicle.PayTax();
            }
            else
            {
                vehicle.PassWithoutPaying();
            }
        }

        // Display All Vehicles
        static void DisplayAllVehicles()
        {
            if (vehicles.Count == 0)
            {
                Console.WriteLine("No vehicles registered yet!");
                return;
            }

            Console.WriteLine("\n========== ALL VEHICLES ==========");

            foreach (ToolVehicle vehicle in vehicles)
            {
                vehicle.DisplayDetails();
            }
        }
    }
}
