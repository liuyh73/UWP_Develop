using Windows.Data.Xml.Dom;
using System.Xml.Linq;
using Windows.UI.Notifications;

namespace schedule.Services
{
    public class ToastService
    {
        public static XmlDocument CreateToast()
        {
            XDocument xDoc = new XDocument(
                new XElement("toast",
                    new XElement("visual",
                        new XElement("binding", new XAttribute("template", "ToastGeneric"),
                            new XElement("text", "ListItem"),
                            new XElement("text", "You have created a new item!")
                        )
                    ),
                    new XElement("actions",
                        new XElement("action", new XAttribute("activationType", "foreground"),
                            new XAttribute("content", "Yes"), new XAttribute("arguments", "yes"))
                        /*new XElement("action", new XAttribute("activationType", "background"),
                            new XAttribute("content", "No"), new XAttribute("arguments", "no"))*/
                    )
                )
            );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }

        public static XmlDocument UpdateToast()
        {
            XDocument xDoc = new XDocument(
                new XElement("toast",
                    new XElement("visual",
                        new XElement("binding", new XAttribute("template", "ToastGeneric"),
                            new XElement("text", "ListItem"),
                            new XElement("text", "You have updated the item!")
                        )
                    ),
                    new XElement("actions",
                        new XElement("action", new XAttribute("activationType", "foreground"),
                            new XAttribute("content", "Yes"), new XAttribute("arguments", "yes"))
                    /*new XElement("action", new XAttribute("activationType", "background"),
                        new XAttribute("content", "No"), new XAttribute("arguments", "no"))*/
                    )
                )
            );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }

        public static XmlDocument DeleteToast()
        {
            XDocument xDoc = new XDocument(
                new XElement("toast",
                    new XElement("visual",
                        new XElement("binding", new XAttribute("template", "ToastGeneric"),
                            new XElement("text", "ListItem"),
                            new XElement("text", "You have deleted a item!")
                        )
                    ),
                    new XElement("actions",
                        new XElement("action", new XAttribute("activationType", "foreground"),
                            new XAttribute("content", "Yes"), new XAttribute("arguments", "yes"))
                    /*new XElement("action", new XAttribute("activationType", "background"),
                        new XAttribute("content", "No"), new XAttribute("arguments", "no"))*/
                    )
                )
            );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }

        public static void CreateNotify()
        {
            var xmlDoc = CreateToast();
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            ToastNotification toast = new ToastNotification(xmlDoc);
            notifier.Show(toast);
        }

        public static void UpdateNotify()
        {
            var xmlDoc = UpdateToast();
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            ToastNotification toast = new ToastNotification(xmlDoc);
            notifier.Show(toast);
        }

        public static void DeleteNotify()
        {
            var xmlDoc = DeleteToast();
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            ToastNotification toast = new ToastNotification(xmlDoc);
            notifier.Show(toast);
        }
    }
}
