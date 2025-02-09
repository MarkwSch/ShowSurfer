using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowSurfer.Models
{
    // Class for Movies and TV Shows
    public class Programme
    {
        public int Id { get; set; }
        public string DisplayTitle { get; set; }
        public string ProgrammeType { get; set; }
        public string Poster { get; set; }
        public string PosterIcon { get; set; }
        public string PosterUrl { get; set; }
        public string Overview { get; set; }
        public string ReleaseDate { get; set; }
        public int[] GenreIds { get; set; }      
    }
}
