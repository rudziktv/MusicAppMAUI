using Android.Media;
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
        }

        public void PlayPause()
        {
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

            CurrentTitle = title;
            CurrentAuthor = author;

            if (autoplay)
            {
                player.Start();
            }
        }
    }
}
