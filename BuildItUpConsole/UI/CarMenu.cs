using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildItUpConsole.UI
{
    internal class CarMenu
    {
        private readonly ICarService _carService;

        public CarMenu(ICarService carService)
        {
            _carService = carService;
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Car Menu ===");
                Console.WriteLine("1. Show all cars");
                Console.WriteLine("2. Add car");
                Console.WriteLine("3. Edit car");
                Console.WriteLine("4. Delete car");
                Console.WriteLine("0. Back to main menu");
                Console.Write("Choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await ShowAllAsync(); break;
                    case "2": await AddAsync(); break;
                    case "3": await EditAsync(); break;
                    case "4": await DeleteAsync(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option!"); break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        private async Task ShowAllAsync()
        {
            var cars = await _carService.GetAllAsync();

            Console.WriteLine("\n--- All Cars ---");
            foreach (var car in cars)
            {
                Console.WriteLine($"{car.Id}. {car.Model} - {car.Engine.EngineModel} ({car.Engine.Company.Name})");
            }
        }

        private async Task AddAsync()
        {
            Console.Write("Car Model: ");
            var model = Console.ReadLine();

            Console.Write("Engine ID: ");
            var engineId = Console.ReadLine();

            Console.Write("Company ID: ");
            var companyId = Console.ReadLine();

            var car = new Car
            {
                Model = model!,
                EngineId = int.Parse(engineId!),
                CompanyId = int.Parse(companyId!)
            };

            await _carService.AddAsync(car);

            Console.WriteLine("Car added.");
        }

        private async Task EditAsync()
        {
            await ShowAllAsync();

            Console.Write("\nEnter ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var car = await _carService.GetByIdAsync(id);
                if (car == null)
                {
                    Console.WriteLine("Car not found.");
                    return;
                }

                Console.Write($"Model ({car.Model}): ");
                var model = Console.ReadLine();
                Console.Write($"Engine ID ({car.EngineId}): ");
                var engineId = Console.ReadLine();

                car.Model = string.IsNullOrWhiteSpace(model) ? car.Model : model;
                car.EngineId = string.IsNullOrWhiteSpace(engineId) ? car.EngineId : int.Parse(engineId);

                await _carService.UpdateAsync(car);
                Console.WriteLine("Car updated.");
            }
        }

        private async Task DeleteAsync()
        {
            await ShowAllAsync();

            Console.Write("\nEnter ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await _carService.DeleteAsync(id);
                Console.WriteLine("Car deleted.");
            }
        }
    }
}
