using ShowSurfer.ViewModels;
using ShowSurfer.Services;
using ShowSurfer.Models;
using ShowSurfer.Controls;
using System.Reflection.Metadata;

namespace ShowSurfer.Pages
{
    public partial class GenreResultsPage : ContentPage
    {
        private readonly GenreViewModel _genreViewModel;

        // Imports the GenreViewModel into the GenreResultsPage
        public GenreResultsPage(GenreViewModel genreViewModel)
        {
            InitializeComponent();
            _genreViewModel = genreViewModel;
            BindingContext = _genreViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _genreViewModel.InitializeAsync();
        }
    }
}

