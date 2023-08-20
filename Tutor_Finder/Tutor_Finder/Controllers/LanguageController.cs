using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Tutor_Finder.Models;
using Tutor_Finder.Models.ViewModels;

namespace Tutor_Finder.Controllers
{
    public class LanguageController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();
        static LanguageController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44309/api/");
        }
        // GET: Language/List
        public ActionResult List()
        {
            string url = "LanguageData/ListLanguages";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<LanguageDto> Language = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;


            return View(Language);
        }

        // GET: Language/Details/5
        public ActionResult Details(int id)
        {
            LanguageDetails ViewModel = new LanguageDetails();

            string url = "LanguageData/FindLanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            LanguageDto Language = response.Content.ReadAsAsync<LanguageDto>().Result;
            ViewModel.Language = Language;

            url = "Tutordata/ListTutorsForLanguages/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TutorDTO> RelatedTutors = response.Content.ReadAsAsync<IEnumerable<TutorDTO>>().Result;
            ViewModel.RelatedTutors = RelatedTutors;

            return View(ViewModel);
        }

        // GET: Language/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Language/Create
        [HttpPost]
        public ActionResult Create(Language Language)
        {
            string url = "LanguageData/addLanguage";

            string jsonpayload = jss.Serialize(Language);

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

        // GET: Language/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Languagedata/findLanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LanguageDto Language = response.Content.ReadAsAsync<LanguageDto>().Result;
            return View(Language);
        }

        // POST: Language/Update/5
        [HttpPost]
        public ActionResult Update(int id, Language Language)
        {
            string url = "Languagedata/updateLanguage/" + id;
            string jsonpayload = jss.Serialize(Language);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Language/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "LanguageData/findLanguage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            LanguageDto Language = response.Content.ReadAsAsync<LanguageDto>().Result;


            return View(Language);
        }
        public ActionResult Error()
        {
            return View();
        }

        // POST: Language/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Language Language)
        {
            string url = "LanguageData/deleteLanguage/" + id;
            string jsonpayload = jss.Serialize(Language);

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
