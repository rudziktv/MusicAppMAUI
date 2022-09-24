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
            DownloadedVisibleCommand = new(() => IsDownloadedVisible = !IsDownloadedVisible);
            PlaylistsVisibleCommand = new(() => IsPlaylistsVisible = !IsPlaylistsVisible);

            FillDownloadedTracks();
            GetPlaylists();
            RefreshCommand = new(FillDownloadedTracks);
            PlaySelectedCommand = new((DownloadedTrack track) => GlobalData.GlobalPlayer.ChangeSource(track.local_path, track.title, track.author, track.youtube_id));
            
            AddPlaylistCommand = new(() =>
            {
                GlobalData.LibraryPage.ShowPopupPl();
            });

            Playlists ??= new();

            Playlists.Add(new());
            Playlists.Add(new());
            Playlists.Add(new());
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
            //Playlists.Clear();

            //LocalDatabaseService local_db = new();
            //var a = local_db.GetAllPlaylists();

            //foreach (var item in a)
            //{
                //Playlists.Add(item);
            //}
        }
    }
}
