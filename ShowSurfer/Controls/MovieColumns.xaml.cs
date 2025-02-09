using ShowSurfer.Models;
using ShowSurfer.Pages;
using ShowSurfer.ViewModels;
using ShowSurfer.Services;
using System.Net.Http.Json;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.ComponentModel;
using System.Diagnostics;

namespace ShowSurfer.Controls;

// EventArgs for handling the selection of a programme
public class SelectProgrammeEventArgs : EventArgs
{
	public Programme Programme { get; set; }
	public SelectProgrammeEventArgs(Programme programme) => Programme = programme;
}

public partial class MovieColumns : ContentView
{
    // Define bindable properties Heading, Programme and OnSelectProgramme
    public static readonly BindableProperty HeadingProperty = BindableProperty.Create(nameof(Heading), typeof(string), typeof(MovieColumns), string.Empty);
	public static readonly BindableProperty ProgrammeProperty = BindableProperty.Create(nameof(Programme), typeof(IEnumerable<Programme>), typeof(MovieColumns), Enumerable.Empty<Programme>());
    public static readonly BindableProperty OnSelectProgrammeCommandProperty = BindableProperty.Create(nameof(OnSelectProgrammeCommand), typeof(ICommand), typeof(MovieColumns));
    
    // ICommand property for handling the selection of a Programme
    public ICommand OnSelectProgrammeCommand
    {
        get => (ICommand)GetValue(OnSelectProgrammeCommandProperty);
        set => SetValue(OnSelectProgrammeCommandProperty, value);
    }

    public MovieColumns()
	{
		InitializeComponent();
    }

    public string Heading
	{
		get => (string)GetValue(MovieColumns.HeadingProperty); 
		set => SetValue(MovieColumns.HeadingProperty, value);
	}
    public IEnumerable<Programme> Programme
	{
		get => (IEnumerable<Programme>)GetValue(MovieColumns.ProgrammeProperty); 
		set => SetValue(MovieColumns.ProgrammeProperty, value);
	}

    // Pushes user to the Movie Details Page when tapped
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border && border.BindingContext is Programme selectedProgramme)
        {
            // Tries three different ViewModels since the MovieColumns control is importanted into these three
            var tmdbService = (BindingContext as TopRatedViewModel)?.TmdbService
                              ?? (BindingContext as SearchViewModel)?.TmdbService
                              ?? (BindingContext as GenreViewModel)?.TmdbService;

            // Passes the selected programme and the tmdbservice through an instance of MovieDetailsViewModel
            MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel(selectedProgramme, tmdbService);

            // Navigate to the MovieDetailsPage
            await Navigation.PushAsync(new MovieDetailsPage(movieDetailsViewModel));
        }
    }
}