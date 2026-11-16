using movieRateGuess.utilities.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities.managers.sqlManager
{
    public interface sqlManagerInterface
    {
        bool checkIfDIsBExist();
        void createDB();
        void createDBTables();
        void addMovie(entity_movie movie);
        bool checkIfMovieExistById(int movieId);
        entity_movie getMovieFromDB(int movieId);
        bool checkIfGenreExistByName(string name, out int genreId);
        void addGenre(entity_genre genre);
        entity_genre getGenreById(int genreId);
        void addMovieGenre(int movieId, int genreId);
        entity_genre[] getAllGenresByMovieId(int movieId);
        bool checkIfDirectorExistByName(string name, out int directorId);
        void addDirector(entity_director director);
        entity_director getDirectorById(int directorId);
        void addMovieDirectors(int movieId, int directorId);
        entity_director[] getAllDirectorsByMovieId(int movieId);
        bool checkIfFriendRatingMovie(string friendId, int movieId, out int rating);
        void addFriendRating(string friendId, int movieId, int rating);
        entity_friendRating getFriendRating(string friendId, int movieId);
    }
}
