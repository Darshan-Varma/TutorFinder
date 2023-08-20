using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models.ViewModels
{
    public class TutorDetails
    {
        public TutorDTO Tutor { get; set; }
        public IEnumerable<StudentDto> Student { get; set; }
        public IEnumerable<StudentDto> OtherStudents { get; set; }
    }
}