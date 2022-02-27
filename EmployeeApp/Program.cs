using ManageEmployee.BAL;
using ManageEmployee.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace EmployeeApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<EmployeeApp>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            // add logging
            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();

            });

            // add services
            serviceCollection.AddSingleton<IXMLParser, EmployeeXMLParser>();
            // add services
            serviceCollection.AddSingleton<IManageEmployee, ManageEmployees>();

            serviceCollection.AddTransient<IInvoker, InvokerClass>();

            // add app
            serviceCollection.AddTransient<EmployeeApp>();
        }

    }


}