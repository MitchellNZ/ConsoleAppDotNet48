using System.Threading.Tasks;
using Forbury.Integrations.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Forbury.Integrations.API.v1.Dto.Enums;
using Forbury.Integrations.API.v1.Interfaces.Model;
using System;

namespace ConsoleAppDotNet48
{
    internal class Program
    {
        // Setup notes
        //
        //  1. Ensure the following packages are installed:
        //    - Forbury.Integrations (1.6.0-beta or higher)
        //    - Microsoft.Extensions.DependencyInjection
        //    - Microsoft.Extensions.Configuration
        //  2. Ensure the appsettings.json has property 'Copy to Output Directory' set to 'Copy always'
        //  3. Add your 'ClientId' and 'ClientSecret' to the appsettings.json file

        static async Task Main(string[] args)
        {
            // Register the appsettings.json files
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Register Forbury services and create the ServiceProvider
            var services = new ServiceCollection();
            var serviceProvider = services
                .AddForburyApi(configuration)
                .BuildServiceProvider();

            // Get the Forbury Commercial Model API Client
            var forburyModelClient = serviceProvider.GetRequiredService<IForburyModelCommercialApiClient>();

            // Load the first page of Commercial models
            var pagedModels = await forburyModelClient.GetModels(ProductType.Commercial);

            // Display some information on the models found
            Console.WriteLine($"Found a total of {pagedModels.TotalItems} models.");
            Console.WriteLine();
            foreach (var model in pagedModels.Items)
            {
                Console.WriteLine($"Model: {model.Attributes.ModelName} ({model.Attributes.ModelId})");
            }

            // Keep console open
            Console.Read();
        }
    }
}
