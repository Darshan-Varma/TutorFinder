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
using Tutor_Finder.Models;

namespace Tutor_Finder.Controllers
{
    public class LanguageDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/LanguageData/ListLanguages
        [HttpGet]
        public IEnumerable<LanguageDto> ListLanguages()
        {
            List<Language> Language = db.Languages.ToList();
            List<LanguageDto> LanguageDtos = new List<LanguageDto>();

            Language.ForEach(a => LanguageDtos.Add(new LanguageDto()
            {
                LanguageID = a.LanguageID,
                LanguageName = a.LanguageName,
                LanguageDescription = a.LanguageDescription
            }));

            return LanguageDtos;
        }

        // GET: api/LanguageData/FindLanguage/5
        [ResponseType(typeof(Language))]
        [HttpGet]
        public IHttpActionResult FindLanguage(int id)
        {
            Language Language = db.Languages.Find(id);
            LanguageDto LanguageDtos = new LanguageDto()
            {
                LanguageID = Language.LanguageID,
                LanguageName = Language.LanguageName,
                LanguageDescription = Language.LanguageDescription
            };
            if (Language == null)
            {
                return NotFound();
            }

            return Ok(LanguageDtos);
        }

        // PUT: api/LanguageData/UpdateLanguage/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateLanguage(int id, Language Language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Language.LanguageID)
            {
                return BadRequest();
            }

            db.Entry(Language).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
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

        // POST: api/LanguageData/AddLanguage
        [ResponseType(typeof(Language))]
        [HttpPost]
        public IHttpActionResult AddLanguage(Language Language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Languages.Add(Language);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Language.LanguageID }, Language);
        }

        // DELETE: api/LanguageData/DeleteLanguage/5
        [ResponseType(typeof(Language))]
        [HttpPost]
        public IHttpActionResult DeleteLanguage(int id)
        {
            Language Language = db.Languages.Find(id);
            if (Language == null)
            {
                return NotFound();
            }

            db.Languages.Remove(Language);
            db.SaveChanges();

            return Ok(Language);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LanguageExists(int id)
        {
            return db.Languages.Count(e => e.LanguageID == id) > 0;
        }
    }
}