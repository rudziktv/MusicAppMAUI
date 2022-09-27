using Kotlin.Properties;
using MauiApp1.LocalDatabase;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    internal class TrackModel : BaseViewModel
    {
        //identity
        public int ID { get; set; }
        //youtube vars
        public string YoutubeId { get; set; }
        public string YoutubeUrl { get; set; }
        //app vars
        public string TrackTitle { get; set; }
        public string TrackAuthor { get; set; }
        public string TrackPath { get; set; }
        public string ThumbPath { get; set; }

        private int _downloaded;

        public int Downloaded
        {
            get { return _downloaded; }
            set
            { 
                _downloaded = value;
                SetIconSource();
            }
        }

        private string _iconSource;

        public string IconSource
        {
            get { return _iconSource; }
            set
            { 
                _iconSource = value;
                OnPropertyChanged(nameof(IconSource));
            }
        }

        public TrackModel(Track track)
        {
            ID = track.ID;
            YoutubeId = track.youtube_id;
            YoutubeUrl = track.href;
            TrackTitle = track.title;
            TrackAuthor = track.author;
            TrackPath = track.local_path;
            ThumbPath = Path.Combine(GlobalData.ThumbsStorage, $"{track.youtube_id}.jpg");
            Application.Current.RequestedThemeChanged += SetIconSource;
            Downloaded = track.downloaded;
        }

        public TrackModel(int id, string youtubeId, string youtubeUrl, string trackTitle, string trackAuthor, string trackPath, string thumbPath, int downloaded, string iconSource)
        {
            ID = id;
            YoutubeId = youtubeId;
            YoutubeUrl = youtubeUrl;
            TrackTitle = trackTitle;
            TrackAuthor = trackAuthor;
            TrackPath = trackPath;
            ThumbPath = thumbPath;
            Downloaded = downloaded;
            IconSource = iconSource;
        }

        public static implicit operator Track(TrackModel trackModel)
        {
            var track = new Track()
            {
                ID = trackModel.ID,
                youtube_id = trackModel.YoutubeId,
                href = trackModel.YoutubeUrl,
                title = trackModel.TrackTitle,
                author = trackModel.TrackAuthor,
                local_path = trackModel.TrackPath,
                downloaded = trackModel.Downloaded
            };
            return track;
        }

        public static explicit operator TrackModel(Track track)
        {
            return new(track);
        }

        private void SetIconSource()
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                IconSource = Downloaded == 1 ? "download_cloud_2_fill.png" : "download_cloud_2_line.png";
            else
                IconSource = Downloaded == 1 ? "download_cloud_2_fill_black.png" : "download_cloud_2_line_black.png";
        }
            
        private void SetIconSource(object sender, AppThemeChangedEventArgs e)
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
                IconSource = Downloaded == 1 ? "download_cloud_2_fill.png" : "download_cloud_2_line.png";
            else
                IconSource = Downloaded == 1 ? "download_cloud_2_fill_black.png" : "download_cloud_2_line_black.png";
        }
    }
}
