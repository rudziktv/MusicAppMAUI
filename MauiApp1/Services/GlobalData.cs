using MauiApp1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    internal static class GlobalData
    {
        public static PlayerService GlobalPlayer { get; set; } = new();
        public static PlayerViewModel PlayerViewModel { get; set; }

        public static string InternalStorageAndroid { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public const string MusicDownloadFolderName = "Music";
        public const string TempFolderName = "Temps";

        //public static string TempStorage = Path.Combine(InternalStorageAndroid, TempStorage);

        public static string DownloadMusicStorage = Path.Combine(InternalStorageAndroid, MusicDownloadFolderName);
        public static string LocalDatabasePath = Path.Combine(InternalStorageAndroid, "metadata.sqlite3");
        public static string LastThumbPath;

        public static string GetMusicDownloadStorage(string fileName)
        {
            var downloadMusicStorage = Path.Combine(InternalStorageAndroid, MusicDownloadFolderName);
            
            if (!Directory.Exists(downloadMusicStorage))
            {
                Directory.CreateDirectory(downloadMusicStorage);
            }

            return Path.Combine(downloadMusicStorage, fileName);
        }
    }
}
