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

        private void GetPlaylist()
        {
            popup = new();
            Shell.Current.CurrentPage.ShowPopup(popup);
            
            //popup.Close();
            
        }
    }
}
