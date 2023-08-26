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
    public class StudentController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();
        static StudentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44309/api/");
        }
        // GET: Student/List
        public ActionResult List()
        {
            string url = "StudentData/ListStudents";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StudentDto> Student = response.Content.ReadAsAsync<IEnumerable<StudentDto>>().Result;


            return View(Student);
        }
        // GET: Student/LoginStudent
        public ActionResult LoginStudent()
        {
            return View();
        }
        // GET: Student/LoginFailed
        public ActionResult LoginFailed()
        {
            return View();
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            StudentDetails ViewModel = new StudentDetails();

            string url = "StudentData/FindStudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StudentDto Student = response.Content.ReadAsAsync<StudentDto>().Result;
            ViewModel.Student = Student;

            url = "Tutordata/ListTutorsForStudent/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TutorDTO> AssociatedTutorsOfStudent = response.Content.ReadAsAsync<IEnumerable<TutorDTO>>().Result;
            ViewModel.AssociatedTutors = AssociatedTutorsOfStudent;

            return View(ViewModel);
        }

        // GET: Student/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(Student Student)
        {
            string url = "StudentData/addStudent";

            string jsonpayload = jss.Serialize(Student);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("LoginStudent");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Studentdata/findStudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudentDto Student = response.Content.ReadAsAsync<StudentDto>().Result;
            return View(Student);
        }

        // POST: Student/Update/5
        [HttpPost]
        public ActionResult Update(int id, Student Student)
        {
            string url = "Studentdata/updateStudent/" + id;
            string jsonpayload = jss.Serialize(Student);
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

        // GET: Student/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "StudentData/findStudent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StudentDto Student = response.Content.ReadAsAsync<StudentDto>().Result;


            return View(Student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Student Student)
        {
            string url = "StudentData/deleteStudent/" + id;
            string jsonpayload = jss.Serialize(Student);

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
