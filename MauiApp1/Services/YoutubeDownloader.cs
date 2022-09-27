using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace MauiApp1.Services
{
    internal class YoutubeDownloader
    {
        public static Task DownloadVideo(string url, string filePath)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var source = filePath;
                    var youtube = YouTube.Default;
                    var vid = youtube.GetVideo(url);
                    File.WriteAllBytes(source, vid.GetBytes());
                }
                catch (VideoLibrary.Exceptions.UnavailableStreamException)
                {
                    await Shell.Current.DisplayAlert("Error", "Track is not avaible, skipping...", "Ok");
                }
                catch (TargetInvocationException)
                {
                    await Shell.Current.DisplayAlert("Error", "Track is not avaible, skipping...", "Ok");
                }
            });
        }

        public static Task DownloadThumbnail(string video_id)
        {
            return Task.Run(async () =>
            {
                var path = Path.Combine(GlobalData.InternalStorageAndroid, "thumbs", $"{video_id}.jpg");
                var href = $"https://img.youtube.com/vi/{video_id}/0.jpg";

                if (!Directory.Exists(Path.Combine(GlobalData.InternalStorageAndroid, "thumbs")))
                {
                    Directory.CreateDirectory(Path.Combine(GlobalData.InternalStorageAndroid, "thumbs"));
                }

                if (!File.Exists(path))
                {
                    var web = new HttpClient();
                    var bytes = await web.GetByteArrayAsync(href);
                    File.WriteAllBytes(path, bytes);
                }
                GlobalData.HomeViewModel.ThumbSource = path;
                if (GlobalData.PlayerViewModel != null)
                {
                    GlobalData.PlayerViewModel.ThumbSource = path;
                }
                else
                {
                    GlobalData.LastThumbPath = path;
                }
            });
        }
    }
}