using Android.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    internal class PlayerService
    {
        public string CurrentTitle { get; set; }
        public string CurrentAuthor { get; set; }

        /// <summary>
        /// Current position on timeline in percents (0.00-1.00).
        /// </summary>
        public float CurrentProgress { get; set; }

        /// <summary>
        /// Duration of current file in miliseconds.
        /// </summary>
        public int CurrentDuration { get; set; }
        public string CurrentPath { get; private set; }
    }
}
