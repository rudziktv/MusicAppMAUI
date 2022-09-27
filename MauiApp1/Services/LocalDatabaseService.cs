using MauiApp1.LocalDatabase;
using MauiApp1.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        /// <summary>
        /// Builds database
        /// </summary>
        private void BuildDatabase()
        {
            connection.CreateTable<Track>();
            connection.CreateTable<Playlist>();
            connection.CreateTable<TrackAtPlaylist>();
            connection.Close();
        }

        /// <summary>
        /// Connects to DB
        /// </summary>
        /// <returns>Is connection is sucsesful</returns>
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

        /// <summary>
        /// Select table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="query">SQLite Query</param>
        /// <param name="args">Arguments to query, signed by '?'</param>
        /// <returns>Table</returns>
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

        /// <summary>
        /// Insert/Update/Delete from table, execute NoReader query
        /// </summary>
        /// <param name="query">SQLite Query</param>
        /// <param name="args">Arguments to query, signed by '?'</param>
        /// <returns>Number of affected rows</returns>
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

        public bool PlaylistExistsInDB(string youtube_url)
        {
            var query = $"SELECT * FROM track WHERE youtube_id='{youtube_url}';";
            return Select<Playlist>(query).Count == 1;
        }

        /// <summary>
        /// Add track to DB
        /// </summary>
        /// <param name="youtube_id">Youtube ID</param>
        /// <param name="local_path">Track local path</param>
        /// <param name="href">Track Url to youtube</param>
        /// <param name="title">Track Title</param>
        /// <param name="author">Track Author</param>
        /// <param name="downloaded">Is track downloaded</param>
        /// <returns>Return added track ID.</returns>
        public int AddTrackToDB(string youtube_id, string local_path, string href, string title, string author, bool downloaded = false)
        {
            string query;
            if (downloaded)
            {
                query = $"INSERT INTO track(youtube_id,local_path,href,title,author,downloaded)" +
                        $"VALUES ('{youtube_id}', '{local_path}', ? , '{title}', '{author}', 1);";
            }
            else
            {
                query = $"INSERT INTO track(youtube_id,local_path,href,title,author,downloaded)" +
                        $"VALUES ('{youtube_id}', '{local_path}', ? , '{title}', '{author}', 0);";
            }
            if (InsertUpdateDelete(query, href) > 0)
            {
                return Select<Track>("SELECT * FROM track ORDER BY id DESC LIMIT 1;")[0].ID;
            }
            return 0;
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

        /*
        public async Task<bool> AddPlaylistFromYouTube(string playlist_url)
        {
            if (!PlaylistExistsInDB(playlist_url))
            {
                try
                {
                    var yt = new YoutubeClient();
                    var playlist = await yt.Playlists.GetAsync(playlist_url);
                    var query = $"INSERT INTO playlist VALUES (null, '{playlist.Title}', '{playlist.Description}', 'thumb_path', '{playlist.Url}', '{playlist.Id}')";
                    return InsertUpdateDelete(query) == 0;
                }
                catch (ArgumentException)
                {
                    await Shell.Current.DisplayAlert("Invalid URL", "Entered url is not valid.\n Make sure your playlist is NOT PRIVATE.", "Ok");
                }
            }
            return false;
        }
        */

        /// <summary>
        /// Adds playlist to DB from youtube
        /// </summary>
        /// <param name="playlist_name">Title of playlist from yt</param>
        /// <param name="playlist_description">Description of playlist from yt</param>
        /// <param name="playlist_url">Url of playlist from yt</param>
        /// <param name="playlist_id">Id of playlist from yt</param>
        /// <returns>Return id of added playlist.</returns>
        public async Task<int> AddPlaylistFromYouTube(string playlist_name, string playlist_description, string playlist_url, string playlist_id)
        {
            if (!PlaylistExistsInDB(playlist_url))
            {
                try
                {
                    if (!Directory.Exists(GlobalData.PlaylistThumbsPath))
                    {
                        Directory.CreateDirectory(GlobalData.PlaylistThumbsPath);
                    }
                    var path = Path.Combine(GlobalData.PlaylistThumbsPath, $"{playlist_id}.jpg");
                    var query = $"INSERT INTO playlist VALUES (null, '{playlist_name}', '{playlist_description}', '{path}', '{playlist_url}', '{playlist_id}')";
                    if (InsertUpdateDelete(query) == 1)
                    {
                        return Select<Playlist>("SELECT * FROM playlist ORDER BY id DESC LIMIT 1;")[0].ID;
                    }                   
                }
                catch (ArgumentException)
                {
                    await Shell.Current.DisplayAlert("Invalid URL", "Entered url is not valid.\n Make sure your playlist is NOT PRIVATE.", "Ok");
                }
            }
            return 0;
        }

        /// <summary>
        /// Get track from DB
        /// </summary>
        /// <param name="youtube_id">Youtube ID</param>
        /// <returns>Track from DB</returns>
        public Track GetTrack(string youtube_id)
        {
            var query = $"SELECT * FROM track WHERE youtube_id='{youtube_id}';";
            var a = Select<Track>(query);
            return a.Count == 0 ? null : a[0];
        }

        public Playlist GetPlaylist(string youtube_id)
        {
            var query = $"SELECT * FROM playlist WHERE youtube_id='{youtube_id}';";
            var a = Select<Playlist>(query);
            return a.Count == 0 ? null : a[0];
        }

        /// <summary>
        /// Assigns track to playlist
        /// </summary>
        /// <param name="id_track">ID_track from DB</param>
        /// <param name="id_playlist">ID_playlist from DB</param>
        /// <returns>Return ID assignment</returns>
        public int AssignTrackToPlaylist(int id_track, int id_playlist)
        {
            var query = $"INSERT INTO track_at_playlist VALUES (null, {id_track}, {id_playlist});";
            if (InsertUpdateDelete(query) > 0)
            {
                return Select<TrackAtPlaylist>("SELECT * FROM track_at_playlist ORDER BY id DESC LIMIT 1;")[0].ID;
            }
            return 0;
        }

        /// <summary>
        /// Adds playlist from yt and adds videos from this playlist.
        /// </summary>
        /// <param name="playlist_url">Link to Playlist</param>
        /// <returns>Is adding playing is succesful.</returns>
        public async Task<Playlist> AddPlaylistAndVideosFromYouTube(string playlist_url)
        {
            try
            {
                var yt = new YoutubeClient();
                var playlist = await yt.Playlists.GetAsync(playlist_url);
                int id_playlist_db = await AddPlaylistFromYouTube(playlist.Title, playlist.Description, playlist.Url, playlist.Id);

                if (id_playlist_db > 0)
                {
                    await foreach (var track in yt.Playlists.GetVideosAsync(playlist_url))
                    {
                        if (!TrackExistsInDB(track.Id))
                        {
                            var path = GlobalData.GetMusicDownloadStorage($"{track.Id}.mp4");
                            var id_track_db = AddTrackToDB(track.Id, path, track.Url, track.Title, track.Author.ChannelTitle, File.Exists(path));
                            if (id_track_db <= 0)
                            {
                                return null;
                            }
                            if (AssignTrackToPlaylist(id_track_db, id_playlist_db) <= 0) return null;
                        }
                        else
                        {
                            var trackDB = GetTrack(track.Id);
                            if (AssignTrackToPlaylist(trackDB.ID, id_playlist_db) <= 0) return null;
                        }
                    }
                    return GetPlaylist(playlist.Id);
                }
            }
            catch (ArgumentException)
            {
                await Shell.Current.DisplayAlert("Invalid URL", "Entered url is not valid.\n Make sure your playlist is NOT PRIVATE.", "Ok");
            }
            return null;
        }

        public async Task<ObservableCollection<DownloadedTrack>> GetTracksFromPlaylist(int playlist_id)
        {
            var query = $"SELECT * FROM track WHERE id IN (SELECT id_track FROM track_at_playlist WHERE id_playlist = {playlist_id});";
            var tracks = Select<Track>(query);
            ObservableCollection<DownloadedTrack> result = new();

            foreach (var item in tracks)
            {
                result.Add(new(item));
                if (!File.Exists(Path.Combine(GlobalData.PlaylistThumbsPath, $"{item.youtube_id}.jpg")))
                    await DownloadService.DownloadThumbnail(item.youtube_id);
            }
            return result;
        }
    }
}
