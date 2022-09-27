using CommunityToolkit.Maui.Views;
using MauiApp1.LocalDatabase;
using MauiApp1.Model;
using MauiApp1.Services;
using MauiApp1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    internal class PlaylistViewModel : BaseViewModel
    {
        private Playlist playlist;

        public string PlaylistName { get; set; }
        public string PlaylistDescription { get; set; }
        public string PlaylistIconPath { get; set; }

        public Command GoBackCommand { get; set; }
        public Command<TrackModel> PlaySelectedCommand { get; set; }

        public Command<TrackModel> DownloadTrackCommand { get; set; }
        public Command DownloadPlaylistCommand { get; set; }
        public ObservableCollection<TrackModel> Tracks { get; set; }

        public PlaylistViewModel(Playlist playlist)
        {
            this.playlist = playlist;
            PlaylistIconPath = playlist.IconPath;

            

            Tracks = new();
            RefreshTracks();

            GoBackCommand = new(() =>
            {
                Shell.Current.GoToAsync("//LibraryPage", true);
            });

            PlaySelectedCommand = new((track) =>
            {
                //await Download();
                GlobalData.GlobalPlayer.ClearQueue();
                GlobalData.GlobalPlayer.PlayPlaylist(Tracks.ToList(), Tracks.IndexOf(track));
            });

            DownloadTrackCommand = new(async (trackModel) =>
            {
                await Shell.Current.DisplayAlert("Download", $"Download track: {trackModel.YoutubeId}", "Ok");
            });

            PlaylistName = playlist.Name;
            PlaylistDescription = playlist.Description;

            Shell.Current.CurrentPage.BindingContext = this;
        }

        private async Task RefreshTracks()
        {
            var local_db = new LocalDatabaseService();
            var a = await local_db.GetTracksFromPlaylist(playlist.ID);
            foreach (var item in a)
            {
                Tracks.Add(new(item));
            }
            OnPropertyChanged(nameof(Tracks));
        }

        
    }
}
