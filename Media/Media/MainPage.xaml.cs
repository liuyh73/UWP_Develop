using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Media
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaPlayer mediaPlayer = new MediaPlayer();
        MediaTimelineController mediaTimelineController = new MediaTimelineController();
        TimeSpan totalTime;
        String playTime;

        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 1)
        };
        
        public MainPage()
        {
            this.InitializeComponent();
            var mediaSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/MKJ - Time (口白).mp3"));
            mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
            mediaPlayer.Source = mediaSource;
            mediaPlayer.TimelineController = mediaTimelineController;
            mediaPlayerElement.SetMediaPlayer(mediaPlayer);
            ellStoryBoard.Begin();
            mediaTimelineController.Start();
            PlayButton.Visibility = Visibility.Collapsed;
            timer.Tick += DispatcherTimer_Tick;
            timer.Start();
        }

        private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            totalTime = sender.Duration.GetValueOrDefault();
            mediaTimelineController.Position = TimeSpan.FromSeconds(0);
            //timeSlider.Value = mediaTimelineController.Position.TotalSeconds;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                timeSlider.Minimum = 0;
                timeSlider.Maximum = totalTime.TotalSeconds;
                timeSlider.StepFrequency = 1;
            });
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;
            ellStoryBoard.Begin();
            timer.Start();
            if (mediaTimelineController.State == MediaTimelineControllerState.Paused)
                mediaTimelineController.Resume();
            else
                mediaTimelineController.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            timeSlider.Value = mediaTimelineController.Position.TotalSeconds;
            playTime=mediaTimelineController.Position.ToString().Substring(0,8);
            time.Text = playTime+"/"+totalTime.ToString().Substring(0,8);

            if ((int)timeSlider.Value == (int)timeSlider.Maximum)
                Stop();
        }

        private void Stop()
        {
            mediaTimelineController.Position = TimeSpan.FromSeconds(0);
            mediaTimelineController.Pause();
            ellStoryBoard.Stop();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }
        
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;

            mediaTimelineController.Pause();
            ellStoryBoard.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private async void SourButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await SelectFile();
            if (file == null) return;

            var mediaSource = MediaSource.CreateFromStorageFile(file);
            mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
            mediaPlayer.Source = mediaSource;
            if (file.FileType == ".mp3" || file.FileType == ".wma")
            {
                ellipse.Visibility = Visibility.Visible;

                StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.SingleItem);
                if (thumbnail == null)
                    thumbNail.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/QQ.jpg"));
                else
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(thumbnail);
                    thumbNail.ImageSource = image;
                }
            }
            else
                ellipse.Visibility = Visibility.Collapsed;
        }

        private async Task<StorageFile> SelectFile()
        {
            var fop = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            fop.FileTypeFilter.Add(".wmv");
            fop.FileTypeFilter.Add(".mkv");
            fop.FileTypeFilter.Add(".mp4");
            fop.FileTypeFilter.Add(".wma");
            fop.FileTypeFilter.Add(".mp3");
            return await fop.PickSingleFileAsync();
        }

        private void FullButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
                view.ExitFullScreenMode();
            else
                view.TryEnterFullScreenMode();
            //mediaPlayerElement.IsFullWindow = !mediaPlayerElement.IsFullWindow;
        }

        private void Volumn_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaPlayer.Volume = (double)Volumn.Value;
        }
    }
}
