using BuildItUp.Data;
using BuildItUp.Services.Implementations;
using BuildItUp.Services.Interfaces;
using BuildItUpConsole.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BuildItUpConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Register DbContext and services
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server =localhost\\SQLEXPRESS; Database = BuildItUpDB; Trusted_Connection = True; TrustServerCertificate = True; "));

            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEngineService, EngineService>();

            // Build the ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Instantiate the service objects
            var carService = serviceProvider.GetRequiredService<ICarService>();
            var companyService = serviceProvider.GetRequiredService<ICompanyService>();
            var engineService = serviceProvider.GetRequiredService<IEngineService>();

            // Instantiate the menu classes
            var carMenu = new CarMenu(carService);
            var companyMenu = new CompanyMenu(companyService);
            var engineMenu = new EngineMenu(engineService,companyService);

            // Main menu loop
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Main Menu ===");
                Console.WriteLine("1. Manage Cars");
                Console.WriteLine("2. Manage Companies");
                Console.WriteLine("3. Manage Engines");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": await carMenu.ShowMenuAsync(); break;
                    case "2": await companyMenu.ShowMenuAsync(); break;
                    case "3": await engineMenu.ShowMenuAsync(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
            }
        }
    }
}
