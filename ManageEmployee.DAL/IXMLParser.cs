
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ManageEmployee.DAL
{
    public interface IXMLParser
    {
        bool AddElementToTheXML(string xmlPath, object elementDetails);
        bool DeleteElementFromXML(string xmlPath, string elementName);

        XmlNodeList SearchAnXMLElement(string xmlPath, string elementName);
    }
}