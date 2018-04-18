using SQLite.Net.Attributes;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace List.Models
{
    public class ListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }

        public string ImgPath { get; set; }

        public ImageSource Img { get; set; }

        public double Size { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool? Finish { get; set; }

        public ListItem(string id, ImageSource img, string imgPath, double size, string title, string detail, DateTimeOffset date)
        {
            this.Id = id== "" ? Guid.NewGuid().ToString(): id;
            this.Img = (img == null ? new BitmapImage(new Uri("ms-appx:///Assets\\pic1.ico")) : img);
            this.ImgPath = imgPath;
            this.Size = size;
            this.Date = date;
            this.Title = title;
            this.Detail = detail;
            this.Finish = false;
        }

        private void NotityPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    public class ListItemDB
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string ImgPath { get; set; }

        public double Size { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool? Finish { get; set; }

        public ListItemDB() { }
        public ListItemDB(string id, string imgPath, double size, string title, string detail, DateTimeOffset date, bool? finish)
        {
            this.Id = id;
            this.ImgPath = imgPath;
            this.Size = size;
            this.Date = date;
            this.Title = title;
            this.Detail = detail;
            this.Finish = finish;
        }
    }
}