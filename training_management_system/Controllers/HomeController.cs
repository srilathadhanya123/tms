using Microsoft.AspNetCore.Mvc;
using tms_lib;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;
using MimeKit.Text;

namespace _training_management_system.Controllers
{

    public class HomeController : Controller
    {
        Class1 ob = new Class1();
       
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        // Login and store the eamil and role in  session

        [HttpPost]
        public ActionResult login(string email, string password, string role)
        {

            var c = ob.login(email, password, role);

            if (c>0)
            {
                HttpContext.Session.SetString("login_eamil", email);
                HttpContext.Session.SetString("login_role", role);
                return RedirectToAction("home");
            }
            else
            {
                ViewData["Message"] = "Invaild user";
            }

            return View();
            
        }


        public ActionResult home()
        {
            var countOfEmp = ob.countOfEmployee();
            ViewBag.countOfEmp = countOfEmp;

            var countOfManager = ob.countOfManager();
            ViewBag.countOfManager = countOfManager;

            var countOfCourse = ob.countOfCourse();
            ViewBag.countOfCourses = countOfCourse;


            //HttpContext.Session.SetString("a", "h1123");                              
            //TempData["data"] = TempData["login_id"];
            return View();
        }

        // To disolay all courses 
        public ViewResult courses()
        {
            var displayCourses = ob.DisplayCourse();
            //HttpContext.Session.SetString("a", "h1123");
            return View(displayCourses);
        }

        // To add new courses 
        [HttpGet]
        public ActionResult add_new_courses()
        {
            return View();
        }

        [HttpPost]
        public ActionResult add_new_courses(Training c)
        {
            var i = ob.add_courses_data(c);
            if (i > 0)
            {
                ViewData["Message"] = "Course is added successfully";
            }
            else
            {
                ViewData["Message"] = "Course addition is not successfull";
            }
            return View();
        }

        // To Apply (employee page)
        public ViewResult applyCourses(int id, DateTime startDate, DateTime endDate)
        {
            var isRequested = ob.applyCourse(HttpContext.Session.GetString("login_eamil"), id, startDate, endDate);
            var result = ob.DisplayCourse();
            ViewBag.isRequested = isRequested;
            return View("courses", result);
        }

        // To display all employee (Hr page)
        public ViewResult employee()
        {
            var displayEmployee = ob.DisplayEmployee();
            //HttpContext.Session.SetString("a", "h1123");
            return View(displayEmployee);
        }






        // To add new employee

        [HttpGet]
        public ActionResult add_new_employee()
        {
            if (ModelState.IsValid)
            {
                var allManagerId = ob.DisplayManagersId();
                ViewBag.manId = allManagerId;
                return View();
            }
            return View();
        }


        [HttpPost]
        public ActionResult add_new_employee(Employee e)
        {

            var allManagerId = ob.DisplayManagersId();
            ViewBag.manId = allManagerId;
            var isEmailUniq = ob.isEmployeeEmailUnique(e);
            if (isEmailUniq > 0)
            {
                ViewBag.isEmailExistError = "Email already exists";
            }
            else if (ModelState.IsValid)
            {

                var isEmployeeAdded = ob.add_employee_data(e);
                if (isEmployeeAdded > 0)
                {
                    ViewData["Message"] = "Insertion  successfull";

                }
                else
                {
                    ViewData["Message"] = "Insertion not successfull";
                }
                return View();
            }
            return View();
        }

        // To display all manager (hr page)
        public ViewResult manager()
        {

            var displayManager = ob.DisplayManager();
            //HttpContext.Session.SetString("a", "h1123");
            return View(displayManager);
        }

        //To add new manager
        
        [HttpGet]
        
        public ActionResult add_new_manager()
        {
           
                var allHrId = ob.DisplayHrId();
            ViewBag.hrId = allHrId;

            return View();
        }
        [HttpPost]
        public ActionResult add_new_manager(Manager m)
        {
            var allHrId = ob.DisplayHrId();
            ViewBag.hrId = allHrId;
            var isEmailUnique = ob.isManagerEmailUnique(m);
            if (isEmailUnique > 0)
            {
                ViewBag.isEmailExistError = "Email already exists";
            }
            else
            {
                var isHrAdded = ob.add_Manager_data(m);
                if (isHrAdded > 0)
                {
                    var email = new MimeMessage();
                    ViewData["Message"] = "Insertion  successfull";

                }
                else
                {
                    ViewData["Message"] = "Insertion not successfull";
                }
            }
            return View();
        }

        // To display all enrolled courses (employee page)
        [Route("home/mycourses")]
        public ViewResult mycourses()
        {
            //HttpContext.Session.SetString("a", "h1123");
            var training_enrolled = ob.DisplayEnrolledCourses();

            var emp = ob.DisplayEmployee();
            var course = ob.DisplayCourse();
            var id = ob.getId(HttpContext.Session.GetString("login_role"), HttpContext.Session.GetString("login_eamil"));


            var enrolledCourses = (from e in emp
                                   join te in training_enrolled on e.Id equals te.EmployeeId
                                   join c in course on te.TrainingId equals c.Id
                                   where (te.EmployeeId == Convert.ToInt32(id))
                                   select new
                                   { c.Id, c.Title, c.Startdate, c.Enddate, c.CourseDescription, te.Feedback }).ToList();

            return View(enrolledCourses);
        }

        [HttpGet]
        public ViewResult addFeedback(int id)
        {
            var course = ob.DisplayCourse();
            var feedback = from c in course
                           where c.Id == id
                           select c;

            return View(feedback);

        }
        [HttpPost]
        public ViewResult addFeedback(string feedbackInput, int id)
        {
            var Userid = ob.getId(HttpContext.Session.GetString("login_role"), HttpContext.Session.GetString("login_eamil"));
            var isFeedbackUpdated = ob.addFeedback(feedbackInput, id, Convert.ToInt32(Userid));
            ViewBag.isFeedbackUpdated = isFeedbackUpdated;
            return View();
        }

        public ViewResult myemployee()
        {
            //HttpContext.Session.SetString("a", "h1123");
            var result = ob.DisplayManager();
            var result1 = ob.DisplayEmployee();
            object id = ob.getId(HttpContext.Session.GetString("login_role"), HttpContext.Session.GetString("login_eamil"));
            ViewBag.id = id;



            var manager = from t in result    //manager
                          join t1 in result1  //employee
                          on t.Id equals t1.ManagerId
                          where (t.EmailId == HttpContext.Session.GetString("login_eamil"))

                          select t1;


            return View(manager);

        }

        public ViewResult courses_req()
        {

            var trainingReq = ob.DisplayTrainingRequest();
            var managerDisp = ob.DisplayManager();
            var emp = ob.DisplayEmployee();
            var course = ob.DisplayCourse();
            var id = ob.getId(HttpContext.Session.GetString("login_role"), HttpContext.Session.GetString("login_eamil"));

            var a = (from man in managerDisp
                     join e in emp on man.Id equals e.ManagerId
                     join tr in trainingReq on e.EmailId equals tr.EmployeeEmail
                     join c in course on tr.TrainingId equals c.Id
                     where (e.ManagerId == Convert.ToInt32(id) && tr.ApprovalStatus is null)
                     select new { c.Title, c.Startdate, c.Enddate, e.Name, e.Id, e.EmailId, tr.TrainingId, tr.ApprovalStatus }).ToList();
            //var email = new MimeMessage();
            //email.Sender = MailboxAddress.Parse("trainingmanagement02@gmail.com");
            //email.To.Add(MailboxAddress.Parse("saichaitanya0488@gmail.com"));
            //email.Subject = "Request for course approval";
            //email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Enter the content</h1>" };

            //// send email
            //using var smtp = new SmtpClient();
            //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("trainingmanagement02", "trainingmanagement");
            //smtp.Send(email);
            //smtp.Disconnect(true);

            return View(a);
        }

        public ViewResult request_approval(int TrainingId, int EmpId, string EmailId)
        {
            var isdone = ob.approval_status_change(TrainingId, EmpId, EmailId);
            ViewBag.isApproved = isdone;


            var trainingReq = ob.DisplayTrainingRequest();
            var managerDisp = ob.DisplayManager();
            var emp = ob.DisplayEmployee();
            var course = ob.DisplayCourse();
            var id = ob.getId(HttpContext.Session.GetString("login_role"), HttpContext.Session.GetString("login_eamil"));

            var a = (from man in managerDisp
                     join e in emp on man.Id equals e.ManagerId
                     join tr in trainingReq on e.EmailId equals tr.EmployeeEmail
                     join c in course on tr.TrainingId equals c.Id
                     where (e.ManagerId == Convert.ToInt32(id) && tr.ApprovalStatus is null)
                     select new { c.Title, c.Startdate, c.Enddate, e.Name, e.Id, e.EmailId, tr.TrainingId, tr.ApprovalStatus }).ToList();


            return View("courses_req", a);

        }


        [HttpGet]
        public ViewResult deny(int TrainingId)
        {
            var course = ob.DisplayCourse();
            var requestDeny = from c in course
                              where c.Id == TrainingId
                              select c;

            return View(requestDeny);

        }
        [HttpPost]
        public ViewResult deny(int TrainingId, string EmailId, string denyReason)
        {
            var isDenied = ob.deny_status_change(TrainingId, EmailId, denyReason);
            ViewBag.isDenyReasonUpdated = isDenied;
            return View("deny");



        }

        public ActionResult ViewProfile()
        {

            if (HttpContext.Session.GetString("login_role") == "employee")
            {
                return RedirectToAction("ProfileEmployee", new { e = HttpContext.Session.GetString("login_eamil") });
            }
            else if (HttpContext.Session.GetString("login_role") == "hr")
            {

                return RedirectToAction("ProfileHr", new { h = HttpContext.Session.GetString("login_eamil") });


            }
            else if (HttpContext.Session.GetString("login_role") == "manager")
            {



                return RedirectToAction("ProfileManager", new { m = HttpContext.Session.GetString("login_eamil") });

                //select * from manager where manager.email_id = 'man@gmail.com';

            }
            return View();
        }

        public ActionResult profileEmployee(string e)
        {
            var res = ob.DisplayEmployee().ToList().Where(c => c.EmailId == e).ToList();
            return View(res);
        }
        public ActionResult profileHr(string h)
        {
            var res = ob.DisplayHr().ToList().Where(c => c.EmailId == h).ToList();
            return View(res);
        }
        public ActionResult profileManager(string m)
        {
            var res = ob.DisplayManager().ToList().Where(c => c.EmailId == m).ToList();
            return View(res);
        }


        public ActionResult logout()
        {
            //HttpContext.Session.SetString("a", "h1123");
            HttpContext.Session.Remove("login_eamil");
            HttpContext.Session.Remove("login_role");
            HttpContext.Session.Remove("firstLName");

            return RedirectToAction("login");
            //HttpContext.Session.SetString("a", "h1123");
        }

        //[HttpGet]
        //public IActionResult sendMail(string emailId)
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult sendMail(string emailId, string subject, string body)
        //{

        //    var email = new MimeMessage();
        //    email.Sender = MailboxAddress.Parse("trainingmanagement02@gmail.com");
        //    email.To.Add(MailboxAddress.Parse(emailId));
        //    email.Subject = subject;
        //    email.Body = new TextPart(TextFormat.Html) { Text = body };

        //    // send email
        //    using var smtp = new SmtpClient();
        //    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        //    smtp.Authenticate("trainingmanagement02", "trainingmanagement");
        //    smtp.Send(email);
        //    smtp.Disconnect(true);
        //    return View();
        //}

    }
}


    
      
    

