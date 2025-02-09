using ShowSurfer.ViewModels;
using ShowSurfer.Services;
using ShowSurfer.Models;
using ShowSurfer.Controls;
namespace ShowSurfer.Pages
{
    public partial class SearchResultPage : ContentPage
    {
        private readonly SearchViewModel _searchViewModel;
        // Imports the SearchViewModel and sets it as the binding context
        public SearchResultPage(SearchViewModel searchViewModel)
        {
            InitializeComponent();
            _searchViewModel = searchViewModel;
            BindingContext = _searchViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _searchViewModel.InitializeAsync();
        }
    }
}

