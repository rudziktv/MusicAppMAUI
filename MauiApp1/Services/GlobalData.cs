using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    internal static class GlobalData
    {
        public static string InternalStorageAndroid { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string MusicDownloadFolderName { get; set; } = "Music";

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
