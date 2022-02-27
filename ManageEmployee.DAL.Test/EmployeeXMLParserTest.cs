using ManageEmployee.BAL;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace ManageEmployee.DAL.Test
{
    public class EmployeeXMLParserTest
    {
      
        Mock<ILogger<EmployeeXMLParser>> xmlParserEmployeesMock = null;
        IXMLParser xmlParser = null;
        [SetUp]
        public void Setup()
        {
            xmlParserEmployeesMock = new Mock<ILogger<EmployeeXMLParser>>();
            xmlParser = new EmployeeXMLParser(xmlParserEmployeesMock.Object);
        }

        [Test]
        public void SearchAnElementInXMLTest()
        {
            var xmlNode=xmlParser.SearchAnXMLElement(AppDomain.CurrentDomain.BaseDirectory + "/EmployeeTest.xml", "Ram");
            Assert.IsTrue(xmlNode.Count > 0);
            
        }

        [Test]
        public void SearchAnElementInXMLTest_DoesNotExistInXML()
        {
            var xmlNode = xmlParser.SearchAnXMLElement(AppDomain.CurrentDomain.BaseDirectory + "/EmployeeTest.xml", "Ramesh");
            Assert.IsTrue(xmlNode.Count == 0);

        }

        [Test]
        public void DeleteAnElementInXMLTest()
        {
            var result = xmlParser.DeleteElementFromXML(AppDomain.CurrentDomain.BaseDirectory + "/EmployeeTest.xml", "Veerappan");
            Assert.IsTrue(result);

        }

        [Test]
        public void DeleteAnElementInXMLTest_NotAvaialbleInXML()
        {
            var result = xmlParser.DeleteElementFromXML(AppDomain.CurrentDomain.BaseDirectory + "/EmployeeTest.xml", "Venkatesh Iyer");
            Assert.IsFalse(result);

        }
    }
}