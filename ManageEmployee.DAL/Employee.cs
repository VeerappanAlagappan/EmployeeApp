using System;
using System.Collections.Generic;
using System.Text;

namespace ManageEmployee.DAL
{
    public class Employee
    {
        public string name { get; set; }

        public string age { get; set; }

        public string designation { get; set; }

        public List<dynamic> Dynamic { get; set; }
    }
}
