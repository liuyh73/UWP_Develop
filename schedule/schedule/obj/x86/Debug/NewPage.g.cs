﻿#pragma checksum "D:\coursewave\现代操作系统应用开发\VS_Projects\schedule\schedule\NewPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E10228B0D39DA1FD36224B1308955C53"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace schedule
{
    partial class NewPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.create_update = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 42 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.create_update).Click += this.Create_Update;
                    #line default
                }
                break;
            case 2:
                {
                    this.cancel = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 43 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.cancel).Click += this.Cancel;
                    #line default
                }
                break;
            case 3:
                {
                    this.pic = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 4:
                {
                    this.slider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                }
                break;
            case 5:
                {
                    this.title = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6:
                {
                    this.detail = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7:
                {
                    this.datePicker = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 8:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element8 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 31 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element8).Click += this.GetPicture;
                    #line default
                }
                break;
            case 9:
                {
                    this.DeleteAppBarButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 50 "..\..\..\NewPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.DeleteAppBarButton).Click += this.DeleteBarButtonClick;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

