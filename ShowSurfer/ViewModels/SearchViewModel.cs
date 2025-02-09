using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShowSurfer.Models;
using ShowSurfer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace ShowSurfer.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;
        public SearchViewModel(TmdbService tmdbService, string query)
        {
            _tmdbService = tmdbService;
            Query = query;
            CustomSearchHeading = $"Search Results for '{query}'";
        }
        public string Query { get; }
        public TmdbService TmdbService => _tmdbService;
        public string CustomSearchHeading { get; }

        // Observable Collection for the SearchResults
        public ObservableCollection<Programme> SearchResults { get; set; } = new();

        public async Task InitializeAsync()
        {
            var searchList = await _tmdbService.GetSearchResultsAsync(Query);
            Debug.WriteLine($"Link: {searchList}");
            SetProgrammeCollection(searchList, SearchResults);
        }

        // Add the Search Results to an ObservableCollection to display
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


        // Command and method for navigating to the ProgrammeDetailsPage
        public ICommand ShowDetailsPageCommand => new Command<Programme>(ShowDetailsPage);

        public async void ShowDetailsPage(Programme selectedProgramme)
        {
            if (selectedProgramme != null)
            {
                var details = await _tmdbService.GetProgrammeDetailsAsync(selectedProgramme.Id);
                // Create an instance of MovieDetailsViewModel
                MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel(selectedProgramme, _tmdbService);
                // Use Shell.Current to access the Shell and navigate to the MovieDetailsPage
                await Shell.Current.GoToAsync($"//movieDetailsPage?selectedProgrammeId={selectedProgramme.Id}");
            }
        }
    }
}
