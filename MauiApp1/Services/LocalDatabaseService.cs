using MauiApp1.LocalDatabase;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Exceptions;

namespace MauiApp1.Services
{
    class LocalDatabaseService
    {
        private SQLiteConnection connection;

        public LocalDatabaseService()
        {
            if (!File.Exists(GlobalData.LocalDatabasePath))
            {
                File.Create(GlobalData.LocalDatabasePath);
                connection = new(GlobalData.LocalDatabasePath, GlobalData.Flags);
                BuildDatabase();
            }
        }

        private void BuildDatabase()
        {
            connection.CreateTable<Track>();
            connection.CreateTable<Playlist>();
            connection.Close();
        }

        private bool OpenConnection()
        {
            try
            {
                connection = new(GlobalData.LocalDatabasePath, GlobalData.Flags);
                return true;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        private List<T> Select<T>(string query,params object[] args) where T : new()
        {
            if (OpenConnection())
            {
                List<T> a;
                try
                {
                    a = connection.Query<T>(query, args);
                }
                catch (SQLiteException)
                {
                    a = new List<T>();
                }
                finally
                {
                    connection.Close();
                }
                return a;
            }
            return new List<T>();
        }

        private int InsertUpdateDelete(string query, params object[] args)
        {
            if (OpenConnection())
            {
                int a;
                try
                {
                    var cursor = connection.CreateCommand(query, args);
                    a = cursor.ExecuteNonQuery();
                }
                catch (SQLiteException)
                {
                    a = -1;
                }
                finally
                {
                    connection.Close();
                }
                return a;
            }
            return -2;
        }


        public bool TrackExistsInDB(string youtube_id)
        {
            var query = $"SELECT * FROM track WHERE youtube_id='{youtube_id}';";
            return Select<Track>(query).Count == 1;
        }

        public async Task<bool> TrackIsDownloaded(string youtube_id)
        {
            var query = $"SELECT * FROM track WHERE youtube_id='{youtube_id}' AND downloaded=1;";
            if (Select<Track>(query).Count == 1)
            {
                return File.Exists(Select<Track>(query)[0].local_path);
            }
            else if (Select<Track>(query).Count == 0 && File.Exists(GlobalData.GetMusicDownloadStorage(youtube_id + ".mp4")))
            {
                if (TrackExistsInDB(youtube_id))
                {
                    UpdateDownloadedTrackInDB(youtube_id);
                }
                else
                {
                    var yt = new YoutubeClient();
                    var vid = await yt.Videos.GetAsync(youtube_id);
                    AddTrackToDB(youtube_id, GlobalData.GetMusicDownloadStorage(youtube_id + ".mp4"), vid.Url, vid.Title, vid.Author.ChannelTitle, true);
                }
                return true;
            }
            else
            {
                var query2 = $"UPDATE track SET downloaded=0 WHERE youtube_id='{youtube_id}';";
                InsertUpdateDelete(query2);
                return false;
            }
        }

        public bool AddTrackToDB(string youtube_id, string local_path, string href, string title, string author, bool downloaded = false)
        {
            var query = $"INSERT INTO track(youtube_id,local_path,href,title,author,downloaded)" +
                        $"VALUES ('{youtube_id}', '{local_path}', ? , '{title}', '{author}', 0);";
            return InsertUpdateDelete(query, href) > 0;
        }

        public bool UpdateDownloadedTrackInDB(string youtube_id)
        {
            var query = $"UPDATE track SET downloaded = 1 WHERE youtube_id = '{youtube_id}';";
            return InsertUpdateDelete(query) > 0;
        }

        public async Task<List<Track>> GetDownloadedTracks()
        {
            var allTracks = GetAllTracks();

            if (Directory.Exists(GlobalData.DownloadMusicStorage))
            {
                var files = Directory.GetFiles(GlobalData.DownloadMusicStorage);

                foreach (var item in files)
                {
                    try
                    {
                        var videoId = Path.GetFileNameWithoutExtension(item);
                        if (!await TrackIsDownloaded(videoId))
                        {
                            var yt = new YoutubeClient();
                            var vid = await yt.Videos.GetAsync(videoId);
                            AddTrackToDB(videoId, item, vid.Url, vid.Title, vid.Author.ChannelTitle, true);
                        }
                    }
                    catch (YoutubeExplodeException)
                    {
                        Console.WriteLine("Video doesn't exists.");
                    }
                    catch (InvalidCastException)
                    {

                    }
                }
            }
            

            foreach (var item in allTracks)
            {
                if (item.downloaded == 0 && File.Exists(item.local_path))
                {
                    UpdateDownloadedTrackInDB(item.youtube_id);
                }
            }
            var query = $"SELECT * FROM track WHERE downloaded = 1;";
            var downloadedTracks = Select<Track>(query);
            return downloadedTracks;
        }

        public List<Track> GetAllTracks()
        {
            var query = $"SELECT * FROM track;";
            return Select<Track>(query);
        }

        public List<Playlist> GetAllPlaylists()
        {
            var query = "SELECT * FROM playlist;";
            return Select<Playlist>(query);
        }
    }
}
