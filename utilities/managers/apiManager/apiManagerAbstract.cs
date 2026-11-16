using movieRateGuess.utilities.entities;
using movieRateGuess.utilities.managers.sqlManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Markup;



namespace movieRateGuess.utilities.managers.apiManager
{

    // одна из проблем с которой я столкнулся это некачественные данные от апи
    // переодически мне приходит вроде бы сущность фильма, а некоторых свойств там нет
    // или свойства есть, а данных в ней нет
    // из-за подобной вещи в коде переодически будут встречаться на первый взгляд странные и непонятные проверки
    // но они нужны. Правда нужны, ведь помогают отбрасывать некачественные данные
    public abstract class apiManagerAbstract : apiManagerInterface
    {
        protected string _apiKey;
        public void getApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        protected sqlManagerInterface _sqlManager;

        public apiManagerAbstract(sqlManagerInterface sqlManager)
        {
            _sqlManager = sqlManager;
        }


        private string doRequestToApi(string url)
        {
            string data = "";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);

                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    data = response.Content.ReadAsStringAsync().Result;
                }
            }

            return data;
        }

        private string getMovieJsonDataById(int movieId)
        {
            string result = "";

            string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/{movieId}";

            result = doRequestToApi(url);

            return result;
        }

        public entity_movie getMovieById(int movieId)
        {
            entity_movie result = new entity_movie();

            result = getFullMovieData(movieId);

            return result;
        }
        public int getMovieEarningsById(int movieId)
        {
            int result = 0;
            
            string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/{movieId}/box_office";

            string jsonData = doRequestToApi(url);
            JsonNode json = JsonNode.Parse(jsonData);
            JsonArray JsonWorldData = (JsonArray)json[1];
            if (JsonWorldData.Count > 1)
            {
                JsonObject JsonWorldDataEarning = (JsonObject)JsonWorldData[1];
                result = JsonWorldDataEarning["amount"].GetValue<int>();
            }

            return result;
        }
        public string[] getDirectorsById(int movieId)
        {
            List<string> result = new List<string>();

            string url = $"https://kinopoiskapiunofficial.tech/api/v1/staff?filmId={movieId}";
            string jsonData = doRequestToApi(url);
            JsonNode json = JsonNode.Parse(jsonData);

            foreach (JsonObject jsonObject in (JsonArray)json)
            {
                string profession = jsonObject["professionKey"].GetValue<string>();
                if (profession == "DIRECTOR")
                {
                    string director = jsonObject["nameEn"].GetValue<string>();
                    result.Add(director);
                }
                else
                {
                    break;
                }
            }

            return result.ToArray();
        }
        public List<entity_movie> getPopularMovies()
        {
            List<entity_movie> result = new List<entity_movie>();

            List<string> urls = new List<string>();
            List<JsonNode> jsonDatas = new List<JsonNode>();

            // вариант чтобы взять 1 страницу фильмов
            int i = 1;
            string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/collections?type=TOP_250_MOVIES&page={i}";
            string jsonData = doRequestToApi(url);
            JsonNode json = JsonNode.Parse(jsonData);
            JsonArray jsonFilms = (JsonArray)json["items"];

            // вариант чтобы взять 5 фильмов
            //JsonArray jsonFilms = new JsonArray(
            //                                        json["items"].AsArray()
            //                                            .Take(5)
            //                                            .Select(item => item?.DeepClone())
            //                                            .ToArray()
            //                                    );
            jsonDatas.Add(jsonFilms);

            // вариант взять все фильмы
            //int i = 1;
            //int maxPages = 1;
            //bool isTillEnd = maxPages == 0;
            //while (isTillEnd || i <= maxPages)
            //{
            //    string url = $"https://kinopoiskapiunofficial.tech/api/v2.2/films/collections?type=TOP_250_MOVIES&page={i}";
            //    JsonArray jsonFilms = getMoviesFromPage(url);

            //    if (jsonFilms.Count == 0)
            //    {
            //        break;
            //    }

            //    jsonDatas.Add(jsonFilms);
            //    i++;
            //}

            foreach (JsonArray jsonArray in jsonDatas)
            {
                foreach (JsonObject jsonObject in jsonArray)
                {
                    int movieId = jsonObject["kinopoiskId"]?.GetValue<int>() ?? 0;
                    if (movieId == 0) continue;


                    entity_movie movie = new entity_movie() { id = movieId };

                    if (_sqlManager.checkIfMovieExistById(movieId) == false)
                    {
                        movie = getFullMovieData(movieId);
                        if (checkIfMovieDataValid(movie))
                        {
                            _sqlManager.addMovie(movie);
                        }
                        else continue;
                    }
                    else
                    {
                        movie = _sqlManager.getMovieFromDB(movieId);
                        movie.genres = _sqlManager.getAllGenresByMovieId(movieId).Select(ch => ch.name).ToArray();
                        movie.directors = _sqlManager.getAllDirectorsByMovieId(movieId).Select(ch => ch.name).ToArray();
                    }
                    
                    result.Add(movie);
                }
            }

            return result;
        }

        private bool checkIfMovieDataValid(entity_movie movie)
        {
            return movie.id > 0 &&
                   string.IsNullOrWhiteSpace(movie.posterUrl) == false &&
                   string.IsNullOrWhiteSpace(movie.name) == false &&
                   string.IsNullOrWhiteSpace(movie.englishName) == false &&
                   movie.year > 0 &&
                   movie.genres != null && 
                   movie.genres.Length > 0 &&
                   movie.directors != null && 
                   movie.directors.Length > 0 &&
                   string.IsNullOrWhiteSpace(movie.description) == false &&
                   movie.rating > 0 ;
        }

        private entity_movie getFullMovieData(int movieId)
        {
            string jsonData = getMovieJsonDataById(movieId);
            entity_movie result = transformJsonDataFromRequest(jsonData);
            result.directors = getDirectorsById(movieId);
            result.earnings = getMovieEarningsById(movieId);

            return result;
        }
        public List<entity_movie> getFriendMoviesById(string friendId)
        {
            List<entity_movie> result = new List<entity_movie>();
            List<JsonNode> jsonDatas = new List<JsonNode>();

            // у апи какой-то прикол с тем, что видимо если пользователь в нее забивается впервые и данных на него нет
            // то она забивает на ответ и молча заполняет данные
            // из-за чего чтобы удостовериться что данные мб и есть нужно второй раз вызвать
            bool firstTimeFriend = true;

            // i - просто счетчик
            int i = 1;
            // maxPages - параметр если вам не нужны все страницы, а только первых несколько
            // параметр работает в тандеме с isTillEnd, iTillEnd отвечает за то, что цикл будет работать
            // то тех пор пока не загрузит все страницы
            // если он видит, что ограничение в виде maxPages стоит, то сам отрубается и цикл идет до итоговой страницы
            // вместо всех
            int maxPages = 1;
            bool isTillEnd = maxPages == 0;
            while (isTillEnd || i <= maxPages)
            {
                string url = $"https://kinopoiskapiunofficial.tech/api/v1/kp_users/{friendId}/votes?page={i}";
                JsonArray jsonFilms = getMoviesFromPage(url);

                if (jsonFilms.Count == 0 && firstTimeFriend == true)
                {
                    jsonFilms = getMoviesFromPage(url);
                    firstTimeFriend = false;
                }

                if (jsonFilms.Count == 0)
                {
                    break;
                }

                jsonDatas.Add(jsonFilms);
                i++;
            }

            foreach (JsonArray jsonArray in jsonDatas)
            {
                foreach (JsonObject jsonObject in jsonArray)
                {
                    int movieId = jsonObject["kinopoiskId"]?.GetValue<int>() ?? 0;
                    if (movieId == 0) continue;


                    entity_movie movie = new entity_movie() { id = movieId };

                    if (_sqlManager.checkIfMovieExistById(movieId) == false)
                    {
                        movie = getFullMovieData(movieId);
                        if (checkIfMovieDataValid(movie))
                        {
                            _sqlManager.addMovie(movie);
                        }
                        else continue;
                    }
                    else
                    {
                        movie = _sqlManager.getMovieFromDB(movieId);
                        movie.genres = _sqlManager.getAllGenresByMovieId(movieId).Select(ch => ch.name).ToArray();
                        movie.directors = _sqlManager.getAllDirectorsByMovieId(movieId).Select(ch => ch.name).ToArray();
                    }

                    // меняем рейтинг из общего в рейтинг друга
                    movie.rating = jsonObject["userRating"].GetValue<int>();

                    result.Add(movie);
                }
            }

            return result;
        }

        private JsonArray getMoviesFromPage(string url)
        {
            string jsonData = doRequestToApi(url);
            JsonNode json = JsonNode.Parse(jsonData);
            JsonArray jsonFilms = (JsonArray)json["items"];

            return jsonFilms;
        }

        public entity_movie transformJsonDataFromRequest(string data)
        {
            entity_movie result = new entity_movie();

            JsonNode json = JsonNode.Parse(data);

            result.id = json["kinopoiskId"]?.GetValue<int>() ?? 0;
            result.posterUrl = json["posterUrl"]?.GetValue<string>() ?? "";
            result.name = json["nameRu"]?.GetValue<string>() ?? "";
            result.englishName = json["nameOriginal"]?.GetValue<string>() ?? "";
            result.year = json["year"]?.GetValue<int>() ?? 0;
            result.age = transformAgeData(json["ratingAgeLimits"]?.GetValue<string>() ?? "");
            result.length = transformLengthData((json["filmLength"]?.GetValue<int>() ?? 0).ToString()) ?? "-";
            result.genres = transformGenresData(json["genres"]);
            result.description = json["description"]?.GetValue<string>() ?? "";
            result.rating = Math.Round(json["ratingKinopoisk"]?.GetValue<double>() ?? 0, MidpointRounding.AwayFromZero);

            return result;
        }

        private string transformAgeData(string jsonAge)
        {
            string result = "-";

            int startAgeTextLength = 3;

            if (jsonAge.Length > startAgeTextLength)
            {
                result = jsonAge.Substring(startAgeTextLength, jsonAge.Length - startAgeTextLength) + "+";
            }

            return result;
        }

        private string transformLengthData(string length)
        {
            int.TryParse(length, out int lengthInt);
            string result = "";

            int hoursAmount = lengthInt / 60;
            if (hoursAmount > 0)
            {
                result = hoursAmount + "ч.";
            }

            int minsAmount = lengthInt % 60;
            result += " ";
            result += minsAmount + "мин.";

            return result;
        }

        private string[] transformGenresData(JsonNode jsonData)
        {
            List<string> result = new List<string>();

            JsonArray jsonGenres = (JsonArray)jsonData;
            foreach (JsonNode jsonNode in jsonGenres)
            {
                result.Add(jsonNode["genre"].GetValue<string>());
            }

            return result.ToArray();
        }



        public bool checkIfApiIsAvailable()
        {
            bool result = false;

            return result;
        }
        public bool checkIfReturnedDataCorrect(object data)
        {
            bool result = false;

            return result;
        }
    }
}
