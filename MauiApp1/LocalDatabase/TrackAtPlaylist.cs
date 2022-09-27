using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.LocalDatabase
{
    [Table("track_at_playlist")]
    internal class TrackAtPlaylist
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }
        [NotNull]
        [Column("id_track")]
        public int ID_track { get; set; }
        [NotNull]
        [Column("id_playlist")]
        public int ID_playlist { get; set; }
    }
}
