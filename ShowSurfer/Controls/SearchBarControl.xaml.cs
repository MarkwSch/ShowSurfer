using Microsoft.Maui.Controls;
using ShowSurfer.Pages;
using ShowSurfer.ViewModels;
using System;
using System.Diagnostics;

namespace ShowSurfer.Controls;

public partial class SearchBarControl : ContentView
{
	public SearchBarControl()
	{
		InitializeComponent();
    }
	
    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        // Tries three different ViewModels as it will change depending on where the searchBar is
        var tmdbService = (BindingContext as TopRatedViewModel)?.TmdbService
                        ?? (BindingContext as SearchViewModel)?.TmdbService
                        ?? (BindingContext as GenreViewModel)?.TmdbService;

        // Get the search query from the searchBar
        string searchQuery = searchBar?.Text;
        Debug.WriteLine($"TmdbService: {tmdbService}, SearchQuery: {searchQuery}");
        if (!string.IsNullOrEmpty(searchQuery))
        {
            // Create a new searchViewModel with the searchQuery
            SearchViewModel searchViewModel = new SearchViewModel(tmdbService, searchQuery);

            // Navigate to the SearchResultPage
            await Navigation.PushAsync(new SearchResultPage(searchViewModel));

        }
        else
        {
            // Handle the case where searchQuery is null or empty
            Debug.WriteLine($"Search Query is empty: {searchQuery}");
        }
        
    }
}
