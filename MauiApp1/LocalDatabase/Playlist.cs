using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.LocalDatabase
{
    [Table("playlist")]
    internal class Playlist
    {
        [Column("id")]
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }

        [Column("name")]
        [NotNull]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        private string _iconPath;

        [Column("icon_path")]
        public string IconPath
        {
            get
            { 
                if (File.Exists(_iconPath))
                {
                    return _iconPath;
                }
                else
                {
                    return "new_playlist.png";
                }
            }
            set { _iconPath = value; }
        }


        [Unique]
        [Column("youtube_url")]
        public string YoutubeUrl { get; set; }

        [Unique]
        [Column("youtube_id")]
        public string YoutubeID { get; set; }
    }
}
