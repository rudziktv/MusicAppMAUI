using Android.Graphics.Drawables;
using Android.Media;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;

namespace MauiApp1.Services
{
    internal class PlayerService : BindableObject
    {
        private MediaPlayer player;

        public string CurrentTitle { get; private set; }
        public string CurrentAuthor { get; private set; }

        /// <summary>
        /// Current position on timeline in percents (0.00-1.00).
        /// </summary>
        public float CurrentProgress
        {
            get
            {
                float a = player.CurrentPosition;
                float b = player.Duration;
                return a / b;
            }
        }

        /// <summary>
        /// Duration of current file in miliseconds.
        /// </summary>
        public int CurrentDuration
        {
            get
            { 
                return player.Duration;
            }
        }

        public string CurrentPath { get; private set; }

        public bool IsPlaying
        {
            get
            { 
                return player.IsPlaying;
            }
        }


        public PlayerService()
        {
            player = new();

            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationTapped;
        }
        
        private void Current_NotificationTapped(NotificationActionEventArgs e)
        {
            switch (e.ActionId)
            {
                case 100:
                    PlayPause();
                    break;
                case 101:
                    player.Looping = !player.Looping;
                    break;
                default:
                    break;
            }
        }
        
        public void PlayPause()
        {
            /*
            var Notify = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = "Description",
                Title = "Paused/Played",
                ReturningData = "Nothing",
                NotificationId = 2137,
                Sound = "",
                Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions()
                {
                    ChannelId = "sample_notify",
                    AutoCancel = false,
                    LaunchAppWhenTapped = true,
                    Priority = Plugin.LocalNotification.AndroidOption.AndroidPriority.Min
                }
            };
            */

            //LocalNotificationCenter.Current.Show(Notify);

            if (player.IsPlaying)
            {
                player.Pause();
            }
            else
            {
                player.Start();
            }
        }

        public void SeekTo(float value)
        {
            if (player.IsPlaying)
            {
                player.SeekTo((int)Math.Round(value * player.Duration));
            }
        }

        public void ChangeSource(string path, string title, string author, string id, bool autoplay = true)
        {
            player.Stop();
            player.Reset();
            player.SetDataSource(path);
            CurrentPath = path;
            player.Prepare();

            var a = DownloadService.DownloadThumbnail(id);
            
            var request = new NotificationRequest
            {
                NotificationId = 100,
                Title = title,
                Description = author,
                CategoryType = NotificationCategoryType.Status,
                Android =
                {
                    Priority = Plugin.LocalNotification.AndroidOption.AndroidPriority.Max,
                    LaunchAppWhenTapped = true,
                    AutoCancel = false,
                    ProgressBarMax = 1000,
                    ProgressBarProgress = 100,
                    ChannelId = "sample_notify"
                }
            };

            LocalNotificationCenter.Current.Show(request);
            
            CurrentTitle = title;
            CurrentAuthor = author;

            if (autoplay)
            {
                player.Start();
            }
        }
    }
}
