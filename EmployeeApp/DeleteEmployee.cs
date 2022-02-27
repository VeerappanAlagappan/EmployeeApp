using ManageEmployee.BAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmployeeApp
{
    public class DeleteEmployee : IExecute
    {
        private readonly ILogger<DeleteEmployee> _logger;
        private readonly IManageEmployee _manageEmployee;


        public DeleteEmployee(ILogger<DeleteEmployee> logger, IManageEmployee manageEmployee)
        {
            _logger = logger;
            _manageEmployee = manageEmployee;
            CommandName = Convert.ToInt32(Actions.Delete);
        }

        public int CommandName
        {
            get; set;
        }
        /// <summary>
        ///  This method executes the action triggered by the user to delete any element in the FS(xml)
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            try
            {
                Console.WriteLine("Enter the employee name to be deleted");
                string employeeName = Console.ReadLine();
                return this._manageEmployee.DeleteEmployees(Common.FilePath, employeeName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {MethodBase.GetCurrentMethod().Name} || Error Message:{ex.Message} || Stack Trace:{ex.StackTrace}");
                throw ex;
            }
        }
    }
}