using NUnit.Framework;
using Moq;
using ManageEmployee.DAL;
using System.Dynamic;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


namespace ManageEmployee.BAL.Test
{
    public class Tests
    {
        Mock<IXMLParser> xmlParserMock = null;
        Mock<ILogger<ManageEmployees>> manageEmployeesMock = null;
        IManageEmployee manageEmployee = null;
      [SetUp]
        public void Setup()
        {
            xmlParserMock = new Mock<IXMLParser>();
            manageEmployeesMock = new Mock<ILogger<ManageEmployees>>();
            manageEmployee = new ManageEmployees(manageEmployeesMock.Object, xmlParserMock.Object);
        }

        [Test]
        public void AddEmployeeTest()
        {
           
            EmployeeRequest empReq = new EmployeeRequest()
            {
                Name = "Sample",
                Age = "34",
                Designation = "BE"
            };

            
            xmlParserMock.Setup(m => m.AddElementToTheXML(It.IsAny<string>(), It.IsAny<object>())).Returns(true);
            var result=manageEmployee.AddEmployees(null, empReq, It.IsAny<string>());
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteEmployeeTest()
        {

            object test = new object();
            xmlParserMock.Setup(m => m.DeleteElementFromXML(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var result = manageEmployee.DeleteEmployees("tempFilePath", "testEmployee");
            Assert.IsTrue(result);
        }
    }
}