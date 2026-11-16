using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities.entities
{
    public class entity_friendRating : viewModelMain
    {
        private string _friendId;
        public string friendId
        {
            get { return _friendId; }
            set
            {
                _friendId = value;
                OnPropertyChanged(nameof(friendId));
            }
        }

        private int _movieId;
        public int movieId
        {
            get { return _movieId; }
            set
            {
                _movieId = value;
                OnPropertyChanged(nameof(movieId));
            }
        }

        private int _rating;
        public int rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged(nameof(rating));
            }
        }
    }
}
