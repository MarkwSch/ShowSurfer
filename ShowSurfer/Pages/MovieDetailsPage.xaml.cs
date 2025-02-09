using ShowSurfer.ViewModels;
namespace ShowSurfer.Pages;

public partial class MovieDetailsPage : ContentPage
{
    private readonly MovieDetailsViewModel _movieDetailsViewModel;
	public MovieDetailsPage(MovieDetailsViewModel movieDetailsViewModel)
	{
        InitializeComponent();
        _movieDetailsViewModel = movieDetailsViewModel;
        BindingContext = _movieDetailsViewModel;
	}

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (_movieDetailsViewModel?.SelectedProgramme != null)
        {
            string titleMovie = _movieDetailsViewModel.SelectedProgramme?.DisplayTitle;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (_movieDetailsViewModel?.SelectedProgramme != null)
        {
            await _movieDetailsViewModel.InitialiseAsync();
        }
    }
}