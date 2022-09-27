using Android.OS;
using CommunityToolkit.Maui.Alerts;
using MauiApp1.LocalDatabase;
using MauiApp1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Android.Content.ClipData;

namespace MauiApp1.Services
{
    internal class DownloadService
    {
        public List<Track> DownloadQueue { get; set; }
        public Task CurrentTask { get; set; }

        public DownloadService()
        {
            DownloadQueue = new();
        }

        public Task RunQueue()
        {
            CurrentTask = Task.Run(async () =>
            {
                foreach (var item in DownloadQueue)
                {
                    await DownloadThumbAndVideo(item);
                }
                DownloadQueue.Clear();
            });
            return CurrentTask;
        }

        public Task RunQueue(ObservableCollection<DownloadedTrack> DownloadedTrack)
        {
            CurrentTask = Task.Run(async () =>
            {
                foreach (var item in DownloadedTrack)
                {
                    await DownloadThumbAndVideo(item);
                }
                DownloadedTrack.Clear();
            });
            return CurrentTask;
        }

        public Task RunQueue(ObservableCollection<DownloadElement> DownloadQueue)
        {
            CurrentTask = Task.Run(async () =>
            {
                foreach (var item in DownloadQueue)
                {
                    if (item.IconPath != "download_cloud_2_fill.png") await DownloadThumbAndVideo(item);
                }
            });
            return CurrentTask;
        }

        public static async Task DownloadVideo(string href, string filePath)
        {
            await YoutubeDownloader.DownloadVideo(href, filePath);
        }

        public static async Task DownloadThumbnail(string video_id)
        {
            await YoutubeDownloader.DownloadThumbnail(video_id);
        }

        public static Task DownloadThumbAndVideo(Track track)
        {
            return Task.Run(async () =>
            {
                await YoutubeDownloader.DownloadVideo(track.href, track.local_path);
                await YoutubeDownloader.DownloadThumbnail(track.youtube_id);
            });
        }

        public static Task DownloadThumbAndVideo(DownloadElement track)
        {
            return Task.Run(async () =>
            {
                await YoutubeDownloader.DownloadVideo(track.Href, track.LocalPath);
                await YoutubeDownloader.DownloadThumbnail(track.VideoID);
                track.IconPath = "download_cloud_2_fill.png";
                var toastText = $"Downloaded {track.SongTitle}";
                var toast = Toast.Make(toastText, CommunityToolkit.Maui.Core.ToastDuration.Short);
                new Thread(async () => { Looper.Prepare(); await toast.Show(); }).Start();
            });
        }
    }
}