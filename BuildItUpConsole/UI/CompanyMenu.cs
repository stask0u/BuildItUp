using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildItUpConsole.UI
{
    internal class CompanyMenu
    {
        private readonly ICompanyService _companyService;

        public CompanyMenu(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Company Menu ===");
                Console.WriteLine("1. Show all companies");
                Console.WriteLine("2. Add company");
                Console.WriteLine("3. Edit company");
                Console.WriteLine("4. Delete company");
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
            var companies = await _companyService.GetAllAsync();

            Console.WriteLine("\n--- All Companies ---");
            foreach (var company in companies)
            {
                Console.WriteLine($"{company.Id}. {company.Name}");
            }
        }

        private async Task AddAsync()
        {
            Console.Write("Company Name: ");
            var name = Console.ReadLine();

            var company = new Company
            {
                Name = name!
            };

            await _companyService.AddAsync(company);

            Console.WriteLine("Company added.");
        }

        private async Task EditAsync()
        {
            await ShowAllAsync();

            Console.Write("\nEnter ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var company = await _companyService.GetByIdAsync(id);
                if (company == null)
                {
                    Console.WriteLine("Company not found.");
                    return;
                }

                Console.Write($"Name ({company.Name}): ");
                var name = Console.ReadLine();

                company.Name = string.IsNullOrWhiteSpace(name) ? company.Name : name;

                await _companyService.UpdateAsync(company);
                Console.WriteLine("Company updated.");
            }
        }

        private async Task DeleteAsync()
        {
            await ShowAllAsync();

            Console.Write("\nEnter ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await _companyService.DeleteAsync(id);
                Console.WriteLine("Company deleted.");
            }
        }
    }
}

