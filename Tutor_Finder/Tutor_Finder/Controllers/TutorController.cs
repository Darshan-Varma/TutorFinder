using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Tutor_Finder.Models;
using Tutor_Finder.Models.ViewModels;

namespace Tutor_Finder.Controllers
{
    public class TutorController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();
        static TutorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44309/api/");
        }
        // GET: Tutor/List
        public ActionResult List()
        {
            string url = "TutorData/ListTutors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TutorDTO> Tutor = response.Content.ReadAsAsync<IEnumerable<TutorDTO>>().Result;


            return View(Tutor);
        }

        //GET: Tutor/Details/5
        public ActionResult Details(int id)
        {
            TutorDetails ViewModel = new TutorDetails();
            string url = "TutorData/FindTutor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            TutorDTO Tutor = response.Content.ReadAsAsync<TutorDTO>().Result;
            ViewModel.Tutor = Tutor;

            url = "Studentdata/listStudentsforTutor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StudentDto> Students = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            ViewModel.Student = Students;

            url = "Studentdata/ListOtherStudents/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StudentDto> OtherStudents = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            ViewModel.OtherStudents = OtherStudents;

            return View(ViewModel);
        }
        //GET: Tutor/DetailsLanguage/5
        public ActionResult DetailsLanguage(int id)
        {
            TutorDetails ViewModel = new TutorDetails();
            string url = "TutorData/FindTutor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            TutorDTO Tutor = response.Content.ReadAsAsync<TutorDTO>().Result;
            ViewModel.Tutor = Tutor;

            url = "Studentdata/listStudentsforTutor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StudentDto> Students = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            ViewModel.Student = Students;

            url = "Studentdata/ListOtherStudents/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StudentDto> OtherStudents = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
            ViewModel.OtherStudents = OtherStudents;

            url = "Studentdata/listLanguagesforTutor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<LanguageDto> Languages = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;
            ViewModel.Language = Languages;

            url = "Studentdata/ListOtherLanguages/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<LanguageDto> OtherLanguages = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;
            ViewModel.OtherLanguages = OtherLanguages;

            return View(ViewModel);
        }

        //POST: Tutor/Associate/{Tutorid}
        [HttpPost]
        public ActionResult Associate(int id, int StudentId)
        {
            string url = "Tutor/AssociateTutorWithStudent/" + id + "/" + StudentId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }
        //POST: Tutor/AssociateLanguage/{Tutorid}
        [HttpPost]
        public ActionResult AssociateLanguage(int id, int LanguageID)
        {
            string url = "Tutor/AssociateTutorWithLanguage/" + id + "/" + LanguageID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("DetailsLanguage/" + id);
        }

        //POST: Tutor/UnAssociate/{Tutorid}
        [HttpGet]
        public ActionResult UnAssociate(int id, int StudentId)
        {
            string url = "Tutor/UnAssociateTutorWithStudent/" + id + "/" + StudentId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Tutor/New
        public ActionResult New()
        {
            string url = "LanguageData/ListLanguages";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<LanguageDto> Language = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;


            return View(Language);
        }

        // POST: Tutor/Create
        [HttpPost]
        public ActionResult Create(Tutor Tutor)
        {
            //curl -H "Content-Type:application/json" -d @Tutor.json https://localhost:44322/api/TutorData/addTutor 
            string url = "TutorData/addTutor";

            string jsonpayload = jss.Serialize(Tutor);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Tutor/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateTutor ViewModel = new UpdateTutor();
            string url = "TutorData/findTutor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TutorDTO Tutor = response.Content.ReadAsAsync<TutorDTO>().Result;

            ViewModel.Tutor = Tutor;

            url = "LanguageData/ListLanguages/";
            response = client.GetAsync(url).Result;
            IEnumerable<LanguageDto> Language = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;

            ViewModel.Language = Language;

            return View(ViewModel);
        }

        // POST: Tutor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Tutor Tutor)
        {
            string url = "TutorData/updateTutor/" + id;
            string jsonpayload = jss.Serialize(Tutor);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Tutor/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TutorData/findTutor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TutorDTO Tutor = response.Content.ReadAsAsync<TutorDTO>().Result;


            return View(Tutor);
        }

        // POST: Tutor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Tutor Tutor)
        {
            string url = "TutorData/deleteTutor/" + id;
            string jsonpayload = jss.Serialize(Tutor);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
