using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using System.Threading.Tasks;
using BuildItUp.Services.Implementations;

namespace BuildItUpConsole.UI
{
    internal class EngineMenu
    {
        private readonly IEngineService _engineService;
        private readonly ICompanyService _companyService;

        public EngineMenu(IEngineService engineService,ICompanyService companyService)
        {
            _engineService = engineService;
            _companyService = companyService;
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Engine Menu ===");
                Console.WriteLine("1. Show all engines");
                Console.WriteLine("2. Add engine");
                Console.WriteLine("3. Edit engine");
                Console.WriteLine("4. Delete engine");
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
            var engines = await _engineService.GetAllAsync();

            Console.WriteLine("\n--- All Engines ---");
            foreach (var engine in engines)
            {
                Console.WriteLine($"{engine.Id}. {engine.EngineModel} - {engine.Company.Name} - {engine.Horsepower}");
            }
        }

        private async Task AddAsync()
        {
            Console.Write("Model: ");
            var model = Console.ReadLine();

            Console.Write("Company Name: ");
            var companyName = Console.ReadLine();
            var company = (await _companyService.GetAllAsync()).FirstOrDefault(c => c.Name == companyName);



            Console.Write("Power (hp): ");
            var power = Console.ReadLine();

            var engine = new Engine
            {
                EngineModel = model!,
                Company = company,
                Horsepower = int.Parse(power!)
            };

            await _engineService.AddAsync(engine);

            Console.WriteLine("Engine added.");
        }

        private async Task EditAsync()
        {
            await ShowAllAsync();

            Console.Write("\nEnter ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var engine = await _engineService.GetByIdAsync(id);
                if (engine == null)
                {
                    Console.WriteLine("Engine not found.");
                    return;
                }

                Console.Write($"Model ({engine.EngineModel}): ");
                var model = Console.ReadLine();
                Console.Write($"Company ({engine.Company.Name}): ");
                var company = Console.ReadLine();
                Console.Write($"Power ({engine.Horsepower}): ");
                var power = Console.ReadLine();

                engine.EngineModel = string.IsNullOrWhiteSpace(model) ? engine.EngineModel : model;
                engine.Company.Name = string.IsNullOrWhiteSpace(company) ? engine.Company.Name : company;
                engine.Horsepower = string.IsNullOrWhiteSpace(power) ? engine.Horsepower : int.Parse(power);

                await _engineService.UpdateAsync(engine);
                Console.WriteLine("Engine updated.");
            }
        }

        private async Task DeleteAsync()
        {
            await ShowAllAsync();

            Console.Write("\nEnter ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await _engineService.DeleteAsync(id);
                Console.WriteLine("Engine deleted.");
            }
        }
    }
}
