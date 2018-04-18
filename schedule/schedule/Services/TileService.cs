using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Xml.Linq;
using List.Models;
using System.Diagnostics;

namespace schedule.Services
{
    public class TileService
    {
        public static void SetBadgeCountOnTile(int count)
        {
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            XmlElement badgeElement = (XmlElement)badgeXml.SelectSingleNode("/badge");
            badgeElement.SetAttribute("value", count.ToString());

            BadgeNotification badge = new BadgeNotification(badgeXml);
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badge);
        }

        public static XmlDocument CreateTiles(ListItem item)
        {
            XDocument xDoc = new XDocument(
                new XElement("tile", new XAttribute("version", 3),
                    new XElement("visual",
                        // Small Tile  
                        new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Schedule"), new XAttribute("template", "TileSmall"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", item.ImgPath)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", item.Title, new XAttribute("hint-style", "caption")),
                                    new XElement("text", item.Detail, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true),new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),

                        // Medium Tile  
                        new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Schedule"), new XAttribute("template", "TileMedium"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", item.ImgPath)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", item.Title, new XAttribute("hint-style", "caption")),
                                    new XElement("text", item.Date.Year+"."+ item.Date.Month+"."+ item.Date.Day, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", "Detail: "+item.Detail, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),

                        // Wide Tile  
                        new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Schedule"), new XAttribute("template", "TileWide"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", item.ImgPath)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", item.Title, new XAttribute("hint-style", "caption")),
                                    new XElement("text", item.Date.Year + "." + item.Date.Month + "." + item.Date.Day, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", "Detail: " + item.Detail, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", "This is the wide tile!", new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                ),
                                new XElement("subgroup", new XAttribute("hint-weight", 15), new XElement("image", new XAttribute("placement", "inline"), new XAttribute("src", item.ImgPath)))
                            )
                        ),

                        //Large Tile 
                        new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Schedule"), new XAttribute("template", "TileLarge"),
                            new XElement("image", new XAttribute("placement", "background"), new XAttribute("src", item.ImgPath)),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", item.Title, new XAttribute("hint-style", "caption")),
                                    new XElement("text", item.Date.Year + "." + item.Date.Month + "." + item.Date.Day, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", "Detail: " + item.Detail, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3)),
                                    new XElement("text", "This is the large tile!", new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                ),
                                new XElement("subgroup", new XAttribute("hint-weight", 15), new XElement("image", new XAttribute("placement", "inline"), new XAttribute("src", item.ImgPath)))
                            )
                        )
                    )
                )
            );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            Debug.WriteLine("\n***************************\n"+xDoc+"\n*****************************************\n");
            return xmlDoc;
        }

        public static void UpdateTile()
        {
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            SetBadgeCountOnTile(MainPage.ViewModel1.AllItems.Count);
            int count = 0;
            foreach (var item in MainPage.ViewModel1.AllItems)
            {
                count++;
                XmlDocument xmlDoc = CreateTiles(item);
                TileNotification notification = new TileNotification(xmlDoc);
                updater.Update(notification);
                if (count == 5) break;
            }
        }

        /*
        TileContent content = new TileContent()
        {
            Visual = new TileVisual()
            {
                TileSmall = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText(){ Text = "Small"}
                        }
                    }
                },

                TileMedium = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText(){ Text = "Medium"}
                        }
                    }
                },

                TileWide = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText(){ Text = "Wide"}
                        }
                    }
                },

                TileLarge = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText(){ Text = "Large"}
                        }
                    }
                },
            }
       };*/
    }
}
