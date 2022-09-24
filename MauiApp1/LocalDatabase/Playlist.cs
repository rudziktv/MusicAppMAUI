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

        [Column("icon_path")]
        public string IconPath { get; set; }
    }
}
