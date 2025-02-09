using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ShowSurfer.Services;
using ShowSurfer.Pages;
using ShowSurfer.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShowSurfer.ViewModels
{
    public partial class GenreViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;
        // Uses the TmdbSerive and the GenreId
        public GenreViewModel(TmdbService tmdbService, int genreId)
        {
            _tmdbService = tmdbService;
            GenreId = genreId;
            // Calls the GetGenreName method to convert the genreId into a name
            string genreName = GetGenreName(genreId);
            CustomSearchHeading = $"Search Results for '{genreName}'";
        }
        public int GenreId { get; }
        public TmdbService TmdbService => _tmdbService;
        public string CustomSearchHeading { get; }

        // Add the Genre Search Results to an ObservableCollection to display
        public ObservableCollection<Programme> GenreSearchResults { get; set; } = new();

        public async Task InitializeAsync()
        {
            var searchList = await _tmdbService.GetGenreSearchResultsAsync(GenreId);
            Debug.WriteLine($"Link: {searchList}");
            SetProgrammeCollection(searchList, GenreSearchResults);

        }

        // Adds the search results to the collection
        private static void SetProgrammeCollection(IEnumerable<Programme> programmes, ObservableCollection<Programme> collection)
        {
            collection.Clear();
            foreach (var programme in programmes)
            {
                // If there is no poster icon, then use the "No Poster Found" image.
                if (programme.PosterIcon == "https://image.tmdb.org/t/p/w220_and_h330_face/")
                {
                    // Imgur link to edited Wikimedia Commons picture I uploaded.
                    programme.PosterIcon = "https://i.imgur.com/e2O6GqJ.png";
                }
                collection.Add(programme);
            }
        }
        // Method to return a genreName for the genreId provided
        public static string GetGenreName(int genreId)
        {
            // Return the correct genre name based on the id for the search heading
            string genreName = genreId == 28 ? "Action" :
                        genreId == 12 ? "Adventure" :
                        genreId == 16 ? "Animation" :
                        genreId == 35 ? "Comedy" :
                        genreId == 80 ? "Crime" :
                        genreId == 99 ? "Documentary" :
                        genreId == 18 ? "Drama" :
                        genreId == 10751 ? "Family" :
                        genreId == 14 ? "Fantasy" :
                        genreId == 36 ? "History" :
                        genreId == 27 ? "Horror" :
                        genreId == 10402 ? "Music" :
                        genreId == 9648 ? "Mystery" :
                        genreId == 10749 ? "Romance" :
                        genreId == 878 ? "Science Fiction" :
                        genreId == 10770 ? "TV Movie" :
                        genreId == 53 ? "Thriller" :
                        genreId == 10752 ? "War" :
                        genreId == 37 ? "Western" :
                        "NoGenre"; 

            // Return the genre name
            return genreName;
        }


        // Command and method for navigating to the ProgrammeDetailsPage
        public ICommand ShowDetailsPageCommand => new Command<Programme>(ShowDetailsPage);

        // Async method to navigate to the ShowDetailsPage
        public async void ShowDetailsPage(Programme selectedProgramme)
        {
            if (selectedProgramme != null)
            {
                var details = await _tmdbService.GetProgrammeDetailsAsync(selectedProgramme.Id);
                // Create an instance of MovieDetailsViewModel and pass the selected Programme and details
                MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel(selectedProgramme, _tmdbService);
                // Use Shell.Current to access the Shell and navigate to the MovieDetailsPage
                await Shell.Current.GoToAsync($"//movieDetailsPage?selectedProgrammeId={selectedProgramme.Id}");
            }
        }
    }
}
