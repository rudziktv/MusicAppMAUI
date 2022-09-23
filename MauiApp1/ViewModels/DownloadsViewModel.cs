using Android.OS;
using CommunityToolkit.Maui.Alerts;
using MauiApp1.Model;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubeExplode;

namespace MauiApp1.ViewModels
{
    class DownloadsViewModel : BaseViewModel
    {
        public ObservableCollection<DownloadElement> DownloadQueue { get; set; }

        private bool _isNotDownloading;

        public bool IsNotDownloading
        {
            get { return _isNotDownloading; }
            set
            {
                _isNotDownloading = value;
                OnPropertyChanged(nameof(IsNotDownloading));
            }
        }

        public string HrefInput { get; set; }



        public ICommand AddToQueueCommand { get; set; }
        public Command<DownloadElement> DeleteFromQueueCommand { get; set; }
        public ICommand DownloadAllCommand { get; set; }

        public DownloadsViewModel()
        {
            DownloadQueue = new();
            IsNotDownloading = true;

            AddToQueueCommand = new Command(AddToQueue);
            DeleteFromQueueCommand = new Command<DownloadElement>(DeleteFromQueue);
            DownloadAllCommand = new Command(DownloadAll);
        }

        private async void AddToQueue()
        {
            var yt = new YoutubeClient();
            var vInfo = await yt.Videos.GetAsync(HrefInput);

            var local_db = new LocalDatabaseService();

            var track_downloaded = await local_db.TrackIsDownloaded(vInfo.Id);

            if (!track_downloaded)
            {
                var name = vInfo.Id + ".mp4";
                DownloadQueue.Add(new(HrefInput, vInfo.Title, vInfo.Author.ChannelTitle, GlobalData.GetMusicDownloadStorage(name), vInfo.Id));
                local_db.AddTrackToDB(vInfo.Id,
                                                  GlobalData.GetMusicDownloadStorage(name),
                                                  HrefInput,
                                                  vInfo.Title,
                                                  vInfo.Author.ChannelTitle);
            }
            else
            {
                var toast = Toast.Make("File is on device. Download passed.", CommunityToolkit.Maui.Core.ToastDuration.Short);
                Thread thread = new(async () => { Looper.Prepare(); await toast.Show(); });
                thread.Start();
            }
        }

        private void DeleteFromQueue(DownloadElement downloadElement)
        {
            DownloadQueue.Remove(downloadElement);
        }

        private void DownloadAll()
        {
            IsNotDownloading = false;
            Thread downloadT = new(Download);
            downloadT.Start();
        }

        private void Download()
        {
            Looper.Prepare();

            foreach (var item in DownloadQueue)
            {
                if (item.IconPath != "download_cloud_2_fill.png")
                {
                    var local_db = new LocalDatabaseService();
                    YoutubeDownloader yt_dw = new();
                    yt_dw.Download(item.Href, item.LocalPath);
                    item.IconPath = "download_cloud_2_fill.png";
                    var toastText = $"Downloaded {item.SongTitle}";
                    var a = local_db.UpdateDownloadedTrackInDB(item.VideoID);
                    var toast = Toast.Make(toastText, CommunityToolkit.Maui.Core.ToastDuration.Short);
                    Thread thread = new(async () => { Looper.Prepare(); await toast.Show(); });
                    thread.Start();
                }
                else
                {
                    var toast = Toast.Make("File is on device. Download passed.", CommunityToolkit.Maui.Core.ToastDuration.Short);
                    Thread thread = new(async () => { Looper.Prepare(); await toast.Show(); });
                    thread.Start();
                }
            }
            IsNotDownloading = true;
        }
    }
}
