using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities
{
    public class viewModelMain : INotifyPropertyChanged
    {
        // ивент обновления
        public event PropertyChangedEventHandler PropertyChanged;
        // метод обновления
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
