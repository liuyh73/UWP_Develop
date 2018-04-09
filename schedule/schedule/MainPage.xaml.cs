using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using List.ViewModels;
using Windows.UI.Popups;
using List.Models;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage.AccessCache;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace schedule
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    sealed partial class MainPage : Page
    {
        public static ListItemViewModel ViewModel1 = new ListItemViewModel();
        public ListItemViewModel ViewModel { get { return ViewModel1; } }
        //private Task<byte[]> imageDataBytes;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 2) as Line;
            line.Opacity = 1;
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 2) as Line;
            line.Opacity = 0;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel1.SelectedItem = e.ClickedItem as ListItem;
            if (right.Visibility.ToString() == "Collapsed")
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(NewPage));
            }
            else
                Assignment();
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
                //Debug.WriteLine(pic.Source.ToString()+"****************************");
                MainPage.ViewModel1.UpdateListItem(pic.Source, slider.Value, title.Text, detail.Text, datePicker.Date);
                rootFrame.Navigate(typeof(MainPage));
                ViewModel1.SelectedItem = null;
                await new MessageDialog("Update successfully!").ShowAsync();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Assignment();
        }

        /*public async Task<ImageSource> ToImageSource(byte[] source)
        {
            using (var stream = new MemoryStream(source))
            {
                var bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                return bitmap;
            }
        }

        public async Task<byte[]> ToBytes(ImageSource source)
        {
            var image = new Image() { Source = source };
            var bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(image);
            var buffer = await bitmap.GetPixelsAsync();
            using (var dataReader = DataReader.FromBuffer(await bitmap.GetPixelsAsync()))
            {
                var bytes = new byte[buffer.Length];
                await dataReader.LoadAsync(buffer.Length);
                dataReader.ReadBytes(bytes);
                return bytes;
            }
        }*/
        private void GetPicture(object sender, RoutedEventArgs e)
        {
            var getSelectPicture = new GetSelectPicture();
            getSelectPicture.selectPic(pic);
        }

        private void AddBarButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel1.SelectedItem = null;
            Frame rootFrame = Window.Current.Content as Frame;
            if (right.Visibility.ToString() == "Collapsed")
                rootFrame.Navigate(typeof(NewPage));
            else
                rootFrame.Navigate(typeof(MainPage));
        }

        private async void DeleteBarButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteAppBarButton.Visibility = Visibility.Collapsed;
            MainPage.ViewModel1.RemoveListItem(MainPage.ViewModel1.SelectedItem);
            ViewModel1.SelectedItem = null;
            Assignment();
            await new MessageDialog("Delete successfully！").ShowAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).isSuspend)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();

                //imageDataBytes = ToBytes(pic.Source);
                List<ListItem> listItems = new List<ListItem>();
                for (var i = 0; i < ViewModel1.AllItems.Count(); i++)
                    composite["ListItem" + i] = ViewModel1.AllItems[i].Finish;
                composite["title"] = title.Text;
                composite["detail"] = detail.Text;
                composite["date"] = datePicker.Date;
                composite["size"] = slider.Value;
                composite["create_update"] = create_update.Content;
                composite["imgname"] = GetSelectPicture.picName;
                ApplicationData.Current.LocalSettings.Values["allItems"] = composite;
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            if (e.NavigationMode == NavigationMode.New)
            {
                Debug.WriteLine("restore!!!");
                ApplicationData.Current.LocalSettings.Values.Remove("allItems");
            }
            else
            {
                Debug.WriteLine("restore!!");
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("allItems"))
                {
                    Debug.WriteLine("restore!");
                    var composite = ApplicationData.Current.LocalSettings.Values["allItems"] as ApplicationDataCompositeValue;

                    for (var i = 0; i < ViewModel1.AllItems.Count(); i++)
                        ViewModel1.AllItems[i].Finish = (bool?)composite["ListItem" + i];
                    title.Text = (string)composite["title"];
                    detail.Text = (string)composite["detail"];
                    datePicker.Date = (DateTimeOffset)composite["date"];
                    slider.Value = (double)composite["size"];
                    create_update.Content = (string)composite["create_update"];
                    GetSelectPicture.picName = (string)composite["imgname"];

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
                    // pic.Source = await ToImageSource((byte[])composite["imageData"]);
                    ApplicationData.Current.LocalSettings.Values.Remove("allItems");
                }
            }
        }

        private void MenuFlyoutEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewModel1.SelectedItem = (sender as MenuFlyoutItem).DataContext as ListItem;
            if (right.Visibility.ToString() == "Collapsed")
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(NewPage));
            }
            else
                Assignment();
        }

        private async void MenuFlyoutDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel1.SelectedItem = (sender as MenuFlyoutItem).DataContext as ListItem;
            ViewModel1.RemoveListItem(ViewModel1.SelectedItem);
            ViewModel1.SelectedItem = null;
            Assignment();
            await new MessageDialog("Delete successfully！").ShowAsync();
        }

        private void Assignment()
        {
            if (ViewModel1.SelectedItem == null)
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
                pic.Source = ViewModel1.SelectedItem.Img;
                title.Text = ViewModel1.SelectedItem.Title;
                slider.Value = ViewModel1.SelectedItem.Size;
                detail.Text = ViewModel1.SelectedItem.Detail;
                datePicker.Date = ViewModel1.SelectedItem.Date;
                create_update.Content = "Update";
            }
        }
    }

    internal class GetSelectPicture
    {
        public static string picName = "";
        public async void selectPic(Image pic)
        {
            var fop = new FileOpenPicker();
            fop.ViewMode = PickerViewMode.Thumbnail;
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.FileTypeFilter.Add(".png");
            fop.FileTypeFilter.Add(".gif");
            fop.FileTypeFilter.Add(".ico");

            Windows.Storage.StorageFile file = await fop.PickSingleFileAsync();
            try
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    pic.Source = bitmapImage;

                    picName = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                    await file.CopyAsync(ApplicationData.Current.LocalFolder, picName, NameCollisionOption.ReplaceExisting);
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
