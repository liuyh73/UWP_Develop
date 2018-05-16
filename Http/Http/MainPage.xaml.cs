using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Http
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public async void SearchWeather(object sender, RoutedEventArgs e)
        {
            if (WeatherBox.Text == "")
                await new MessageDialog("Please input the city!").ShowAsync();
            else
            {
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                Uri requestUri = new Uri("https://www.sojson.com/open/api/weather/json.shtml?city="+WeatherBox.Text);
                //Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
                String WeatherInfo = "";
                try
                {
                    //httpResponse = await httpClient.GetAsync(requestUri);
                    //httpResponse.EnsureSuccessStatusCode();
                    //WeatherInfo = await httpResponse.Content.ReadAsStringAsync();
                    WeatherInfo = await httpClient.GetStringAsync(requestUri);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Data);
                }

                JArray FoercastWeather = (JArray)JObject.Parse(WeatherInfo)["data"]["forecast"];
                Debug.WriteLine(FoercastWeather);
                ShowWeather(FoercastWeather);
            }
        }

        public async void SearchIP(object sender, RoutedEventArgs e)
        {
            if (IPBox.Text == "")
                await new MessageDialog("Please input the IP!").ShowAsync();
            else
            {
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                Uri requestUri = new Uri("http://ip-api.com/xml/" + IPBox.Text);
                
                String IPInfo = "";
                try
                {
                    //httpResponse = await httpClient.GetAsync(requestUri);
                    //httpResponse.EnsureSuccessStatusCode();
                    //WeatherInfo = await httpResponse.Content.ReadAsStringAsync();
                    IPInfo = await httpClient.GetStringAsync(requestUri);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Data);
                }
                Debug.WriteLine(IPInfo);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(IPInfo);
                ShowIPAddress(xmldoc);
                //ShowWeather(FoercastWeather);
            }
        }

        public void ShowWeather(JArray weather)
        {
            StringBuilder WeatherDetail = new StringBuilder();
            foreach(JObject o in weather)
            {
                WeatherDetail.Append("Date: ").Append(o["date"]).Append("\nSunrise: ").Append(o["sunrise"]).
                    Append("\nHigh: ").Append(o["high"]).Append("\nLow: ").Append(o["low"]).Append("\nSunset: ").
                    Append(o["sunset"]).Append("\ntype: ").Append(o["type"]).Append("\n--------****--------\n");
            }
            WeatherBlock.Text = WeatherDetail.ToString();
        }

        public void ShowIPAddress(XmlDocument ipinfo)
        {
            StringBuilder IPDetail = new StringBuilder();
            IPDetail.Append("Country: ").Append(ipinfo["query"]["country"].InnerText).Append("\n\nRegion: ").Append(ipinfo["query"]["regionName"].InnerText).
                Append("\n\nCity: ").Append(ipinfo["query"]["city"].InnerText);
            IPBlock.Text = IPDetail.ToString();
        }

    }
}
