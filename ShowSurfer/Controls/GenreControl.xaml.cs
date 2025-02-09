using Microsoft.Maui.Controls;
using ShowSurfer.Pages;
using ShowSurfer.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace ShowSurfer.Controls;

public partial class GenreControl : ContentView
{
    public ObservableCollection<string> GenreButtonGrid { get; set; }

    public GenreControl()
    {
        // Observable Collection containing the names of the genres
        GenreButtonGrid = new ObservableCollection<string>
        {
            "ACTION", "ADVENTURE", "ANIMATION", "COMEDY", "CRIME", "DOCUMENTARY",
            "DRAMA", "FAMILY", "FANTASY", "HISTORY", "HORROR", "MUSIC",
            "MYSTERY", "ROMANCE", "SCIENCE FICTION", "TV MOVIE", "THRILLER",
            "WAR", "WESTERN"
        };

        InitializeComponent();
    }

    // Method for when the genre buttons are clicked.
    private async void GenreButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            string genreName = clickedButton.Text;
            // Calls the method to translate the genre string into the genreId
            int genreId = GetGenreId(genreName);

            if (genreId != 0)
            {
                // Set topRated as the ViewModel to use the TmdbService, doesn't really matter what
                // ViewModel is used since I am only using the TmdbService.
                var tmdbService = (BindingContext as TopRatedViewModel)?.TmdbService;

                Debug.WriteLine($"TmdbService: {tmdbService}, GenreSearchQuery: {genreId}");

                GenreViewModel genreViewModel = new GenreViewModel(tmdbService, genreId);

                // Navigates to the GenreResultsPage
                await Navigation.PushAsync(new GenreResultsPage(genreViewModel));
            }
        }
    }
    public static int GetGenreId(string genreName)
    {
        // Ternary conditional operator to determine the genreId based on the genreName
        int genreId = genreName == "ACTION" ? 28 :
                      genreName == "ADVENTURE" ? 12 :
                      genreName == "ANIMATION" ? 16 :
                      genreName == "COMEDY" ? 35 :
                      genreName == "CRIME" ? 80 :
                      genreName == "DOCUMENTARY" ? 99 :
                      genreName == "DRAMA" ? 18 :
                      genreName == "FAMILY" ? 10751 :
                      genreName == "FANTASY" ? 14 :
                      genreName == "HISTORY" ? 36 :
                      genreName == "HORROR" ? 27 :
                      genreName == "MUSIC" ? 10402 :
                      genreName == "MYSTERY" ? 9648 :
                      genreName == "ROMANCE" ? 10749 :
                      genreName == "SCIENCE FICTION" ? 878 :
                      genreName == "TV MOVIE" ? 10770 :
                      genreName == "THRILLER" ? 53 :
                      genreName == "WAR" ? 10752 :
                      genreName == "WESTERN" ? 37 :
                      0; // Default genreId is 0 (nothing)

        // Return the GenreId based on the text of the button
        return genreId;
    }
}