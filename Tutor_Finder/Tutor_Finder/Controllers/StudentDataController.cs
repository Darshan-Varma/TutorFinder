using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.ModelBinding;
using System.Xml.Linq;
using Tutor_Finder.Models;

namespace Tutor_Finder.Controllers
{
    public class StudentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StudentData/ListStudents
        [System.Web.Http.HttpGet]
        public IEnumerable<StudentDto> ListStudents()
        {
            List<Student> Student = db.Students.ToList();
            List<StudentDto> StudentDtos = new List<StudentDto>();

            Student.ForEach(a => StudentDtos.Add(new StudentDto()
            {
                StudentID = a.StudentID,
                StudentFirstName = a.StudentFirstName,
                StudentLastName = a.StudentLastName
            }));

            return StudentDtos;
        }

        // GET: api/StudentData/ListStudentsForTutor/2
        [System.Web.Http.HttpGet]
        public IEnumerable<StudentDto> ListStudentsForTutor(int id)
        {
            List<Student> Student = db.Students.Where(
                    k => k.Tutors.Any(
                        a => a.TutorID == id)
                    ).ToList();
            List<StudentDto> StudentDtos = new List<StudentDto>();

            Student.ForEach(a => StudentDtos.Add(new StudentDto()
            {
                StudentID = a.StudentID,
                StudentFirstName = a.StudentFirstName,
                StudentLastName = a.StudentLastName,
                StudentNumber = a.StudentNumber,
                StudentSemester = a.StudentSemester,
                StudentEmailID = a.StudentEmailID,
                StudentContactNumber = a.StudentContactNumber
            }));

            return StudentDtos;
        }
        // GET: api/StudentData/ListLanguagesForTutor/2
        [System.Web.Http.HttpGet]
        public IEnumerable<LanguageDto> ListLanguagesForTutor(int id)
        {
            List<Language> Language = db.Languages.Where(
                    k => k.Tutors.Any(
                        a => a.TutorID == id)
                    ).ToList();
            List<LanguageDto> LanguageDtos = new List<LanguageDto>();

            Language.ForEach(a => LanguageDtos.Add(new LanguageDto()
            {
                LanguageID = a.LanguageID,
                LanguageName = a.LanguageName,
                LanguageDescription = a.LanguageDescription
            }));

            return LanguageDtos;
        }

        // GET: api/StudentData/ListOtherStudents/2
        [System.Web.Http.HttpGet]
        public IEnumerable<StudentDto> ListOtherStudents(int id)
        {
            List<Student> Student = db.Students.Where(
                    k => !k.Tutors.Any(
                        a => a.TutorID == id)
                    ).ToList();
            List<StudentDto> StudentDtos = new List<StudentDto>();

            Student.ForEach(a => StudentDtos.Add(new StudentDto()
            {
                StudentID = a.StudentID,
                StudentFirstName = a.StudentFirstName,
                StudentLastName = a.StudentLastName
            }));

            return StudentDtos;
        }
        

        // GET: api/StudentData/ListOtherLanguages/2
        [System.Web.Http.HttpGet]
        public IEnumerable<LanguageDto> ListOtherLanguages(int id)
        {
            List<Language> Language = db.Languages.Where(
                    k => !k.Tutors.Any(
                        a => a.TutorID == id)
                    ).ToList();
            List<LanguageDto> LanguageDtos = new List<LanguageDto>();

            Language.ForEach(a => LanguageDtos.Add(new LanguageDto()
            {
                LanguageID = a.LanguageID,
                LanguageName = a.LanguageName,
                LanguageDescription = a.LanguageDescription
            }));

            return LanguageDtos;
        }

        // GET: api/StudentData/FindStudent/5
        [ResponseType(typeof(Student))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult FindStudent(int id)
        {
            Student Student = db.Students.Find(id);
            StudentDto StudentDtos = new StudentDto()
            {
                StudentID = Student.StudentID,
                StudentFirstName = Student.StudentFirstName,
                StudentLastName = Student.StudentLastName,
                StudentNumber = Student.StudentNumber,
                StudentSemester = Student.StudentSemester

            };
            if (Student == null)
            {
                return NotFound();
            }

            return Ok(StudentDtos);
        }

        // PUT: api/StudentData/UpdateStudent/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateStudent(int id, Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Student.StudentID)
            {
                return BadRequest();
            }

            db.Entry(Student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/StudentData/AddStudent
        [ResponseType(typeof(Student))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult AddStudent(Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(Student);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Student.StudentID }, Student);
        }

        // POST: api/StudentData/CheckLogin
        [ResponseType(typeof(Student))]
        [System.Web.Http.HttpPost]
        public IEnumerable<Student> CheckLogin(Student student)
        {
            List<Student> isStudentLoggedIn = db.Students.Where(x => x.StudentEmailID == student.StudentEmailID
                                        && x.StudentPassword == student.StudentPassword).ToList();
            return isStudentLoggedIn;
        }

        // DELETE: api/StudentData/DeleteStudent/5
        [ResponseType(typeof(Student))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student Student = db.Students.Find(id);
            if (Student == null)
            {
                return NotFound();
            }

            db.Students.Remove(Student);
            db.SaveChanges();

            return Ok(Student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.StudentID == id) > 0;
        }
    }
}