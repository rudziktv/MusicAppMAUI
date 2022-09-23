using AngleSharp.Browser;
using MauiApp1.LocalDatabase;
using MauiApp1.Services;
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
        public ObservableCollection<Track> DownloadedTracks { get; set; }

        public Command RefreshCommand { get; set; }
        public Command<Track> PlaySelectedCommand { get; set; }

        public LibraryViewModel()
        {
            FillDownloadedTracks();
            RefreshCommand = new(FillDownloadedTracks);
            PlaySelectedCommand = new(PlaySelected);
        }

        private void PlaySelected(Track track)
        {
            GlobalData.GlobalPlayer.ChangeSource(track.local_path, track.title, track.author, track.youtube_id);
        }

        private async void FillDownloadedTracks()
        {
            DownloadedTracks ??= new();

            DownloadedTracks.Clear();
            LocalDatabaseService local_db = new();
            var a = await local_db.GetDownloadedTracks();


            foreach (var item in a)
            {
                DownloadedTracks.Add(item);
            }
        }
    }
}
