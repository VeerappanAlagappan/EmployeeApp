using ManageEmployee.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace ManageEmployee.BAL
{
    public class ManageEmployees : IManageEmployee
    {
        private readonly ILogger<ManageEmployees> _logger;
        private readonly IXMLParser _xmlParser;


        public ManageEmployees(ILogger<ManageEmployees> logger, IXMLParser xmlParser)
        {
            this._logger = logger;
            this._xmlParser = xmlParser;
        }
        public bool AddEmployees(dynamic s, EmployeeRequest employeeReq, string filePath)
        {
            try
            {
                Employee employee = new Employee()
                {
                    age = employeeReq.Age,
                    designation = employeeReq.Designation,
                    name = employeeReq.Name,
                    Dynamic = s

                };

                return this._xmlParser.AddElementToTheXML(filePath, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {MethodBase.GetCurrentMethod().Name} || Error Message:{ex.Message} || Stack Trace:{ex.StackTrace}");
                throw ex;
            }
        }

        public bool DeleteEmployees(string filePath, string employeeName)
        {
            try
            {
                return this._xmlParser.DeleteElementFromXML(filePath, employeeName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {MethodBase.GetCurrentMethod().Name} || Error Message:{ex.Message} || Stack Trace:{ex.StackTrace}");
                throw ex;
            }
        }


    }
   

}