using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Core;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using List.Models;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using System.Diagnostics;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
        }

        private async void Create_Update(object sender, RoutedEventArgs e)
        {
            string message = "";
            if (title.Text == "")
                message += "Title";
            if (detail.Text == "")
            {
                if (message != "") message += ",Detail";
                else message += "Detail";
            }
            if (message != "") message += "不得为空\n";

            if (datePicker.Date < DateTimeOffset.Now.LocalDateTime.AddDays(-1))
                message += "时间不得小于当前日期";

            if (message != "")
                await new MessageDialog(message).ShowAsync();
            else if (create_update.Content.ToString() == "Create")
            {
                Frame rootFrame = Window.Current.Content as Frame;
                MainPage.ViewModel1.AddListItem(pic.Source, slider.Value, title.Text, detail.Text, datePicker.Date);
                rootFrame.Navigate(typeof(MainPage));
                await new MessageDialog("Create successfully!").ShowAsync();
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;
                MainPage.ViewModel1.UpdateListItem(pic.Source, slider.Value, title.Text, detail.Text, datePicker.Date);
                rootFrame.Navigate(typeof(MainPage));
                MainPage.ViewModel1.SelectedItem = null;
                await new MessageDialog("Update successfully!").ShowAsync();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Assignment();
        }

        private void GetPicture(object sender, RoutedEventArgs e)
        {
            var getSelectPicture = new GetSelectPicture();
            getSelectPicture.selectPic(pic);
        }

        private async void DeleteBarButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteAppBarButton.Visibility = Visibility.Collapsed;
            MainPage.ViewModel1.RemoveListItem(MainPage.ViewModel1.SelectedItem);
            MainPage.ViewModel1.SelectedItem = null;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
            await new MessageDialog("Delete successfully！").ShowAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).isSuspend)
            {
                Debug.WriteLine("infoStore!======================");
                ApplicationDataCompositeValue compositeInfo = new ApplicationDataCompositeValue
                {
                    ["title"] = title.Text,
                    ["detail"] = detail.Text,
                    ["date"] = datePicker.Date,
                    ["size"] = slider.Value,
                    ["create_update"] = create_update.Content,
                    ["imgname"] = GetSelectPicture.picName
                };
                Debug.WriteLine(compositeInfo["title"] + " " + compositeInfo["detail"] + " " + compositeInfo["date"]);
                ApplicationData.Current.LocalSettings.Values["info"] = compositeInfo;
                Debug.WriteLine(compositeInfo + " ");
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Assignment();
            if (e.NavigationMode == NavigationMode.New)
            {
                Debug.WriteLine("infoRestore????????????????????");
                ApplicationData.Current.LocalSettings.Values.Remove("info");
            }
            else
            {
                Debug.WriteLine("infoRestore!!!!!!!!!!!!!!!!!!");
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("info"))
                {
                    Debug.WriteLine("infoRestore!");
                    var compositeInfo = ApplicationData.Current.LocalSettings.Values["info"] as ApplicationDataCompositeValue;
                    title.Text = (string)compositeInfo["title"];
                    detail.Text = (string)compositeInfo["detail"];
                    datePicker.Date = (DateTimeOffset)compositeInfo["date"];
                    slider.Value = (double)compositeInfo["size"];
                    create_update.Content = (string)compositeInfo["create_update"];

                    GetSelectPicture.picName = (string)compositeInfo["imgname"];
                    if (GetSelectPicture.picName == "")
                        pic.Source = new BitmapImage(new Uri("ms-appx:///Assets/pic3.ico"));
                    else
                    {
                        var file = await ApplicationData.Current.LocalFolder.GetFileAsync(GetSelectPicture.picName);
                        IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                        BitmapImage bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(fileStream);
                        pic.Source = bitmapImage;
                    }

                    ApplicationData.Current.LocalSettings.Values.Remove("info");
                }
            }
        }

        private void Assignment()
        {
            if (MainPage.ViewModel1.SelectedItem == null)
            {
                DeleteAppBarButton.Visibility = Visibility.Collapsed;
                pic.Source = new BitmapImage(new Uri("ms-appx:///Assets/pic3.ico"));
                title.Text = "";
                slider.Value = 0.4;
                detail.Text = "";
                datePicker.Date = DateTimeOffset.Now;
                create_update.Content = "Create";
            }
            else
            {
                DeleteAppBarButton.Visibility = Visibility.Visible;
                pic.Source = MainPage.ViewModel1.SelectedItem.Img;
                title.Text = MainPage.ViewModel1.SelectedItem.Title;
                slider.Value = MainPage.ViewModel1.SelectedItem.Size;
                detail.Text = MainPage.ViewModel1.SelectedItem.Detail;
                datePicker.Date = MainPage.ViewModel1.SelectedItem.Date;
                create_update.Content = "Update";
            }
        }
    }
}
