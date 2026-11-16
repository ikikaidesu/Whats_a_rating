using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities.entities
{
    public class entity_movie : viewModelMain
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }
        private string _posterUrl;
        public string posterUrl
        {
            get { return _posterUrl; }
            set
            {
                _posterUrl = value;
                OnPropertyChanged(nameof(posterUrl));
            }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(name));
            }
        }
        private string _englishName;
        public string englishName
        {
            get { return _englishName; }
            set
            {
                _englishName = value;
                OnPropertyChanged(nameof(englishName));
            }
        }
        private int _year;
        public int year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged(nameof(year));
            }
        }

        private string _age;
        public string age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged(nameof(age));
            }
        }

        private string _length;
        public string length
        {
            get { return _length; }
            set
            {
                _length = value;
                OnPropertyChanged(nameof(length));
            }
        }

        private string[] _genres;
        public string[] genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged(nameof(genres));
            }
        }

        private string[] _directors;
        public string[] directors
        {
            get { return _directors; }
            set
            {
                _directors = value;
                OnPropertyChanged(nameof(directors));
            }
        }

        private int _earnings;
        public int earnings
        {
            get { return _earnings; }
            set
            {
                _earnings = value;
                OnPropertyChanged(nameof(earnings));
            }
        }
        private string _description;
        public string description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(description));
            }
        }

        private double _rating;
        public double rating
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
