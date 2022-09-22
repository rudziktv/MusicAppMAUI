using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
