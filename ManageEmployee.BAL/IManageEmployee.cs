using System;
using System.Collections.Generic;
using System.Text;

namespace ManageEmployee.BAL
{
    public interface IManageEmployee
    {
        bool AddEmployees(dynamic s, EmployeeRequest employeeReq, string filePath);

        bool DeleteEmployees(string filePath, string employeeName);
    }
}