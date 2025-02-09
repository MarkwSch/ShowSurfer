using ShowSurfer.Services;
using ShowSurfer.ViewModels;
using ShowSurfer.Models;
using ShowSurfer.Controls;

namespace ShowSurfer.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly TopRatedViewModel _topRatedViewModel;
        // Imports the TopRatedViewModel into the MainPage and sets it as the BindingContext.
        public MainPage(TopRatedViewModel topRatedViewModel)
        {
            InitializeComponent();
            _topRatedViewModel = topRatedViewModel;
            BindingContext = _topRatedViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _topRatedViewModel.InitializeAsync();
            
        }
    }
}