using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities.entities
{
    public class entity_ratingButton : viewModelMain
    {
        private int _number;
        public int number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged(nameof(number));
            }
        }
        private string _color;
        public string color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(color));
            }
        }
        private string _defaultColor;
        public string defaultColor
        {
            get { return _defaultColor; }
            set
            {
                _defaultColor = value;
                OnPropertyChanged(nameof(defaultColor));
            }
        }
        private bool _canClick;
        public bool canClick
        {
            get { return _canClick; }
            set
            {
                _canClick = value;
                OnPropertyChanged(nameof(canClick));
            }
        }
    }
}
