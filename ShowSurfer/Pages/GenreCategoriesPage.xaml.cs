using System.Diagnostics;
using Microsoft.Maui.Controls;
using ShowSurfer.Pages;
using ShowSurfer.ViewModels;
using ShowSurfer.Services;
using System;

namespace ShowSurfer.Pages
{
    public partial class GenreCategoriesPage : ContentPage
    {
        private readonly TopRatedViewModel _topRatedViewModel;
        // Utilises the TopRatedViewModel for the TmdbService.
        public GenreCategoriesPage(TopRatedViewModel topRatedViewModel)
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
