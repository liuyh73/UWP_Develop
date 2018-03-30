using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace List.ViewModels
{
    class ListItemViewModel
    {
        private ObservableCollection<Models.ListItem> allItems = new ObservableCollection<Models.ListItem>();
        public ObservableCollection<Models.ListItem> AllItems { get { return this.allItems; } }
        private Models.ListItem selectedItem;
        public Models.ListItem SelectedItem
        {
            get { return selectedItem; }
            set { this.selectedItem = value; }
        }

        public void AddListItem(ImageSource img, double size, string title, string detail, DateTimeOffset date)
        {
            this.allItems.Add(new Models.ListItem(img, size, title, detail, date));
        }

        public void RemoveListItem(Models.ListItem SelectedItem1)
        {
            this.allItems.Remove(SelectedItem1);
            this.selectedItem = null;
        }

        public void UpdateListItem(ImageSource img, double size, string title,string detail, DateTimeOffset date)
        {
            this.selectedItem.Img = img;
            this.selectedItem.Size = size;
            this.selectedItem.Title = title;
            this.selectedItem.Detail = detail;
            this.selectedItem.Date = date;
            this.selectedItem = null;
        }
    }
}