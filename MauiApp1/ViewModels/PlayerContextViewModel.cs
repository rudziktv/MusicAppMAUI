using MauiApp1.LocalDatabase;
using MauiApp1.Model;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    internal class PlayerContextViewModel
    {
        public ObservableCollection<Track> PlayingQueue { get; set; }
        public Command CloseCommand { get; set; }

        public PlayerContextViewModel()
        {
            CloseCommand = new(() => GlobalData.PlayerContext.Close());

            PlayingQueue = new();

            for (int i = 0; i < 10; i++)
            {
                PlayingQueue.Add(new()
                {
                    title = $"{i}. Track Title"
                });
            }
        }

        
    }
}
