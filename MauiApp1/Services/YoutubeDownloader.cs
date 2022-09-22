using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace MauiApp1.Services
{
    internal class YoutubeDownloader
    {
        public void Download(string url, string filePath)
        {
            var source = filePath;
            var youtube = YouTube.Default;
            var vid = youtube.GetVideo(url);
            File.WriteAllBytes(source, vid.GetBytes());
        }
    }
}
