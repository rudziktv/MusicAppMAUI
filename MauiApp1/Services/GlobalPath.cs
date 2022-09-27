using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    internal class GlobalPath
    {
        public const string MusicDownloadFolderName = "Music";
        public const string TempFolderName = "Temps";
        public const string PlaylistThumbs = "Playlists";

        public static string InternalStorageAndroid { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        
        public static string DownloadMusicStorage = Path.Combine(InternalStorageAndroid, MusicDownloadFolderName);
        public static string LocalDatabasePath = Path.Combine(InternalStorageAndroid, "metadata.sqlite3");
        public static string PlaylistThumbsPath = Path.Combine(InternalStorageAndroid, PlaylistThumbs);

    }
}
