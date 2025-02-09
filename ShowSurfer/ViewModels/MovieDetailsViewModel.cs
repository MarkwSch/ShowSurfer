using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ShowSurfer.Services;
using ShowSurfer.Pages;
using ShowSurfer.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace ShowSurfer.ViewModels
{
    public partial class MovieDetailsViewModel : ObservableObject
    {
        private readonly TmdbService _tmdbService;
        private Programme _selectedProgramme;
        private WatchProviders _watchProviders;
        public WatchProviders WatchProviders
        {
            get => _watchProviders;
            set => SetProperty(ref _watchProviders, value);
        }
        
        // Streaming Availability Collections - TMDB refers to streaming as 'Flatrate'
        public ObservableCollection<FlatRate> FlatrateCollection { get; } = new();
        public ObservableCollection<FlatRate> FilteredFlatrateCollection { get; } = new();

        // Creates a list of unique country names.
        private static readonly List<string> CountryCodesList = new List<string>
        {
            "Australia", "Belgium", "Canada", "Denmark", "France", "Great Britain",
            "Hong Kong", "India", "Japan", "Netherlands", "New Zealand", "Norway",
            "Singapore", "Switzerland", "Sweden", "United States of America"
        };
        private ObservableCollection<string> _uniqueCountryCodes = new ObservableCollection<string>(CountryCodesList);
        public ObservableCollection<string> UniqueCountryCodes
        {
            get => _uniqueCountryCodes;
            set => SetProperty(ref _uniqueCountryCodes, value);
        }
        // Initialises the observable properties for each attribute that I bring into the UI.
        [ObservableProperty]
        private int _runtime;
        [ObservableProperty]
        public string _original_language;
        [ObservableProperty]
        public string _original_title;
        [ObservableProperty]
        public string _overview;
        [ObservableProperty]
        public float _popularity;
        [ObservableProperty]
        public string _release_date;
        [ObservableProperty]
        public string _tagline;
        [ObservableProperty]
        public string _companyName;
        [ObservableProperty]
        public string _movieBackdrop;
        [ObservableProperty]
        public string _job;
        [ObservableProperty]
        public string _genreName;
        [ObservableProperty]
        public float _vote_average;
        [ObservableProperty]
        public int _vote_percentage;
        [ObservableProperty]
        public int _vote_count;
        [ObservableProperty]
        public string _vote_string;
        [ObservableProperty]
        public int _revenue;
        [ObservableProperty]
        public string firstMovieBackDrop;

        // Property to combine Production Companies into one string.
        [ObservableProperty]
        public string _productionCompaniesConcatenate;
        // Property to combine genres into one string.
        [ObservableProperty]
        public string _genresConcatenate;

        private string _selectedCountryCode;

        public MovieDetailsViewModel(Programme selectedProgramme, TmdbService tmdbService)
        {
            SelectedProgramme = selectedProgramme;
            _tmdbService = tmdbService;
        }

        public Programme SelectedProgramme
        {
            get => _selectedProgramme;
            set => SetProperty(ref _selectedProgramme, value);
        }

        public string SelectedCountryCode
        {
            get => _selectedCountryCode;
            set
            {
                SetProperty(ref _selectedCountryCode, value);
                UpdateDisplayedItems();
            }
        }
        // Initialising the bool for streaming availability message
        private bool _noStreamingVisibility;
        public bool NoStreamingVisibility
        {
            get => _noStreamingVisibility;
            set => SetProperty(ref _noStreamingVisibility, value);
        }

        // Initialising the collection for the Production Companies
        private ObservableCollection<Production_Companies> _productionCompanies = new ObservableCollection<Production_Companies>();
        public ObservableCollection<Production_Companies> ProductionCompanies
        {
            get => _productionCompanies;
            set => SetProperty(ref _productionCompanies, value);

        }
        // Initialising the collection for the Genres
        private ObservableCollection<Genre> _genres = new ObservableCollection<Genre>();
        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        // Initialising the collection for the Cast
        private ObservableCollection<Cast> _actorsCollection = new ObservableCollection<Cast>();
        public ObservableCollection<Cast> ActorsCollection
        {
            get => _actorsCollection;
            set => SetProperty(ref _actorsCollection, value);
        }

        // Initialising the collection for the Crew
        private ObservableCollection<Crew> _crewCollection = new ObservableCollection<Crew>();
        public ObservableCollection<Crew> CrewCollection
        {
            get => _crewCollection;
            set => SetProperty(ref _crewCollection, value);
        }
        // Initialising collection for the Backdrops
        private ObservableCollection<Backdrops> _movieImageCollection = new ObservableCollection<Backdrops>();
        public ObservableCollection<Backdrops> MovieImageCollection
        {
            get => _movieImageCollection;
            set => SetProperty(ref _movieImageCollection, value);
        }

        // Initialise
        public async Task InitialiseAsync()
        {
            Debug.WriteLine($"SelectedProgramme: {_selectedProgramme}");
            Debug.WriteLine($"Details: {_tmdbService}");

            // Call the Movie Image method
            var _movieImages = await _tmdbService.GetMovieImageResultsAsync(_selectedProgramme.Id);
            // Get a list of avaialable backdrops and store them. Might turn it into a carousel view at a later point,
            // but I think the single image looks better for now.
            foreach(Backdrops backdrops in _movieImages.backdrops)
            {
                string MovieBackdrop = backdrops.file_icon;
                MovieImageCollection.Add(backdrops);
            }
            // If there are Backdrops provided, set it as the first one.
            if (MovieImageCollection.Count > 0)
            {
                FirstMovieBackDrop = MovieImageCollection[0].file_icon;
            }
            

            // Call the Cast List method to populate the page
            var _castDetails = await _tmdbService.GetCastResultsAsync(_selectedProgramme.Id);
            Debug.WriteLine(_castDetails);
            CrewCollection.Clear();
            ActorsCollection.Clear();
            // For each cast in the list, store their details.
            foreach (Cast cast in _castDetails.cast)
            {
                string CastName = cast.name;
                string Character = cast.character;
                string CastCharacter = cast.cast_character;
                string Role = cast.known_for_department;
                string ProfileIcon = cast.profile_icon;
                cast.profile_icon = $"https://image.tmdb.org/t/p/h632{cast.profile_path}";

                // Only get the first 10 actors/actresses.
                if (cast.known_for_department == "Acting" && ActorsCollection.Count < 10)
                {
                    // Set crew_job to both the name and character of the person. Easier to display.
                    if (cast.character != null)
                    {
                        cast.cast_character = $"{cast.name} as {cast.character}";
                    }
                    else
                    {
                        // If they don't have a character (e.g common in Documentaries) then just display their name.
                        cast.cast_character = $"{cast.name}";
                    }
                    if (cast.profile_path == null)
                    {
                        // Wikimedia Commons Default Profile Image
                        cast.profile_icon = "https://upload.wikimedia.org/wikipedia/commons/7/72/Default-welcomer.png";
                    }
                    
                    ActorsCollection.Add(cast);
                }
            }
            // For each crew member in the list, store their details.
            foreach (Crew crew in _castDetails.crew)
            {
                string CrewName = crew.name;
                string Job = crew.job;
                string CrewJob = crew.crew_job;
                string ProfileIcon = crew.profile_icon;

                // Set crew_job to both the name and job of the person. Easier to display.
                if (crew.job != null)
                {
                    crew.crew_job = $"{crew.name} as {crew.job}";
                }
                else
                {
                    crew.crew_job = $"{crew.name}";
                }
                // Get profile images for each person
                if (crew.profile_path == null)
                {
                    crew.profile_icon = "https://upload.wikimedia.org/wikipedia/commons/7/72/Default-welcomer.png";
                }
                // If there aren't any profile images available, use the default on from Wikimedia Commons.
                else
                {
                    crew.profile_icon = $"https://image.tmdb.org/t/p/h632{crew.profile_path}";
                }
                // Display only the Producers and Directors
                if (crew.job == "Director")
                {
                    CrewCollection.Add(crew);
                }
                if (crew.job == "Producer")
                {
                    CrewCollection.Add(crew);
                }
            }

            // Call the GetProgrammeDetails method to populate the page with more information.
            var _details = await _tmdbService.GetProgrammeDetailsAsync(_selectedProgramme.Id);
            if (_selectedProgramme != null && _details != null)
            {
                // Use _details to populate properties
                Runtime = _details.runtime;
                Original_title = _details.original_title;
                Original_language = _details.original_language;
                Overview = _details.overview;
                Popularity = _details.popularity;
                Release_date = _details.release_date;
                Revenue = _details.revenue;
                Tagline = _details.tagline;
                Vote_average = _details.vote_average;
                Vote_count = _details.vote_count;
                Vote_percentage = (int)Math.Ceiling(Vote_average * 10);
                Vote_string = $"{Vote_percentage}% ({Vote_count} votes)";
            }


            // Obtain Genre Information
            Genres.Clear();
            foreach (Genre genre in _details.genres)
            {
                GenreName = genre.Name;
                Genres.Add(genre);
            }
            // Concatenate genre names to display them in the UI
            GenresConcatenate = string.Join(", ", Genres.Select(c => c.Name));


            // Populate Production Companies
            ProductionCompanies.Clear();
            foreach (Production_Companies company in _details.production_companies)
            {
                CompanyName = company.name;
                ProductionCompanies.Add(company);
            }
            // Concatenate production company names to easily display them in the UI
            ProductionCompaniesConcatenate = string.Join(", ", ProductionCompanies.Select(c => c.name));


            // Get watch providers information
            WatchProviders = await _tmdbService.GetWatchProvidersAsync(SelectedProgramme.Id);
            // Display the entire watch providers information
            DisplayWatchProvidersInfo();
            // Set an initial value for the selected country code
            SelectedCountryCode = UniqueCountryCodes.FirstOrDefault();
            // Update displayed items based on the initial country code
            UpdateDisplayedItems();
        }


        // Method to store the WatchProviders based on the countryCode
        private void DisplayWatchProvidersInfo()
        {
            if (WatchProviders?.results != null)
            {
                foreach (var countryCodePair in WatchProviders.results)
                {
                    string countryCode = countryCodePair.Key;
                    // Calls the CountryCodetoName method
                    countryCode = CountyCodetoName(countryCode);
                    var countryInfo = countryCodePair.Value;

                    // Debugging
                    Debug.WriteLine($"Watch Providers for {countryCode}:");
                    Debug.WriteLine($"Link: {countryInfo.link}");
                    Debug.WriteLine("Flatrate:");
                    // Display flatrate details
                    DisplayWatchProviders(countryInfo.flatrate, countryCode);
                }
            }
        }

        // Method to add the Watch Providers for each country. Binds the country code to the FlatRate class for filtering later.
        private void DisplayWatchProviders(IEnumerable<FlatRate> items, string countryCode_string)
        {
            // Converts the items in the list into accessible FlatRate items for the UI
            foreach (FlatRate item in items ?? Enumerable.Empty<FlatRate>())
            {
                string logoPath = item.logo_path;
                int providerId = item.provider_id;
                string providerName = item.provider_name;
                int displayPriority = item.display_priority;
                item.country_code = countryCode_string;

                Debug.WriteLine($"LogoPath: {logoPath}, ProviderId: {providerId}, ProviderName: {providerName}, DisplayPriority: {displayPriority}, countryCode:{countryCode_string}");

                // Add the item to FlatrateCollection
                FlatrateCollection.Add(item);
            }
        }

        private void UpdateDisplayedItems()
        {
            // Clear the collection before updating
            FilteredFlatrateCollection.Clear();

            // Filter and add items with the selected country code
            foreach (var item in FlatrateCollection.Where(f => f.country_code == SelectedCountryCode))
            {
                FilteredFlatrateCollection.Add(item);
            }
            // Check if there are streaming options, if there is no options display the message
            if (FilteredFlatrateCollection.Count == 0)
            {
                NoStreamingVisibility = true;
            }
            else
            {
                // No message needed if there is streaming availability
                NoStreamingVisibility = false;
            }
        }
            
        // Method to change the country code into a name for the Picker
        public static string CountyCodetoName(string countryCode)
        {
            // Converts the country code into the country name
            countryCode = countryCode == "AU" ? "Australia" :
                countryCode == "BE" ? "Belgium" :
                countryCode == "CA" ? "Canada" :
                countryCode == "CH" ? "Switzerland" :
                countryCode == "DK" ? "Denmark" :
                countryCode == "FR" ? "France" :
                countryCode == "GB" ? "Great Britain" :
                countryCode == "HK" ? "Hong Kong" :
                countryCode == "IN" ? "India" :
                countryCode == "JP" ? "Japan" :
                countryCode == "NL" ? "Netherlands" :
                countryCode == "NO" ? "Norway" :
                countryCode == "NZ" ? "New Zealand" :
                countryCode == "SG" ? "Singapore" :
                countryCode == "SE" ? "Sweden" :
                countryCode == "US" ? "United States of America" :
                countryCode; // If it couldn't find a name, just leave as the countryCode

            return countryCode;
        }

    }
}

