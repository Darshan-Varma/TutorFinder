using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tutor_Finder.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; } 
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentSemester { get; set; }
        public string StudentNumber { get; set; }
        public ICollection<Tutor> Tutors { get; set; }

    }

    public class StudentDto
    {
        public int StudentID { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
    }
}