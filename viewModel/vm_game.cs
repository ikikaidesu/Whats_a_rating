using movieRateGuess.models;
using movieRateGuess.utilities;
using movieRateGuess.utilities.entities;
using movieRateGuess.utilities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace movieRateGuess.viewModel
{
    internal class vm_game : viewModelMain
    {
        private m_game model;
        private vm_navigation _currentView;
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
        private enum_gameModes _currentGameMode;
        public enum_gameModes currentGameMode
        {
            get { return _currentGameMode; }
            set
            {
                _currentGameMode = value;
                OnPropertyChanged(nameof(currentGameMode));
            }
        }
        private List<entity_movie> _movies;
        public List<entity_movie> movies
        {
            get { return _movies; }
            set
            {
                _movies = value;
                OnPropertyChanged(nameof(movies));
            }
        }

        private List<int> _guessedMovies;
        public List<int> guessedMovies
        {
            get { return _guessedMovies; }
            set
            {
                _guessedMovies = value;
                OnPropertyChanged(nameof(guessedMovies));
            }
        }

        private int _guessedMovieCount;
        public int guessedMovieCount
        {
            get { return _guessedMovieCount; }
            set
            {
                _guessedMovieCount = value;
                OnPropertyChanged(nameof(guessedMovieCount));
            }
        }

        private entity_movie _currentMovie;
        public entity_movie currentMovie
        {
            get { return _currentMovie; }
            set
            {
                _currentMovie = value;
                OnPropertyChanged(nameof(currentMovie));
            }
        }

        private enum_gameState _gameState;
        public enum_gameState gameState
        { 
            get { return _gameState; }
            set
            {
                _gameState = value;
                OnPropertyChanged(nameof(gameState));
            }
        }

        private List<entity_ratingButton> _ratingButtons;
        public List<entity_ratingButton> ratingButtons
        {
            get { return _ratingButtons; }
            set
            {
                _ratingButtons = value;
                OnPropertyChanged(nameof(ratingButtons));
            }
        }

        private string _wrongColorRating;
        public string wrongColorRating
        {
            get { return _wrongColorRating; }
            set
            {
                _wrongColorRating = value;
                OnPropertyChanged(nameof(wrongColorRating));
            }
        }

        private int _maxAttempts;
        public int maxAttempts
        { 
            get { return _maxAttempts; }
            set
            {
                _maxAttempts = value;
                OnPropertyChanged(nameof(maxAttempts));
            }
        }

        private int _currentAttemptsLeft;
        public int currentAttemptsLeft
        {
            get { return _currentAttemptsLeft; }
            set
            {
                _currentAttemptsLeft = value;
                OnPropertyChanged(nameof(currentAttemptsLeft));
            }
        }

        private DateTime _startTime;

        private bool _isLoading;
        public bool isLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(isLoading));
            }
        }


        public ICommand goBackToMenuCommand { get; set; }
        public ICommand clickOnRatingCommand { get; set; }
        public ICommand goToNextMovieCommand { get; set; }

        public vm_game(vm_navigation _currentView, string friendId)
        {
            this._currentView = _currentView;
            this.friendId = friendId;
            defineGameMode();
            initializeDataAsync(currentGameMode);

            goBackToMenuCommand = new RelayCommand(goBackToMenu);
            clickOnRatingCommand = new RelayCommand(clickOnRating);
            goToNextMovieCommand = new RelayCommand(goToNextMovie);
        }

        private void defineGameMode()
        {
            if (string.IsNullOrEmpty(friendId))
            {
                currentGameMode = enum_gameModes.random;
            }
            else
            {
                currentGameMode = enum_gameModes.friend;
            }
        }


        private async void initializeDataAsync(enum_gameModes gameMode)
        {
            isLoading = true;

            model = await Task.Run(() => new m_game(friendId));
            movies = await Task.Run(() => model.getMoviesToGuess());
            ratingButtons = await Task.Run(() => model.getRatingButtons());

            gameState = enum_gameState.playing;
            currentMovie = movies.First();
            guessedMovies = new List<int>();
            guessedMovieCount = 0;
            wrongColorRating = model.getWrongRatingColor();
            maxAttempts = 3;
            currentAttemptsLeft = maxAttempts;
            _startTime = DateTime.Now;

            isLoading = false;
        }

        private void goBackToMenu(object obj)
        {
            _currentView.navigateTo(enum_pages.menu);
        }

        private void clickOnRating(object obj)
        {
            if (obj.GetType() == typeof(int))
            {
                int guess = Convert.ToInt16(obj);
                currentAttemptsLeft--;
                checkGuess(guess);

            }
        }

        private void checkGuess(int guess)
        {
            int rating = Convert.ToInt16(Math.Round(currentMovie.rating, 0, MidpointRounding.AwayFromZero));
            //int rating = Convert.ToInt16(Math.Round(8.5, 0, MidpointRounding.AwayFromZero));

            // выйграл
            if (guess == rating)
            {
                win(guess);
                return;
            }
            else
            {
                // проиграл
                if (currentAttemptsLeft == 0)
                {
                    defeat();
                    return;
                }
                // выбрал меньше
                if (guess < rating)
                {
                    for (int i = 0; i < guess; i++)
                    {
                        entity_ratingButton currentButton = ratingButtons[i];
                        if (currentButton.canClick == true)
                        {
                            turnOffRatingButton(currentButton);
                        }
                    }
                }
                // выбрал больше
                else if (guess > rating)
                {
                    for (int i = ratingButtons.Count; i >= guess; i--)
                    {
                        entity_ratingButton currentButton = ratingButtons[i - 1];
                        if (currentButton.canClick == true)
                        {
                            turnOffRatingButton(currentButton);
                        }
                    }
                }
            }
        }

        private void turnOffRatingButton(entity_ratingButton currentButton)
        {
            currentButton.color = wrongColorRating;
            currentButton.canClick = false;
        }

        private void win(int guess)
        {
            for (int i = 0; i < ratingButtons.Count; i++)
            {
                entity_ratingButton currentButton = ratingButtons[i];
                if (guess - 1 != i && currentButton.canClick == true)
                {
                    turnOffRatingButton(currentButton);
                }
            }
            gameState = enum_gameState.win;
            guessedMovieCount++;
        }

        private void defeat()
        {
            for (int i = 0; i < ratingButtons.Count; i++)
            {
                entity_ratingButton currentButton = ratingButtons[i];
                turnOffRatingButton(currentButton);
            }
            gameState = enum_gameState.defeat;
        }


        private void goToNextMovie(object obj)
        {
            guessedMovies.Add(currentMovie.id);
            // если кол-во угаданных фильмов приближается к размеру фильмов в целом
            // (тоесть человек почти угадал все фильмы)
            // то просто обнуляем список дабы режим был бесконечным
            if (guessedMovies.Count == movies.Count)
            {
                int filmAmount = movies.Count;
                TimeSpan time = DateTime.Now - _startTime;
                _currentView.navigateTo(enum_pages.result, filmAmount: filmAmount, score: guessedMovieCount, time: time);
                return;
            }

            Random rnd = new Random();
            do
            {
                int i = rnd.Next(0, movies.Count);
                currentMovie = movies[i];
            }
            while (guessedMovies.Contains(currentMovie.id) == true);

            gameState = enum_gameState.playing;

            for (int i = 0; i < ratingButtons.Count; i++)
            {
                entity_ratingButton btn = ratingButtons[i];
                btn.color = btn.defaultColor;
                btn.canClick = true;
            }

            currentAttemptsLeft = maxAttempts;
        }
    }
}
