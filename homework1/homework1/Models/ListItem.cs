using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace List.Models
{
     class ListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public string Id { get; set; }

        public ImageSource Img { get; set; }

        public double Size { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool? Finish { get; set; }

        public ListItem(ImageSource img, double size, string title, string detail, DateTimeOffset date)
        {
            //this.Id = Guid.NewGuid().ToString();
            this.Img = (img == null ? new BitmapImage(new Uri("Assets/pic3.ico")) : img);
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
}