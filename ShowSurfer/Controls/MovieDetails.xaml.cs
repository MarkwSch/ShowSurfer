using ShowSurfer.Models;

namespace ShowSurfer.Controls;

public partial class MovieDetails : ContentView
{
	public static readonly BindableProperty ProgrammeProperty = BindableProperty.Create(nameof(Programme), typeof(Programme), typeof(MovieDetails), null);

	public MovieDetails()
	{
		InitializeComponent();
	}
	public Programme Programme 
	{ 
		get => (Programme)GetValue(MovieDetails.ProgrammeProperty); 
		set => SetValue(MovieDetails.ProgrammeProperty, value); 
	}
}