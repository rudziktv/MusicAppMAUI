using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.LocalDatabase
{
    [Table("track")]
    internal class Track : TableParent
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [NotNull, Unique]
        [Column("youtube_id")]
        public string youtube_id { get; set; }

        [Unique]
        [Column("local_path")]
        public string local_path { get; set; }

        [NotNull, Unique]
        [Column("href")]
        public string href { get; set; }

        [NotNull]
        [Column("title")]
        public string title { get; set; }

        [NotNull]
        [Column("author")]
        public string author { get; set; }

        [NotNull]
        [Column("downloaded")]
        public int downloaded { get; set; }
    }
}
