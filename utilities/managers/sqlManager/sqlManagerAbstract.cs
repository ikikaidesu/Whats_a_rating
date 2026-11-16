using Microsoft.Data.Sqlite;
using movieRateGuess.utilities.entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Resources;
using System.Xml.Linq;

namespace movieRateGuess.utilities.managers.sqlManager
{
    public abstract class sqlManagerAbstract : sqlManagerInterface
    {
        private readonly string _path =  Path.Combine(
                                         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                         "movieRateGuess",
                                         "db.sqlite");
        private readonly string _connectionString;

        protected sqlManagerAbstract()
        {
            _connectionString = $"Data Source={_path};Foreign Keys=True";
            if (checkIfDIsBExist() == false)
            {
                createDB();
            }
            createDBTables();
        }

        public bool checkIfDIsBExist()
        {
            return File.Exists(_path);
        }
        public void createDB()
        {
            // создание директории для данных файла
            Directory.CreateDirectory(Path.GetDirectoryName(_path));

            // создания файла бдшки
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

            }
        }
        public void createDBTables()
        {
            string sqlMovie = @"CREATE TABLE IF NOT EXISTS movies (
                                id INTEGER PRIMARY KEY NOT NULL,
                                posterURL TEXT NOT NULL,
                                name TEXT NOT NULL,
                                englishName TEXT NOT NULL,
                                year INTEGER NOT NULL,
                                age TEXT NOT NULL,
                                length TEXT NOT NULL,
                                earnings INTEGER NOT NULL,
                                description TEXT NOT NULL,
                                rating REAL NOT NULL);";

            string sqlGenre = @"CREATE TABLE IF NOT EXISTS genres (
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                name TEXT NOT NULL UNIQUE);";

            string sqlMovieGenre = @"CREATE TABLE IF NOT EXISTS movie_genres (
                                     movieId INTEGER NOT NULL,
                                     genreId INTEGER NOT NULL,

                                     PRIMARY KEY (movieId, genreId),

                                     FOREIGN KEY (movieId) REFERENCES movies(id) ON DELETE CASCADE,
                                     FOREIGN KEY (genreId) REFERENCES genres(id) ON DELETE CASCADE);";

            string sqlDirectors = @"CREATE TABLE IF NOT EXISTS directors (
                                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    name TEXT NOT NULL UNIQUE);";

            string sqlMovieDirectors = @"CREATE TABLE IF NOT EXISTS movie_directors (
                                         movieId INTEGER NOT NULL,
                                         directorId INTEGER NOT NULL,

                                         PRIMARY KEY (movieId, directorId),

                                         FOREIGN KEY (movieId) REFERENCES movies(id) ON DELETE CASCADE,
                                         FOREIGN KEY (directorId) REFERENCES directors(id) ON DELETE CASCADE);";

            string sqlFriendRatings = @"CREATE TABLE IF NOT EXISTS friends_rating (
                                         friendId TEXT NOT NULL,
                                         movieId INTEGER NOT NULL,
                                         rating INTEGER NOT NULL,

                                         PRIMARY KEY (friendId, movieId),

                                         FOREIGN KEY (movieId) REFERENCES movies(id) ON DELETE CASCADE
                                        );";

            string[] sqlCommands =
                                {
                                    sqlMovie,
                                    sqlGenre,
                                    sqlMovieGenre,
                                    sqlDirectors,
                                    sqlMovieDirectors,
                                    sqlFriendRatings
                                };


            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                foreach (string sql in sqlCommands)
                {
                    using (SqliteCommand command = new SqliteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        public void addMovie(entity_movie movie)
        {
            string sqlMovie = $@"INSERT INTO movies (id,posterURL,name,englishName,year,age,length,earnings,description,rating)
                                VALUES 
                                (
                                @id,
                                @posterUrl,
                                @name,
                                @englishName,
                                @year,
                                @age,
                                @length,
                                @earnings,
                                @description,
                                @rating
                                )";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sqlMovie, connection))
                {
                    command.Parameters.AddWithValue("@id", movie.id);
                    command.Parameters.AddWithValue("@posterUrl", movie.posterUrl);
                    command.Parameters.AddWithValue("@name", movie.name);
                    command.Parameters.AddWithValue("@englishName", movie.englishName);
                    command.Parameters.AddWithValue("@year", movie.year);
                    command.Parameters.AddWithValue("@age", movie.age);
                    command.Parameters.AddWithValue("@length", movie.length);
                    command.Parameters.AddWithValue("@earnings", movie.earnings);
                    command.Parameters.AddWithValue("@description", movie.description);
                    command.Parameters.AddWithValue("@rating", movie.rating);

                    command.ExecuteNonQuery();
                }
            }

            foreach (string genre in movie.genres)
            {
                int genreId = -1;
                if (checkIfGenreExistByName(genre, out genreId) == false)
                {
                    entity_genre genreEntity = new entity_genre { name = genre };
                    addGenre(genreEntity);
                    // вызываем еще раз чтобы получить айди
                    checkIfGenreExistByName(genre, out genreId);
                }
                addMovieGenre(movie.id, genreId);
            }


            foreach (string director in movie.directors)
            {
                int directorId = -1;
                if (checkIfDirectorExistByName(director, out directorId) == false)
                {
                    entity_director directorEntity = new entity_director { name = director };
                    addDirector(directorEntity);
                    // вызываем еще раз чтобы получить айди
                    checkIfDirectorExistByName(director, out directorId);
                }
                addMovieDirectors(movie.id, directorId);
            }
        }

        public bool checkIfMovieExistById(int movieId)
        {
            string sql = "SELECT id FROM movies WHERE id = @id;";
            
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", movieId);
                    return command.ExecuteScalar() != null;
                }
            }    
        }

        public entity_movie getMovieFromDB(int movieId)
        {
            entity_movie result = null;

            string sql = "SELECT * FROM movies WHERE id = @id;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", movieId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new entity_movie
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                posterUrl = reader.GetString(reader.GetOrdinal("posterUrl")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                englishName = reader.GetString(reader.GetOrdinal("englishName")),
                                year = reader.GetInt32(reader.GetOrdinal("year")),
                                age = reader.GetString(reader.GetOrdinal("age")),
                                length = reader.GetString(reader.GetOrdinal("length")),
                                earnings = reader.GetInt32(reader.GetOrdinal("earnings")),
                                description = reader.GetString(reader.GetOrdinal("description")),
                                rating = reader.GetDouble(reader.GetOrdinal("rating"))
                            };
                        }
                    }
                }
            }

            return result;
        }

        public bool checkIfGenreExistByName(string name, out int genreId)
        {
            string sql = "SELECT id FROM genres WHERE name = @name;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        genreId = Convert.ToInt32(result);
                        return true;
                    }
                }
            }

            genreId = -1;
            return false;
        }

        public void addGenre(entity_genre genre)
        {
            string sqlMovie = $@"INSERT INTO genres (name)
                                VALUES 
                                (
                                @name
                                )";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sqlMovie, connection))
                {
                    command.Parameters.AddWithValue("@name", genre.name);

                    command.ExecuteNonQuery();
                }
            }
        }
        public entity_genre getGenreById(int genreId)
        {
            entity_genre result = null;

            string sql = "SELECT * FROM genres WHERE id = @id;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", genreId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new entity_genre
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                name = reader.GetString(reader.GetOrdinal("name"))
                            };
                        }
                    }
                }
            }

            return result;
        }

        public void addMovieGenre(int movieId, int genreId)
        {
            string sql = $@"INSERT INTO movie_genres (movieId, genreId)
                                VALUES 
                                (
                                @movieId,
                                @genreId
                                )";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    command.Parameters.AddWithValue("@genreId", genreId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public entity_genre[] getAllGenresByMovieId(int movieId)
        {
            List<entity_genre> result = new List<entity_genre>();

            string sql = @"SELECT genres.*
                           FROM genres
                           INNER JOIN movie_genres ON genres.id = movie_genres.genreId
                           WHERE movie_genres.movieId = @movieId;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entity_genre genre = new entity_genre()
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                name = reader.GetString(reader.GetOrdinal("name"))
                            };

                            result.Add(genre);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        public bool checkIfDirectorExistByName(string name, out int directorId)
        {
            string sql = "SELECT id FROM directors WHERE name = @name;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        directorId = Convert.ToInt32(result);
                        return true;
                    }
                }
            }

            directorId = -1;
            return false;
        }
        public void addDirector(entity_director director)
        {
            string sql = $@"INSERT INTO directors (name)
                                VALUES 
                                (
                                @name
                                )";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", director.name);

                    command.ExecuteNonQuery();
                }
            }
        }
        public entity_director getDirectorById(int directorId)
        {
            entity_director result = null;

            string sql = "SELECT * FROM directors WHERE id = @id;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", directorId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new entity_director
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                name = reader.GetString(reader.GetOrdinal("name"))
                            };
                        }
                    }
                }
            }

            return result;
        }
        public void addMovieDirectors(int movieId, int directorId)
        {
            string sql = $@"INSERT INTO movie_directors (movieId, directorId)
                                VALUES 
                                (
                                @movieId,
                                @directorId
                                )";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    command.Parameters.AddWithValue("@directorId", directorId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public entity_director[] getAllDirectorsByMovieId(int movieId)
        {
            List<entity_director> result = new List<entity_director>();

            string sql = @"SELECT directors.*
                           FROM directors
                           INNER JOIN movie_directors ON directors.id = movie_directors.directorId
                           WHERE movie_directors.movieId = @movieId;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entity_director genre = new entity_director()
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                name = reader.GetString(reader.GetOrdinal("name"))
                            };

                            result.Add(genre);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        public bool checkIfFriendRatingMovie(string friendId, int movieId, out int friendRating)
        {
            string sql = "SELECT rating FROM friends_rating WHERE friendId = @friendId and movieId = @movieId;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@friendId", friendId);
                    command.Parameters.AddWithValue("@movieId", movieId);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        friendRating = Convert.ToInt32(result);
                        return true;
                    }
                }
            }

            friendRating = 0;
            return false;
        }

        public void addFriendRating(string friendId, int movieId, int rating)
        {
            string sql = $@"INSERT INTO friends_rating (friendId, movieId, rating)
                                VALUES 
                                (
                                @friendId,
                                @movieId,
                                @rating
                                )";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@friendId", friendId);
                    command.Parameters.AddWithValue("@movieId", movieId);
                    command.Parameters.AddWithValue("@rating", rating);

                    command.ExecuteNonQuery();
                }
            }
        }

        public entity_friendRating getFriendRating(string friendId, int movieId)
        {
            entity_friendRating result = null;

            string sql = @"SELECT * FROM friends_rating WHERE friendId = @id and 
                                                              movieId = @movieId;";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@friendId", friendId);
                    command.Parameters.AddWithValue("@movieId", movieId);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new entity_friendRating
                            {
                                friendId = reader.GetString(reader.GetOrdinal("friendId")),
                                movieId = reader.GetInt32(reader.GetOrdinal("movieId")),
                                rating = reader.GetInt32(reader.GetOrdinal("rating")),
                            };
                        }
                    }
                }
            }

            return result;
        }
    }
}
