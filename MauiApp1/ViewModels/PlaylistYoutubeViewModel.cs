using CommunityToolkit.Maui.Views;
using MauiApp1.Services;
using MauiApp1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    internal class PlaylistYoutubeViewModel : BaseViewModel
    {
        SpinnerPopup popup;
        public string HrefToPlaylist { get; set; }
        public Command GetPlaylistCommand { get; set; }
        public Command CancelCommand { get; set; }

        public PlaylistYoutubeViewModel()
        {
            GetPlaylistCommand = new(GetPlaylist);
            CancelCommand = new(() => GlobalData.PlaylistPopup.Close());
        }

        private async void GetPlaylist()
        {
            if (!string.IsNullOrEmpty(HrefToPlaylist))
            {
                popup = new();
                Shell.Current.CurrentPage.ShowPopup(popup);
                var local_db = new LocalDatabaseService();
                var playlist = await local_db.AddPlaylistAndVideosFromYouTube(HrefToPlaylist);
                if (playlist != null)
                {
                    popup.Close();
                    await Shell.Current.DisplayAlert("Adding playlist", "Playlist has been added to local database.", "Ok");
                    await Shell.Current.GoToAsync(nameof(PlaylistPage));
                    new PlaylistViewModel(playlist);
                    GlobalData.PlaylistPopup.Close();
                }
                else
                {
                    popup.Close();
                    await Shell.Current.DisplayAlert("Error", "Playlist has been not added. Download and adding ended with error.\nCheck your network connection and make sure your playlist is NOT PRIVATE.", "Ok");
                }
            }
        }
    }
}
