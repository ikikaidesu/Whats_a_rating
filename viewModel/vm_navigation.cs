using movieRateGuess.utilities;
using movieRateGuess.utilities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.viewModel
{
    public class vm_navigation : viewModelMain
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public vm_navigation()
        {
            CurrentView = new vm_menu(this);
        }

        public void navigateTo(enum_pages page, string friendId = "", int filmAmount=0,int score=0, TimeSpan? time = null)
        {
            switch (page)
            {
                case enum_pages.menu:
                    CurrentView = new vm_menu(this);
                    break;
                case enum_pages.game:
                    CurrentView = new vm_game(this, friendId);
                    break;
                case enum_pages.result:
                    CurrentView = new vm_results(this, filmAmount, score, time);
                    break;
                default:
                    throw new ArgumentException("Invalid page enum value");
            }
        }
    }
}
