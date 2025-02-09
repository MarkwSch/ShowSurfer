using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShowSurfer.Models;
using ShowSurfer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace ShowSurfer.ViewModels
{
    public partial class TopRatedViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;
        public TopRatedViewModel(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        public TmdbService TmdbService => _tmdbService;

        public ObservableCollection<Programme> TopRated { get; set; } = new();

        public async Task InitializeAsync()
        {
            var topRatedList = await _tmdbService.GetTopRatedAsync();
            SetProgrammeCollection(topRatedList, TopRated);
        }
        // Sets the collection to the TopRated movies.
        private static void SetProgrammeCollection(IEnumerable<Programme> programmes, ObservableCollection<Programme> collection)
        {
            collection.Clear();
            foreach (var programme in programmes)
            {
                // If there is no poster icon, then use the "No Poster Found" image.
                if(programme.PosterIcon == "https://image.tmdb.org/t/p/w220_and_h330_face/")
                {
                    // WikiMedia Commons "No Poster Found" image.
                    programme.PosterIcon = "https://upload.wikimedia.org/wikipedia/commons/c/c2/No_image_poster.png?20170513175923";
                }
                collection.Add(programme);
            }
        }

        // Command for showing the movie details page.
        public ICommand ShowDetailsPageCommand => new Command<Programme>(ShowDetailsPage);
        // Method that navigates to the movie details page.
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