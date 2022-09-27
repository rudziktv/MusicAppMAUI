using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{
    internal class HomeViewModel : BaseViewModel
    {
        private string _trackTitle;

        public string TrackTitle
        {
            get { return _trackTitle; }
            set
            { 
                _trackTitle = value;
                OnPropertyChanged(nameof(TrackTitle));
            }
        }

        private string _trackAuthor;

        public string TrackAuthor
        {
            get { return _trackAuthor; }
            set
            { 
                _trackAuthor = value;
                OnPropertyChanged(nameof(TrackAuthor));
            }
        }

        private string _playIconPath;

        public string PlayIconPath
        {
            get { return _playIconPath; }
            set
            { 
                _playIconPath = value;
                OnPropertyChanged(nameof(PlayIconPath));
            }
        }


        private Thread playerUpdate;

        private string _thumbSource;

        public string ThumbSource
        {
            get { return _thumbSource; }
            set
            {
                _thumbSource = value;
                OnPropertyChanged(nameof(ThumbSource));
            }
        }

        public Command OpenPlayerCommand { get; set; }
        public Command PlayPauseCommand { get; set; }
        public Command GoToSideCommand { get; set; }

        public HomeViewModel()
        {
            //File.Delete(GlobalData.LocalDatabasePath);
            Application.Current.UserAppTheme = AppTheme.Dark;

            GlobalData.HomeViewModel = this;

            OpenPlayerCommand = new(OpenPlayer);
            PlayPauseCommand = new(PlayPause);
            GoToSideCommand = new(() => Shell.Current.GoToAsync("//SidePage"));

            playerUpdate = new(PlayerUpdate);
            playerUpdate.Start();
        }

        private void OpenPlayer()
        {
            Shell.Current.GoToAsync("//PlayerPage", true);
        }

        private void PlayerUpdate()
        {
            Shell.Current.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                TrackTitle = GlobalData.GlobalPlayer.CurrentTitle;
                TrackAuthor = GlobalData.GlobalPlayer.CurrentAuthor;

                if (GlobalData.GlobalPlayer.IsPlaying)
                {
                    PlayIconPath = Application.Current.RequestedTheme == AppTheme.Dark ? "pause_line.png" : "pause_line_black.png";
                }
                else
                {
                    PlayIconPath = Application.Current.RequestedTheme == AppTheme.Dark ? "play_line.png" : "play_line_black.png";
                }
                return true;
            });
        }

        private void PlayPause()
        {
            GlobalData.GlobalPlayer.PlayPause();

            if (GlobalData.GlobalPlayer.IsPlaying)
            {
                PlayIconPath = Application.Current.RequestedTheme == AppTheme.Dark ? "pause_line.png" : "pause_line_black.png";
            }
            else
            {
                PlayIconPath = Application.Current.RequestedTheme == AppTheme.Dark ? "play_line.png" : "play_line_black.png";
            }
        }
    }
}
