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
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace homework1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class NewPage : Page
    {
        private ListItem Item;
        public NewPage()
        {
            this.InitializeComponent();
            if(this.Item == null)
                this.Item = new ListItem(pic.Source, slider.Value, "", "", DateTimeOffset.Now);
        }

        private async void Create_Update(object sender, RoutedEventArgs e)
        {
            string message = "";
            if (title.Text == "")
                message += "Title";
            if (detail.Text == "")
            {
                if (message != "")  message += ",Detail";
                else                message += "Detail";
            }
            if (message != "")      message += "不得为空\n";

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
                await new MessageDialog("Update successfully!").ShowAsync();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            pic.Source = Item.Img;
            title.Text = Item.Title;
            slider.Value = Item.Size;
            detail.Text = Item.Detail;
            datePicker.Date = Item.Date;
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
            await new MessageDialog("Delete successfully！").ShowAsync();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        private void AddBarButtonClick(object sender, RoutedEventArgs e)
        {
            if (SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility != AppViewBackButtonVisibility.Visible)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (Window.Current.Bounds.Width >= 800)
                    rootFrame.Navigate(typeof(MainPage));
                else
                    rootFrame.Navigate(typeof(NewPage));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                Item = e.Parameter as ListItem;

                DeleteAppBarButton.Visibility = Visibility.Visible;
                pic.Source = Item.Img;
                slider.Value = Item.Size;
                title.Text = Item.Title;
                detail.Text = Item.Detail;
                datePicker.Date = Item.Date;
                create_update.Content = "Update";
            }
        }
    }

    internal class GetSelectPicture
    {
        public async void selectPic(Image pic)
        {
            var fop = new FileOpenPicker();
            fop.ViewMode = PickerViewMode.Thumbnail;
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.FileTypeFilter.Add(".png");
            fop.FileTypeFilter.Add(".gif");

            Windows.Storage.StorageFile file = await fop.PickSingleFileAsync();
            try
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    pic.Source = bitmapImage;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
