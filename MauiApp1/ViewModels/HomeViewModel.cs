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
        public ICommand OpenPlayerCommand { get; set; }

        public HomeViewModel()
        {

        }
    }
}
