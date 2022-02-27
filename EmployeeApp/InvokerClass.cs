using ManageEmployee.BAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EmployeeApp
{
    class InvokerClass : IInvoker
    {
        private List<IExecute> objArrayList = new List<IExecute>();
        private readonly ILogger<InvokerClass> _logger;
        private readonly IManageEmployee _manageEmployee;
        private readonly ILogger<AddEmployee> _logger1;
        private readonly ILogger<DeleteEmployee> _logger2;
        public InvokerClass(ILogger<InvokerClass> logger, ILogger<AddEmployee> logger1, ILogger<DeleteEmployee> logger2, IManageEmployee manageEmployee)
        {
            _logger = logger;
            _logger1 = logger1;
            _logger2 = logger2;
            this._manageEmployee = manageEmployee;

            initComponent();

        }

        public void initComponent()
        {
            objArrayList.Add(new AddEmployee(_logger1, _manageEmployee));
            objArrayList.Add(new DeleteEmployee(_logger2, _manageEmployee));
        }

        public bool getCommand(int getUsrInput)
        {
            bool flag = false;
            foreach (IExecute obj in objArrayList)
            {
                if (obj.CommandName == getUsrInput)
                {
                    flag = obj.Execute();
                }
            }
            return flag;

        }

    }

    public interface IInvoker
    {
        bool getCommand(int getUsrInput);
    }

    public interface IExecute
    {
        int CommandName { get; set; }
        bool Execute();
    }

    enum Actions
    {
        Add = 1,
        Delete = 2

    }
}