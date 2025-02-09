using Microsoft.Maui.Controls;
using ShowSurfer.Pages;
using ShowSurfer.ViewModels;
using System;
using System.Diagnostics;

namespace ShowSurfer.Controls;

public partial class GenreCategoriesControl : ContentView
{
    public GenreCategoriesControl()
    {
        InitializeComponent();
    }
    // Assuming you have a SearchBar named searchBar in your XAML
    private async void GenreButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            string buttonText = clickedButton.Text;

            int genreId = 0;

            // Assign the genreID based on the text of the button 
            if (buttonText == "ACTION")
            {
                genreId = 28;
                Debug.WriteLine(buttonText);
            }
            else if (buttonText == "ADVENTURE")
            {
                Debug.WriteLine(buttonText);
                // Do something for the "ADVENTURE" button
            }
            // Add more conditions as needed



            if (genreId != 0)
            {
                // API call using genreID
                var tmdbService = (BindingContext as GenreViewModel)?.TmdbService
                               ?? (BindingContext as SearchViewModel)?.TmdbService;
                Debug.WriteLine($"TmdbService: {tmdbService}, GenreSearchQuery: {genreId}");

                GenreViewModel genreViewModel = new GenreViewModel(tmdbService, genreId);

                // Navigate to the SearchResultPage, passing the search query if needed
                await Navigation.PushAsync(new GenreResultsPage(genreViewModel));
            }
        }
    }

    public GenreViewModel _genreViewModel
    {
        get => (GenreViewModel)BindingContext;
        set => BindingContext = value;
    }
    public SearchViewModel _searchViewModel
    {
        get => (SearchViewModel)BindingContext;
        set => BindingContext = value;
    }
}