using MauiApp1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    class DownloadElement : BaseViewModel
    {
        public string Href { get; }
        public string SongTitle { get; set; }
        public string SongAuthor { get; set; }
        public string LocalPath { get; set; }
        public string VideoID { get; set; }

        private string _iconPath;

        public string IconPath
        {
            get { return _iconPath; }
            set
            {
                OnPropertyChanged(nameof(IconPath));
                _iconPath = value;
            }
        }




        public DownloadElement(string href, string songTitle, string songAuthor, string localPath, string videoID)
        {
            Href = href;
            SongTitle = songTitle;
            SongAuthor = songAuthor;
            LocalPath = localPath;
            VideoID = videoID;
            IconPath = "download_cloud_2_line.png";
        }
    }
}
