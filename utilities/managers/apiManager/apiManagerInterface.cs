using movieRateGuess.utilities.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities.managers.apiManager
{
    public interface apiManagerInterface
    {
        void getApiKey(string apiKey);
        entity_movie getMovieById(int movieId);
        int getMovieEarningsById(int movieId);
        string[] getDirectorsById(int movieId);
        List<entity_movie> getPopularMovies();
        List<entity_movie> getFriendMoviesById(string friendId);
        bool checkIfApiIsAvailable();
        bool checkIfReturnedDataCorrect(object data);
    }
}
