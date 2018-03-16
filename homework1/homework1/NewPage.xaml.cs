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
using Windows.UI.Popups;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace homework1
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

        private async void Create(object sender, RoutedEventArgs e)
        {
            string message = "";
            if (title.Text == "")
                message += "Title";
            if (detail.Text == "")
            {
                if (message != "")
                    message += ",Detail";
                else
                    message += "Detail";
            }
            if (message != "")
                message += "不得为空\n";
            if (datePicker.Date < DateTimeOffset.Now.LocalDateTime.AddDays(-1))
                message += "时间不得小于当前日期";

            string msg;
            if (message == "") msg = "创建成功!";
            else msg = "错误提示:";
            var messageDialog = new MessageDialog(message, msg);
            messageDialog.Commands.Add(new UICommand("确定", cmd => { }));

            await messageDialog.ShowAsync();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            title.Text = "";
            detail.Text = "";
            datePicker.Date = DateTime.Now;
        }

    }
}
