using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models.ViewModels
{
    public class LanguageDetails
    {
        public LanguageDto Language { get; set; }
        public IEnumerable<TutorDTO> RelatedTutors { get; set; }
    }
}