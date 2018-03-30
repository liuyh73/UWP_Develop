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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace homework1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    sealed partial class MainPage : Page
    {
        public static ListItemViewModel ViewModel1 = new ListItemViewModel();
        public ListItemViewModel ViewModel { get { return ViewModel1; } }
        public MainPage()
        {
            this.InitializeComponent();
            right.Navigate(typeof(NewPage));
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
                rootFrame.Navigate(typeof(NewPage), e.ClickedItem as ListItem);
            }
            else
                right.Navigate(typeof(NewPage), e.ClickedItem as ListItem);
        }

        private void AddBarButtonClick(object sender, RoutedEventArgs e)
        {
            if (right.Visibility.ToString() == "Collapsed")
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(NewPage));
            }
            else
                right.Navigate(typeof(NewPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void MenuFlyoutEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewModel1.SelectedItem = (sender as MenuFlyoutItem).DataContext as ListItem;
            if (right.Visibility.ToString() == "Collapsed")
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(NewPage), ViewModel.SelectedItem);
            }
            else
                right.Navigate(typeof(NewPage), ViewModel.SelectedItem);
        }

        private async void MenuFlyoutDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel1.SelectedItem = (sender as MenuFlyoutItem).DataContext as ListItem;
            ViewModel1.RemoveListItem(ViewModel1.SelectedItem);
            await new MessageDialog("Delete successfully！").ShowAsync();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }
    }
}
