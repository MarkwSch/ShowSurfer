using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowSurfer.Services
{
    public class TmdbService
    {
        // My API Key from TMDB
        private const string ApiKey = "Enter your API KEY Here";

        public const string TmdbHttpClientName = "TmdbClient";

        private readonly IHttpClientFactory _httpClientFactory;

        public TmdbService (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient HttpClient => _httpClientFactory.CreateClient(TmdbHttpClientName);
    }
}
