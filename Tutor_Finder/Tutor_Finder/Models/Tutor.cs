using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models
{
    public class Tutor
    {
        public int TutorID { get; set; }
        public string TutorFirstName { get; set; }
        public string TutorLastName { get; set; }
        public int TutorRating { get; set; }
        public string TutorDescription { get; set; }
    }
}   