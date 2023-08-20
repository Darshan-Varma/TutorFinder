using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models.ViewModels
{
    public class StudentDetails
    {
        public StudentDto Student { get; set; }
        public IEnumerable<TutorDTO> AssociatedTutors { get; set; }
    }
}