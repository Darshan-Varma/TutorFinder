using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Routing;
using Tutor_Finder.Models;

namespace Tutor_Finder.Controllers
{
    public class TutorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TutorData/ListTutors
        [HttpGet]
        public IEnumerable<TutorDTO> ListTutors()
        {
            List<Tutor> Tutors = db.Tutors.ToList();
            List<TutorDTO> TutorDTOs = new List<TutorDTO>();

            Tutors.ForEach(a => TutorDTOs.Add(new TutorDTO()
            {
                TutorID = a.TutorID,
                TutorFirstName = a.TutorFirstName,
                TutorLastName = a.TutorLastName
            }));

            return TutorDTOs;
        }

        // GET: api/TutorData/ListTutorsForLanguagess/3
        [HttpGet]
        public IEnumerable<TutorDTO> ListTutorsForLanguages(int id)
        {
            List<Tutor> Tutors = db.Tutors.Where(a => a.TutorID == id).ToList();
            List<TutorDTO> TutorDTOs = new List<TutorDTO>();

            Tutors.ForEach(a => TutorDTOs.Add(new TutorDTO()
            {
                TutorID = a.TutorID,
                TutorFirstName = a.TutorFirstName,
                TutorLastName = a.TutorLastName

            }));

            return TutorDTOs;
        }

        // GET: api/TutorData/ListTutorsForStudent/3
        [HttpGet]
        public IEnumerable<TutorDTO> ListTutorsForStudent(int id)
        {
            List<Tutor> Tutors = db.Tutors.Where(a => a.Students.Any(
                        k => k.StudentID == id
                )).ToList();
            List<TutorDTO> TutorDTOs = new List<TutorDTO>();

            Tutors.ForEach(a => TutorDTOs.Add(new TutorDTO()
            {
                TutorID = a.TutorID,
                TutorFirstName = a.TutorFirstName,
                TutorLastName = a.TutorLastName
            }));

            return TutorDTOs;
        }

        [HttpPost]
        [Route("api/Tutor/AssociateTutorWithStudent/{Tutorid}/{Studentid}")]
        public IHttpActionResult AssociateTutorWithStudent(int Tutorid, int Studentid)
        {

            Tutor SelectedTutor = db.Tutors.Include(a => a.Students).Where(a => a.TutorID == Tutorid).FirstOrDefault();
            Student Student = db.Students.Find(Studentid);

            if (SelectedTutor == null || Student == null)
            {
                return NotFound();
            }


            SelectedTutor.Students.Add(Student);
            db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("api/Tutor/UnAssociateTutorWithStudent/{Tutorid}/{Studentid}")]
        public IHttpActionResult UnAssociateTutorWithStudent(int Tutorid, int Studentid)
        {

            Tutor SelectedTutor = db.Tutors.Include(a => a.Students).Where(a => a.TutorID == Tutorid).FirstOrDefault();
            Student Student = db.Students.Find(Studentid);

            if (SelectedTutor == null || Student == null)
            {
                return NotFound();
            }


            SelectedTutor.Students.Remove(Student);
            db.SaveChanges();

            return Ok();
        }


        // GET: api/TutorData/FindTutor/5
        [ResponseType(typeof(Tutor))]
        [HttpGet]
        public IHttpActionResult FindTutor(int id)
        {
            Tutor Tutor = db.Tutors.Find(id);
            TutorDTO TutorDTOs = new TutorDTO()
            {
                TutorID = Tutor.TutorID,
                TutorFirstName = Tutor.TutorFirstName,
                TutorLastName = Tutor.TutorLastName
            };
            if (Tutor == null)
            {
                return NotFound();
            }

            return Ok(TutorDTOs);
        }

        // POST: api/TutorData/UpdateTutor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTutor(int id, Tutor Tutor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Tutor.TutorID)
            {
                return BadRequest();
            }

            db.Entry(Tutor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TutorExists(id))
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

        // POST: api/TutorData/AddTutor
        [ResponseType(typeof(Tutor))]
        [HttpPost]
        public IHttpActionResult AddTutor(Tutor Tutor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tutors.Add(Tutor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Tutor.TutorID }, Tutor);
        }

        // POST: api/TutorData/DeleteTutor/5
        [ResponseType(typeof(Tutor))]
        [HttpPost]
        public IHttpActionResult DeleteTutor(int id)
        {
            Tutor Tutor = db.Tutors.Find(id);
            if (Tutor == null)
            {
                return NotFound();
            }

            db.Tutors.Remove(Tutor);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TutorExists(int id)
        {
            return db.Tutors.Count(e => e.TutorID == id) > 0;
        }
    }
}