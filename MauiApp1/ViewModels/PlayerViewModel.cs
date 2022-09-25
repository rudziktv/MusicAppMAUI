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
    internal class PlayerViewModel : BaseViewModel
    {
		private Thread timelineThread;

		private float _currentProgress;

		public float CurrentProgress
		{
			get { return _currentProgress; }
			set
			{ 
				_currentProgress = value;
				OnPropertyChanged(nameof(CurrentProgress));
			}
		}

		private string _playButtonIcon;

		public string PlayButtonIcon
		{
			get { return _playButtonIcon; }
			set
			{ 
				_playButtonIcon = value;
				OnPropertyChanged(nameof(PlayButtonIcon));
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




		private bool _isPlaying;

		public bool IsPlaying
		{
			get { return _isPlaying; }
			set
			{ 
				_isPlaying = value;
				OnPropertyChanged(nameof(IsPlaying));
				PlayButtonIcon = IsPlaying ? "pause_line.png" : "play_line.png";
            }
		}

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



		public Command GoBackCommand { get; set; }
		public Command OpenPlayerContextCommand { get; set; }
		public Command PlayPauseCommand { get; set; }
		public Command SeekToCommand { get; set; }

		public PlayerViewModel()
		{
			GoBackCommand = new(GoBack);
			OpenPlayerContextCommand = new(() => Shell.Current.ShowPopup(new PlayerContextMenu()));
			PlayPauseCommand = new(PlayPause);
			SeekToCommand = new(SeekTo);

            ThumbSource = GlobalData.LastThumbPath;

            timelineThread = new(TimelineUpdate);
			timelineThread.Start();

			GlobalData.PlayerViewModel = this;
		}

		private void GoBack()
		{
			Shell.Current.GoToAsync("//HomePage", true);
		}

		private void PlayPause()
		{
			GlobalData.GlobalPlayer.PlayPause();
			if (GlobalData.GlobalPlayer.IsPlaying)
			{
				PlayIconPath = "pause_line.png";

            }
			else
			{
				PlayIconPath = "play_line.png";

            }
		}

		private void SeekTo()
		{
			GlobalData.GlobalPlayer.SeekTo(CurrentProgress);
		}

		private void TimelineUpdate()
		{
            GlobalData.GlobalPlayer.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(500), () => {
                PlayIconPath = GlobalData.GlobalPlayer.IsPlaying ? "pause_line.png" : "play_line.png";
                if (GlobalData.GlobalPlayer.IsPlaying)
                {
					TrackTitle = GlobalData.GlobalPlayer.CurrentTitle;
					TrackAuthor = GlobalData.GlobalPlayer.CurrentAuthor;
                    CurrentProgress = GlobalData.GlobalPlayer.CurrentProgress;
                }
                /*
				else
				{
					CurrentProgress = 0;
                    IsPlaying = GlobalData.GlobalPlayer.IsPlaying;
                }
				*/
                return true;
            });
		}
    }
}
