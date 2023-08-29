using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
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
            //client.BaseAddress = new Uri("http://darshan0305-001-site1.gtempurl.com/api/");
        }
        // GET: Tutor/List
        public ActionResult List(Admin admin)
        {
            if (Session["AdminID"] != null)
            {
                string url = "TutorData/ListTutors";
                HttpResponseMessage response = client.GetAsync(url).Result;

                IEnumerable<TutorDTO> Tutor = response.Content.ReadAsAsync<IEnumerable<TutorDTO>>().Result;


                return View(Tutor);
            }
            else if (admin.AdminEmailID != null)
            {
                bool isLogin = CheckAdminLogin(admin);
                if (isLogin)
                {
                    string url = "TutorData/ListTutors";
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    IEnumerable<TutorDTO> Tutor = response.Content.ReadAsAsync<IEnumerable<TutorDTO>>().Result;


                    return View(Tutor);
                }
                else
                    return RedirectToAction("LoginFailedAdmin");
            }
            else
                return RedirectToAction("LoginFailedAdmin");
        }
        // GET: Tutor/MainPage
        public ActionResult MainPage(Tutor tutor)
        {
            
            if (Session["TutorID"] != null)
            {
                TutorDetails ViewModel = new TutorDetails();
                string url = "TutorData/FindTutor/" + Session["TutorID"];
                HttpResponseMessage response = client.GetAsync(url).Result;

                TutorDTO Tutor = response.Content.ReadAsAsync<TutorDTO>().Result;
                ViewModel.Tutor = Tutor;

                url = "Studentdata/listStudentsforTutor/" + Session["TutorID"];
                response = client.GetAsync(url).Result;
                IEnumerable<StudentDto> Students = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
                ViewModel.Student = Students;


                return View(ViewModel);
            }
            else if(tutor.EmailID != null)
            {
                bool isLogin = CheckLogin(tutor);
                if (isLogin)
                {
                    TutorDetails ViewModel = new TutorDetails();
                    string url = "TutorData/FindTutor/" + Session["TutorID"];
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    TutorDTO Tutor = response.Content.ReadAsAsync<TutorDTO>().Result;
                    ViewModel.Tutor = Tutor;

                    url = "Studentdata/listStudentsforTutor/" + Session["TutorID"];
                    response = client.GetAsync(url).Result;
                    IEnumerable<StudentDto> Students = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;
                    ViewModel.Student = Students;


                    return View(ViewModel);
                }
                else
                    return RedirectToAction("LoginFailed");
            }
            else
                return RedirectToAction("LoginFailed");
        }

        // GET: Student/LoginFailed
        public ActionResult LoginFailed()
        {
            return View();
        }
        // GET: Tutor/LoginFailedAdmin
        public ActionResult LoginFailedAdmin()
        {
            return View();
        }
        //GET: Tutor/GetStudentsForTutor/5
        public ActionResult GetStudentsForTutor(int id)
        {
            if (Session.Count > 0)
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


                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("LoginFailed");
            }
        }
        //GET: Tutor/Details/5
        public ActionResult Details(int id)
        {
            if (Session["StudentID"] != null || Session["isAdmin"].ToString() == "true")
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
            else
            {
                return RedirectToAction("LoginFailed");
            }
        }
        // POST: Tutor/CheckLogin
        [HttpPost]
        public bool CheckLogin(Tutor tutor)
        {
            string url = "TutorData/CheckLogin";

            string jsonpayload = jss.Serialize(tutor);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            var response = client.PostAsync(url, content).Result;
            IEnumerable<Tutor> loggedInTutor = response.Content.ReadAsAsync<IEnumerable<Tutor>>().Result;

            if (loggedInTutor.Count() > 0)
            {
                Tutor TutorLoggedIn = loggedInTutor.SingleOrDefault();
                Session["EmailID"] = TutorLoggedIn.EmailID;
                Session["StudentName"] = TutorLoggedIn.TutorFirstName;
                Session["isAdmin"] = "false";
                Session["TutorID"] = TutorLoggedIn.TutorID;
                return true;
            }
            else
            {
                return false;
            }
        }
        // POST: Tutor/CheckAdminLogin
        [HttpPost]
        public bool CheckAdminLogin(Admin admin)
        {
            string url = "TutorData/CheckAdminLogin";

            string jsonpayload = jss.Serialize(admin);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            var response = client.PostAsync(url, content).Result;
            IEnumerable<Admin> loggedInAdmin = response.Content.ReadAsAsync<IEnumerable<Admin>>().Result;

            if (loggedInAdmin.Count() > 0)
            {
                Admin AdminLoggedIn = loggedInAdmin.SingleOrDefault();
                Session["AdminEmailID"] = AdminLoggedIn.AdminEmailID;
                Session["AdminName"] = AdminLoggedIn.AdminFirstName;
                Session["isAdmin"] = "true";
                Session["AdminID"] = AdminLoggedIn.AdminId;
                return true;
            }
            else
            {
                return false;
            }
        }
        //GET: Tutor/DetailsLanguage/5
        public ActionResult DetailsLanguage(int id)
        {
            if (Session.Count > 0)
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
            else
            {
                return RedirectToAction("LoginFailed");
            }
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
        //POST: Tutor/UnassociateLanguage/{Tutorid}
        [HttpGet]
        public ActionResult UnassociateLanguage(int id, int LanguageId)
        {
            string url = "Tutor/UnAssociateLanguageFromTutor/" + id + "/" + LanguageId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("DetailsLanguage/" + id);
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

        // GET: Student/LoginTutor
        public ActionResult LoginTutor()
        {
            if (Session.Count > 0)
                Session.RemoveAll();
            return View();
        }
        // GET: Tutor/LoginAdmin
        public ActionResult LoginAdmin()
        {
            if (Session.Count > 0)
                Session.RemoveAll();
            return View();
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
