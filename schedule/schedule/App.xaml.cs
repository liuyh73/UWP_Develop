using schedule.Services;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using List.Models;
using Windows.Storage.Streams;

namespace schedule
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public bool isSuspend = false;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var composite = ApplicationData.Current.LocalSettings.Values["allItems"] as ApplicationDataCompositeValue;
            var compositeInfo = ApplicationData.Current.LocalSettings.Values["info"] as ApplicationDataCompositeValue;
            using (var conn = ScheduleDB.GetDbConnection())
            {
                var listItemDBs = conn.Table<ListItemDB>();
                foreach (var listItemDB in listItemDBs)
                {
                    //System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + listItemDB.Date);
                    BitmapImage bitmapImage = new BitmapImage();
                    var picName = listItemDB.ImgPath.Substring(listItemDB.ImgPath.LastIndexOf('\\') + 1);
                    if (picName == "pic1.ico")
                    {
                        bitmapImage=new BitmapImage(new Uri("ms-appx:///Assets\\pic1.ico"));
                    }
                    else
                    {
                        var file = await ApplicationData.Current.LocalFolder.GetFileAsync(picName);
                        IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                        await bitmapImage.SetSourceAsync(fileStream);
                    }

                    ListItem listItem = new ListItem(listItemDB.Id, bitmapImage, listItemDB.ImgPath, listItemDB.Size, listItemDB.Title, listItemDB.Detail, listItemDB.Date);
                    listItem.Finish = listItemDB.Finish;
                    MainPage.ViewModel1.AllItems.Add(listItem);
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("allItems") && (string)composite["seleted"]!="" && listItem.Id == (string)composite["seleted"])
                        MainPage.ViewModel1.SelectedItem = listItem;
                    if(ApplicationData.Current.LocalSettings.Values.ContainsKey("info") && (string)compositeInfo["seleted"] != "" && listItem.Id == (string)compositeInfo["seleted"])
                        MainPage.ViewModel1.SelectedItem = listItem;

                }
            }
            TileService.UpdateTile();

            Frame rootFrame = Window.Current.Content as Frame;
            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();
                rootFrame.Navigated += OnNavigated;
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NavigationState"))
                    {
                        rootFrame.SetNavigationState((string)ApplicationData.Current.LocalSettings.Values["NavigationState"]);
                    }
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }

        }

        private void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            MainPage.ViewModel1.SelectedItem = null;
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            isSuspend = true;
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动

            //Get the frame navigation state serialized as a string and save in settings
            Frame frame = Window.Current.Content as Frame;
            ApplicationData.Current.LocalSettings.Values["NavigationState"] = frame.GetNavigationState();
            /*
            using (var conn = ScheduleDB.GetDbConnection())
            {
                var listItemDBs = conn.Table<ListItemDB>();

                foreach (ListItem listitem in MainPage.ViewModel1.AllItems)
                {
                    bool flag = true;
                    foreach(ListItemDB listitemDB in listItemDBs)
                        if (listitemDB.Id == listitem.Id)
                        {
                            listitemDB.ImgPath = listitem.ImgPath;
                            listitemDB.Size = listitem.Size;
                            listitemDB.Title = listitem.Title;
                            listitemDB.Detail = listitem.Detail;
                            listitemDB.Date = listitem.Date;
                            listitemDB.Finish = listitem.Finish;
                            conn.Update(listitemDB);
                            flag = false;
                            break;
                        }
                    if (flag)
                    {
                        ListItemDB listItemDB = new ListItemDB(listitem.Id, listitem.ImgPath, listitem.Size, listitem.Title, listitem.Detail, listitem.Date, listitem.Finish);
                        conn.Insert(listItemDB);
                    }
                }

                foreach (ListItemDB listitemDB in listItemDBs)
                {
                    bool flag = false;
                    foreach (ListItem listitem in MainPage.ViewModel1.AllItems)
                    {
                        if (listitem.Id == listitemDB.Id)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == false)
                        conn.Delete(listitemDB);
                }
            }
            */
            using (var conn = ScheduleDB.GetDbConnection())
            {
                conn.DeleteAll<ListItemDB>();

                foreach (ListItem listitem in MainPage.ViewModel1.AllItems)
                {
                    //System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!" + listitem.Date.ToLocalTime().ToString() + "!!!!!!!!!!!!!!!!!\n");
                    ListItemDB listItemDB = new ListItemDB(listitem.Id, listitem.ImgPath, listitem.Size, listitem.Title, listitem.Detail, listitem.Date, listitem.Finish);
                    conn.Insert(listItemDB);
                }
            }
            deferral.Complete();
        }
    }
}
