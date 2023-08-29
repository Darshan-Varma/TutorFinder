using Newtonsoft.Json;
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
            //client.BaseAddress = new Uri("http://darshan0305-001-site1.gtempurl.com/api/");
        }
        // GET: Language/List
        public ActionResult List(Student student)
        {
            if (Session["StudentID"] != null || Session["AdminID"] !=null)
            {
                string url = "LanguageData/ListLanguages";
                HttpResponseMessage response = client.GetAsync(url).Result;

                IEnumerable<LanguageDto> Language = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;


                return View(Language);
            }
            else if(student.StudentEmailID != null)
            {
                bool isLogin = CheckLogin(student);
                if (isLogin)
                {
                    string url = "LanguageData/ListLanguages";
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    IEnumerable<LanguageDto> Language = response.Content.ReadAsAsync<IEnumerable<LanguageDto>>().Result;


                    return View(Language);
                }
                else
                {
                    return RedirectToAction("LoginFailed");
                }
            }
            else
            {
                return RedirectToAction("LoginFailed");
            }
            
        }
        // GET: Language/LoginFailedAdmin
        public ActionResult LoginFailedAdmin()
        {
            return View();
        }
        // GET: Language/LoginFailed
        public ActionResult LoginFailed()
        {
            return View();
        }
        // GET: Language/Details/5
        public ActionResult Details(int id)
        {
            if (Session["StudentID"] != null || Session["AdminID"] !=null)
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
            else
            {
                return RedirectToAction("LoginFailed");
            }
        }

        // GET: Language/New
        public ActionResult New()
        {
            if (Session["AdminID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginFailedAdmin");
            }
        }

        // POST: Language/Create
        [HttpPost]
        public ActionResult Create(Language Language)
        {
            if (Session["AdminID"] != null)
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
            else
            {
                return RedirectToAction("LoginFailedAdmin");
            }
        }


        // POST: Language/CheckLogin
        [HttpPost]
        public bool CheckLogin(Student student)
        {
            string url = "StudentData/CheckLogin";

            string jsonpayload = jss.Serialize(student);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            var response = client.PostAsync(url, content).Result;
            //bool login = response.Content.ReadAsAsync<bool>().Result;
            IEnumerable<Student> loggedInStudent = response.Content.ReadAsAsync<IEnumerable<Student>>().Result;

            if (loggedInStudent.Count()>0)
            {
                Student StudentLoggedIn = loggedInStudent.SingleOrDefault();

                Session["StudentID"] = StudentLoggedIn.StudentID;
                Session["EmailID"] = StudentLoggedIn.StudentEmailID;
                Session["StudentName"] = StudentLoggedIn.StudentFirstName;
                Session["isAdmin"] = "false";
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Language/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["AdminID"] != null)
            {
                string url = "Languagedata/findLanguage/" + id;
                HttpResponseMessage response = client.GetAsync(url).Result;
                LanguageDto Language = response.Content.ReadAsAsync<LanguageDto>().Result;
                return View(Language);
            }
            else
            {
                return RedirectToAction("LoginFailedAdmin");
            }
        }

        // POST: Language/Update/5
        [HttpPost]
        public ActionResult Update(int id, Language Language)
        {
            if (Session["AdminID"] != null)
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
            else
            {
                return RedirectToAction("LoginFailedAdmin");
            }
        }

        // GET: Language/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            if (Session["AdminID"] != null)
            {
                string url = "LanguageData/findLanguage/" + id;
                HttpResponseMessage response = client.GetAsync(url).Result;
                LanguageDto Language = response.Content.ReadAsAsync<LanguageDto>().Result;


                return View(Language);
            }
            else
            {
                return RedirectToAction("LoginFailedAdmin");
            }
        }
        public ActionResult Error()
        {
            if (Session["AdminID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginFailedAdmin");
            }
        }

        // POST: Language/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Language Language)
        {
            if (Session["AdminID"] != null)
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
            else
            {
                return RedirectToAction("LoginFailedAdmin");
            }
        }
    }
}
