using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    internal class DownloadService
    {
        public static Thread DownloadFromYoutube(string href, string filePath)
        {
            var yt_dw = new YoutubeDownloader();

            Thread download = new(() => yt_dw.Download(href, filePath));
            download.Start();

            return download;
        }

        public static Thread DownloadThumbnail(string video_id)
        {
            string href = $"https://img.youtube.com/vi/{video_id}/0.jpg";

            HttpClient web = new();

            if (!Directory.Exists(Path.Combine(GlobalData.InternalStorageAndroid, "thumbs")))
            {
                Directory.CreateDirectory(Path.Combine(GlobalData.InternalStorageAndroid, "thumbs"));
            }

            var path = Path.Combine(GlobalData.InternalStorageAndroid, "thumbs", $"{video_id}.jpg");

            Thread download = new(async () => {
                if (!File.Exists(path))
                {
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
            download.Start();

            return download;
        }
    }
}
