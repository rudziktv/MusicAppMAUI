using MauiApp1.LocalDatabase;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    internal class DownloadedTrack : Track
    {
        public string ThumbSource { get; set; }

        public DownloadedTrack(Track track)
        {
            title = track.title;
            author = track.author;
            href = track.href;
            ID = track.ID;
            youtube_id = track.youtube_id;
            downloaded = track.downloaded;
            local_path = track.local_path;

            ThumbSource = Path.Combine(GlobalData.InternalStorageAndroid, "thumbs", $"{youtube_id}.jpg");
        }
    }
}
