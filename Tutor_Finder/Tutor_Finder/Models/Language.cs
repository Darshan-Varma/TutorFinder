using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models
{
    public class Language
    {
        [Key]
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }
        public string LanguageDescription { get; set; }
        public ICollection<Tutor> Tutors { get; set; }   
    }

    public class LanguageDto
    {
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }
        public string LanguageDescription { get; set; }
    }
}