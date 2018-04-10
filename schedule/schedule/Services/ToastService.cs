using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace schedule.Services
{
    static class ToastService
    {
        public static System.Xml.XmlDocument CreateToast()
        {
            XDocument xDoc = new XDocument(
                new XElement("toast",
                    new XElement("visual",
                        new XElement("binding", new XAttribute("template", "ToastGeneric"),
                            new XElement("text", "To Do List"),
                            new XElement("text", "Is the task complete?")
                            ) // binding  
                        ), // visual  
                    new XElement("actions",
                        new XElement("action", new XAttribute("activationType", "background"),
                            new XAttribute("content", "Yes"), new XAttribute("arguments", "yes")),
                        new XElement("action", new XAttribute("activationType", "background"),
                            new XAttribute("content", "No"), new XAttribute("arguments", "no"))
                        ) // actions  
                    )
                );

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }
    }
}
