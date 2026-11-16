using movieRateGuess.utilities.entities;
using movieRateGuess.utilities.managers.apiManager;
using movieRateGuess.utilities.managers.sqlManager;
using movieRateGuess.utilities.managers.txtManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace movieRateGuess.models
{
    public class m_game
    {
        private txtManagerInterface _txtManager;
        private apiManagerInterface _apiManager;
        private sqlManagerInterface _sqlManager;
        private string _friendId;
        private string[] colorsArray;
        private List<entity_ratingButton> ratingButtons;
        private int amountOfRatingButtons;
        private string wrongRatingColor;


        private List<entity_movie> moviesToGuess;

        public m_game(string friendId)
        {
            initializeData(friendId);

        }

        private void initializeData(string friendId)
        {
            _txtManager = new txtManager();
            _sqlManager = new sqlManager();
            _apiManager = new apiManager(_sqlManager);


            _friendId = friendId;
            string apiKey = _txtManager.getFileContent();
            _apiManager.getApiKey(apiKey);

            if (string.IsNullOrEmpty(_friendId) == true)
            {
                moviesToGuess = _apiManager.getPopularMovies();
            }
            else
            {
                moviesToGuess = _apiManager.getFriendMoviesById(_friendId);
            }

            shuffle(moviesToGuess);

            amountOfRatingButtons = 10;
            colorsArray = new string[]
                {
                "#FF0000",
                "#FF2600",
                "#FF6A00",
                "#FFA600",
                "#FFE100",
                "#D9FF00",
                "#9DFF00",
                "#95FF00",
                "#33FF00",
                "#00FF55"
                };

            ratingButtons = new List<entity_ratingButton>();
            for (int i = 0; i < amountOfRatingButtons; i++)
            {
                entity_ratingButton ratingButton = new entity_ratingButton()
                {
                    number = i + 1,
                    color = colorsArray[i],
                    defaultColor = colorsArray[i],
                    canClick = true
                };
                ratingButtons.Add(ratingButton);

            }

            wrongRatingColor = "#B8B8B8";

        }

        public List<entity_movie> getMoviesToGuess()
        {
            return moviesToGuess;
        }

        public List<entity_ratingButton> getRatingButtons()
        {
            return ratingButtons;
        }

        public string getWrongRatingColor()
        {
            return wrongRatingColor;
        }

        // данный метод использует алгоритм Фишера — Йетса для перемешивания списка
        private void shuffle<T>(List<T> list)
        {
            int n = list.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
