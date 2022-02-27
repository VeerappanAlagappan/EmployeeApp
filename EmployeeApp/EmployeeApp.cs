using ManageEmployee.BAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace EmployeeApp
{
    public class EmployeeApp
    {
        private readonly ILogger<EmployeeApp> _logger;
        private readonly IManageEmployee _manageEmployee;
        private readonly IInvoker _invoker;
        public EmployeeApp(IManageEmployee manageEmployee, ILogger<EmployeeApp> logger, IInvoker invoker)
        {
            this._manageEmployee = manageEmployee;
            this._logger = logger;
            this._invoker = invoker;

        }
        public void Run()
        {
            //_logger.LogInformation("Entered in to the Employee App!");
            try
            {

                Console.WriteLine("Welcome to the Manage Employee App");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Please provide the xml path to be processed");
                Common.FilePath = Console.ReadLine();
                if (!string.IsNullOrEmpty(Common.FilePath) && File.Exists(Common.FilePath))
                {
                    Console.WriteLine("Choose Any of the following options to perform in the xml");
                    Console.WriteLine("Press");
                    Console.WriteLine("\"1\" => Add A New Employee");
                    Console.WriteLine("\"2\" => Delete Existing Employee");
                    var s = Console.ReadKey().KeyChar;
                    int num = 0;

                    if (int.TryParse(s.ToString(), out num))
                    {
                        //usage: Command Pattern to call particular class based on user input
                        if (this._invoker.getCommand(num))
                        {
                            _logger.LogInformation("The operation you have chosen is successfully executed");
                        }
                        else
                        {
                            _logger.LogInformation("Operation is Failed or Aborted!Please check the input/xml");
                        }

                    }
                    else
                    {
                        _logger.LogWarning("Enter A Valid Number");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Occured");
                _logger.LogError($"Error Message-{ex.Message}|| Error Details-{ex.InnerException} || Error Locate-{ex.StackTrace}");
            }
        }


    }



}