using ShowSurfer.Controls;
using ShowSurfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace ShowSurfer.Services
{
    public class TmdbService
    {
        // My API Key from TMDB
        private const string ApiKey = "Enter your API Key Here";
        //Specify the HttpClientName
        public const string TmdbHttpClientName = "TmdbClient";
        // Import the HttpClientFactory
        private readonly IHttpClientFactory _httpClientFactory;

        public TmdbService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient HttpClient => _httpClientFactory.CreateClient(TmdbHttpClientName);

        // Passes the API call specified below into the GetProgrammeAsync method (Pulling Top Rated Movies)
        public async Task<IEnumerable<Programme>> GetTopRatedAsync() => await GetProgrammeAsync(TmdbUrlCalls.TopRated);
        // Passes the API call specified below into the GetProgrammeAsync method (Search Querying)
        public async Task<IEnumerable<Programme>> GetSearchResultsAsync(string query) => await GetProgrammeAsync(TmdbUrlCalls.GetSearchResults(query));
        // Passes the API call specified below into the GetProgrammeAsync method (Genre Querying)
        public async Task<IEnumerable<Programme>> GetGenreSearchResultsAsync(int genreId) => await GetProgrammeAsync(TmdbUrlCalls.GetGenreSearchResults(genreId));
        // Method to retrieve a list of programs based on the above method.
        public async Task<IEnumerable<Programme>> GetProgrammeAsync(string url)
        {
            var trendingMovies = await HttpClient.GetFromJsonAsync<Movie>($"{url}&api_key={ApiKey}");
            // Converts result to a Programme Object
            return trendingMovies.results.Select(r => r.ToProgrammeObject());
        }
       
        // Awaits the ProgrammeDetailsAsync method
        public async Task<MovieInformation> GetProgrammeDetailsAsync(int id) =>
            await HttpClient.GetFromJsonAsync<MovieInformation>($"{TmdbUrlCalls.GetProgrammeInfo(id)}&api_key={ApiKey}");

        // Calls the method to get the Watch Providers
        public async Task<WatchProviders> GetWatchProvidersAsync(int id) =>
         await HttpClient.GetFromJsonAsync<WatchProviders>($"{TmdbUrlCalls.GetWatchProviders(id)}&api_key={ApiKey}");

         // Calls the method to get the Cast List
        public async Task<CastResults> GetCastResultsAsync(int id) =>
         await HttpClient.GetFromJsonAsync<CastResults>($"{TmdbUrlCalls.GetCastResults(id)}&api_key={ApiKey}"); 

         // Calls the method to get the Backdrop List
        public async Task<BackdropResult> GetMovieImageResultsAsync(int id) =>
         await HttpClient.GetFromJsonAsync<BackdropResult>($"{TmdbUrlCalls.GetMovieImageResults(id)}&api_key={ApiKey}"); 
         

    }

    // List of API url calls that I make for this application
    public static class TmdbUrlCalls
    {
        // Currently set to TopRated but might swap to Trending at some point
        public const string Trending = "3/trending/all/week?language=en-US";
        public const string TopRated = "3/movie/top_rated?language=en-US";

        // Defines the URLS where you need to pass a value into it
        public static string GetMovieImageResults(int movieId) => $"3/movie/{movieId}/images?";
        public static string GetCastResults(int movieId) => $"3/movie/{movieId}/credits?language=en-US";
        public static string GetSearchResults(string searchQuery) => $"3/search/movie?query={searchQuery}&include_adult=false&language=en-US&page=1";
        public static string GetWatchProviders(int movieId) => $"3/movie/{movieId}/watch/providers?language=en-US";
        public static string GetGenreSearchResults(int genreId) => $"/3/discover/movie?include_adult=false&include_video=false&language=en-US&page=1&sort_by=popularity.desc&with_genres={genreId}";
        public static string GetProgrammeInfo(int movieId) => $"3/movie/{movieId}?language=en-US";
    }

    // Reads the entire JSON return for the movie Query
    public class Movie
    {
        public int page { get; set; }
        public Result[] results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }

    // Formats the results of a movie query.
    public class Result
    {
        public string backdrop_path { get; set; }
        public int[] genre_ids { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public bool video { get; set; }
        // "Movie" or "TV Show"
        public string programme_type { get; set; }
        // If poster_path is Null use the backdrop_path
        public string PosterPath => poster_path ?? backdrop_path;
        public string Poster => $"https://image.tmdb.org/t/p/w600_and_h900_bestv2/{PosterPath}";
        public string PosterIcon => $"https://image.tmdb.org/t/p/w220_and_h330_face/{PosterPath}";
        public string PosterUrl => $"https://image.tmdb.org/t/p/original/{PosterPath}";
        // Simplified if else statement. Try get title first, then name, then original_title and finally, if all other options are null, get the original_name.
        public string DisplayTitle => title ?? name ?? original_title ?? original_name;


        // Turns it into a Programme Object
        public Programme ToProgrammeObject() => new()
        {
            Id = id,
            DisplayTitle = DisplayTitle,
            ProgrammeType = programme_type,
            Poster = Poster,
            PosterIcon = PosterIcon,
            PosterUrl = PosterUrl,
            Overview = overview,
            ReleaseDate = release_date,
            GenreIds = genre_ids
        };
    }

    // More Detailed Movie Information
    public class MovieInformation
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public object belongs_to_collection { get; set; }
        public int budget { get; set; }
        public Genre[] genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public Production_Companies[] production_companies { get; set; }
        public string release_date { get; set; }
        public int revenue { get; set; }
        public int runtime { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
    }

    // Reads the entire JSON return for the Watch Providers endpoint
    public class WatchProviders
    {
        public int id { get; set; }
        public Dictionary<string, CountryCode> results { get; set; }
    }

    // Reads the CountryCode for each Result
    public class CountryCode
    {
        public string link { get; set; }
        public List<FlatRate> flatrate { get; set; }
        public List<FlatRate> buy { get; set; }
        public List<FlatRate> rent { get; set; }
    }

    // Obtains the specific country watch providers
    public class FlatRate
    {
        public string logo_path { get; set; }
        public int provider_id { get; set; }
        public string provider_name { get; set; }
        public int display_priority { get; set; }
        public string country_code { get; set; }


        public string logo_icon => $"https://image.tmdb.org/t/p/w500/{logo_path}";
        // Turns it into a Programme Object
        public WatchProvider ToWatchProviderObject() => new()
        {
            LogoPath = logo_icon,
            ProviderId = provider_id,
            ProviderName = provider_name,
            DisplayPriority = display_priority,
            ProviderCountryCode = country_code
        };
    }

    // Production Company class
    public class Production_Companies
    {
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }


    // Cast/Crew Related Classes
    public class CastResults
    {
        public int id { get; set; }
        public List<Cast> cast { get; set; }
        public List<Crew> crew { get; set; }
    }

    public class Cast
    {
        public int id { get; set; }
        public string known_for_department { get; set; }
        public string name { set; get; }
        public string profile_path { set; get; }
        public string character { set; get; }
        public string profile_icon { set; get; }
        public string cast_character { set; get; }
    }
    public class Crew
    {
        public int id { get; set; }
        public string known_for_department { get; set; }
        public string name { set; get; }
        public string profile_path { set; get; }
        public string job { set; get; }
        public string profile_icon { set; get; }
        public string crew_job { set; get; }
    }

    // Backdrop related classes
    public class BackdropResult
    {
        public List<Backdrops> backdrops { get; set; }
    }
    public class Backdrops
    {
        public string file_path { get; set; }
        public string file_icon => $"https://image.tmdb.org/t/p/original/{file_path}";
    }

    // Genre class
    public record struct Genre(int Id, string Name);
}