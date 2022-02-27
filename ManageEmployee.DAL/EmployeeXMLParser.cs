using System;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ManageEmployee.DAL
{
    public class EmployeeXMLParser : IXMLParser
    {
        XmlDocument xmlDocument = null;

        private readonly ILogger<EmployeeXMLParser> _logger;
        private readonly IXMLParser _xmlParser;


        public EmployeeXMLParser(ILogger<EmployeeXMLParser> logger)
        {
            this._logger = logger;

        }

        /// <summary>
        /// Adds the new element to the XML user has provided
        /// </summary>
        /// <param name="xmlPath">xml path to insert new elements</param>
        /// <param name="employeeDetails">Employee details as a POCO object</param>
        public bool AddElementToTheXML(string xmlPath, object employeeDetails)
        {
            try
            {
                bool flag = false;
                //Read the xml
                ReadXml(xmlPath);
                if (xmlDocument != null)
                {

                    //Serialize the POCO objects to XML.
                    var node = SerializeToXmlElement(xmlDocument, employeeDetails);

                    //Create the Parent Element for the serialized XML and maps the nodes to the parent element
                    var shortcutsNode = xmlDocument.CreateElement("employee");
                    shortcutsNode.InnerXml = node;
                    xmlDocument.DocumentElement.AppendChild(shortcutsNode);


                    //save changes
                    xmlDocument.Save(xmlPath);
                    flag = true;
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //release xml object
                xmlDocument = null;
            }

        }

        public XmlNodeList SearchAnXMLElement(string xmlPath, string elementName)
        {
            try
            {
                //Read the xml
                ReadXml(xmlPath);
                XmlNodeList xmlNode = null;
                if (xmlDocument != null)
                {
                    xmlNode = xmlDocument.SelectNodes($"/employees/employee[{nameof(Employee.name)}='{elementName}']");
                }
                return xmlNode;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public bool DeleteElementFromXML(string xmlPath, string employeeName)
        {
            try
            {

                bool flag = false;
                var nodeList = SearchAnXMLElement(xmlPath, employeeName);
                if (nodeList.Count <= 0)
                {
                    _logger.LogWarning("The entered Name is not found in the xml,so no changes done");
                    return flag;
                }
                if (xmlDocument != null)
                {
                    foreach (XmlNode node in nodeList)
                    {
                        xmlDocument.DocumentElement.RemoveChild(node);
                    }
                }
                //save changes
                xmlDocument.Save(xmlPath);
                flag = true;
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //release xml object
                xmlDocument = null;
            }

        }

        #region Helpers
        /// <summary>
        /// Gets the file path of the xml to be read and processed and stores in XML Document Object
        /// </summary>
        /// <param name="xmlPath"></param>
        private void ReadXml(string xmlPath)
        {
            try
            {
                xmlDocument = new XmlDocument();
                //Load the given XML
                xmlDocument.Load(xmlPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        /// <summary>
        /// Serializes the POCO object to XML
        /// </summary>
        /// <param name="xDoc">gets the XML Document object</param>
        /// <param name="o">gets the POCO objet to be serialized as XML</param>
        /// <returns>the xml seriliazed in string format</returns>
        private static string SerializeToXmlElement(XmlDocument xDoc, object o)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                var lListOfBs = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                 from lType in lAssembly.GetTypes()
                                 where typeof(Employee).IsAssignableFrom(lType)
                                 select lType).ToArray();
                using (XmlWriter writer = doc.CreateNavigator().AppendChild())
                {
                    new XmlSerializer(o.GetType(), lListOfBs).Serialize(writer, o);
                }
                StringBuilder sb = new StringBuilder(doc.DocumentElement.InnerXml);
                var rootXML = doc.DocumentElement.InnerXml;
                string getDynamicString, processedOutput = "";

                if (rootXML.IndexOf("<Dynamic") > 0)
                {
                    getDynamicString = rootXML.Substring(rootXML.IndexOf("<Dynamic"));
                    processedOutput = ProcessDynamicTagsInXML(getDynamicString, sb);
                }
                else
                {
                    processedOutput = rootXML;
                }

                return processedOutput;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Removes the Dynamic tags and rename them to the field name that user has chosen
        /// </summary>
        /// <param name="xmlCOntent">gets the xml content where the dynamic tags has to be removed in string</param>
        /// <param name="sb">String builder that stores the full xml</param>
        /// <returns>processed xml string with no dynamic tag names</returns>
        private static string ProcessDynamicTagsInXML(string xmlCOntent, StringBuilder sb)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlCOntent);
                var s = doc.Descendants().Where(i => i.Name == "anyType");
                var extractAttribute = "";
                var xmlString = sb.ToString();
                xmlString = xmlString.Replace("<Dynamic>", "").Replace("</Dynamic>", "");
                xmlString = xmlString.Remove(xmlString.IndexOf("<anyType"));
                foreach (var node in s)
                {
                    extractAttribute = node.FirstAttribute.Value;
                    xmlString += node.ToString().Replace($"<anyType xsi:type=\"{extractAttribute}\"", $"<{extractAttribute}")
                        .Replace($"</anyType>", $"</{extractAttribute}>")
                        .Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                }
                return xmlString;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion

    }




}