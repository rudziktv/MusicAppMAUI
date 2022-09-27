using AngleSharp.Browser;
using MauiApp1.LocalDatabase;
using CommunityToolkit.Maui.Views;

using MauiApp1.Model;
using MauiApp1.Services;
using MauiApp1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;

namespace MauiApp1.ViewModels
{
    internal class LibraryViewModel : BaseViewModel
    {
        public ObservableCollection<DownloadedTrack> DownloadedTracks { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }

        public Command DownloadedVisibleCommand { get; set; }
        public Command PlaylistsVisibleCommand { get; set; }
        public Command RefreshCommand { get; set; }
        public Command AddPlaylistCommand { get; set; }
        public Command<DownloadedTrack> PlaySelectedCommand { get; set; }
        public Command<Playlist> OpenPlaylistCommand { get; set; }
        public Command<Playlist> PlayPlaylistCommand { get; set; }

        private bool _isDownloadedVisible;

        public bool IsDownloadedVisible
        {
            get { return _isDownloadedVisible; }
            set
            { 
                _isDownloadedVisible = value;
                OnPropertyChanged(nameof(IsDownloadedVisible));
            }
        }

        private bool _isPlaylistsVisible;

        public bool IsPlaylistsVisible
        {
            get { return _isPlaylistsVisible; }
            set
            { 
                _isPlaylistsVisible = value;
                OnPropertyChanged(nameof(IsPlaylistsVisible));
            }
        }

        public LibraryViewModel()
        {
            //IsDownloadedVisible = false;
            //IsPlaylistsVisible = false;
            var popup = new SpinnerPopup();
            Shell.Current.ShowPopup(popup);

            DownloadedVisibleCommand = new(() => IsDownloadedVisible = !IsDownloadedVisible);
            PlaylistsVisibleCommand = new(() => IsPlaylistsVisible = !IsPlaylistsVisible);

            FillDownloadedTracks();
            GetPlaylists();
            RefreshCommand = new(Refresh);
            PlaySelectedCommand = new((DownloadedTrack track) => {
                GlobalData.GlobalPlayer.ChangeSource(track.local_path, track.title, track.author, track.youtube_id);
            });
            
            AddPlaylistCommand = new(() =>
            {
                GlobalData.PlaylistPopup = new AddPlaylistPage();
                Shell.Current.CurrentPage.ShowPopup(GlobalData.PlaylistPopup);
            });

            OpenPlaylistCommand = new(async (playlist) =>
            {
                await Shell.Current.GoToAsync(nameof(PlaylistPage));
                new PlaylistViewModel(playlist);
            });

            //PlayPlaylistCommand = new(async(playlist) =>
            //{
            //var local_db = new LocalDatabaseService();
            //var a = await local_db.GetTracksFromPlaylist(playlist.ID);
            //GlobalData.GlobalPlayer.PlayPlaylist(a.ToList());
            //});
            popup.Close();
        }

        private void Refresh()
        {
            FillDownloadedTracks();
            GetPlaylists();
        }

        private async void FillDownloadedTracks()
        {
            DownloadedTracks ??= new();
            DownloadedTracks.Clear();
            LocalDatabaseService local_db = new();
            var a = await local_db.GetDownloadedTracks();
            foreach (var item in a)
            {
                DownloadedTracks.Add(new DownloadedTrack(item));
            }
        }

        private void GetPlaylists()
        {
            Playlists ??= new();
            var local_db = new LocalDatabaseService();
            Playlists = local_db.GetAllPlaylists().ToObservableCollection();
            OnPropertyChanged(nameof(Playlists));
        }
    }
}
