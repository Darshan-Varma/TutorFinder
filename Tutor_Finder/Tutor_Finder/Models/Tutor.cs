using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models
{
    public class Tutor
    {
        [Key]
        public int TutorID { get; set; }
        public string TutorFirstName { get; set; }
        public string TutorLastName { get; set; }
        public int TutorRating { get; set; }
        public string TutorDescription { get; set; }
        public ICollection<Language> Languages { get; set; }
        public ICollection<Student> Students { get; set; }
    }

    public class TutorDTO
    {
        public int TutorID { get; set; }
        public string TutorFirstName { get; set; }
        public string TutorLastName { get; set; }
        public string LanguageName { get; set; }
    }
}   