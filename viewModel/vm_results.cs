using movieRateGuess.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace movieRateGuess.viewModel
{
    public class vm_results : viewModelMain
    {
        private vm_navigation navigation;
        private int _filmAmount;
        public int filmAmount
        {
            get { return _filmAmount; }
            set
            {
                _filmAmount = value;
                OnPropertyChanged(nameof(filmAmount));
            }
        }

        private int _score;
        public int score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged(nameof(score));
            }
        }
        private TimeSpan? _time;
        public TimeSpan? time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(time));
            }
        }

        public ICommand goBackCommand { get; set; }

        public vm_results(vm_navigation navigation, int filmAmount, int score, TimeSpan? time)
        {
            initializeData(navigation, filmAmount, score, time);
        }

        private void initializeData(vm_navigation navigation, int filmAmount, int score, TimeSpan? time)
        {
            this.navigation = navigation;
            this.filmAmount = filmAmount;
            this.score = score;
            this.time = time;

            time = TimeSpan.FromSeconds(Math.Round(time.Value.TotalSeconds));

            goBackCommand = new RelayCommand(goBack);
        }

        private void goBack(object obj)
        {
            navigation.navigateTo(utilities.enums.enum_pages.menu);
        }
    }
}
