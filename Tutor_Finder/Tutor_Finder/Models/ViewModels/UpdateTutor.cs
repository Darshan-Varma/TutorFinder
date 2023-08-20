using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models.ViewModels
{
    public class UpdateTutor
    {
        public TutorDTO Tutor { get; set; }


        public IEnumerable<LanguageDto> Language { get; set; }
    }
}