using System.Data;
namespace tms_lib
{
    public class Class1
    {
        Training_Management_SystemContext data = new Training_Management_SystemContext();

        //Login hr * manager * employee database logic
        public int login(string email, string password, string role)
        {
            var c = 0;

            if (role == "hr")
            {
               c = data.Hrs.Where(c => c.EmailId == email && c.Password == password).Count() ;

                
            }
            else if (role == "manager")
            {
                c = data.Managers.Where(c => c.EmailId == email && c.Password == password).Count() ;
            }
            else if (role == "employee")
            {
               c = data.Employees.Where(c => c.EmailId == email && c.Password == password).Count();
            }
            return c;
        }

        // Logic to display all Employee (for HR page)
        public IEnumerable<Employee> DisplayEmployee()
        {
            var res = data.Employees.ToList();

            return res;
        }



      

        public int isEmployeeEmailUnique(Employee e)
        {
            object email = e.EmailId;
            int isExists = data.Employees.Where(c => c.EmailId == email).Count();
            return isExists;
        }


        // logic to display count of employee
        public int countOfEmployee()
        {
            var res = data.Employees.Count();

            return res;
        }

        // Logic to add new Employee (for HR Page)
        public int add_employee_data(Employee e)
        {
            data.Employees.Add(e);

            int isEmployeeAdded = data.SaveChanges();

            return isEmployeeAdded;

        }


        // Logic to add new Manager (for HR Page)
        public int add_Manager_data(Manager m)
        {
            object email = m.EmailId;

            data.Managers.Add(m);

            int isEmployeeAdded = data.SaveChanges();

            return isEmployeeAdded;

        }

        // Logic to find is the manager eamil is exists 
        public int isManagerEmailUnique(Manager m)
        {
            object email = m.EmailId;
            int isExists = data.Managers.Where(c => c.EmailId == email).Count();
            return isExists;
        }

        //Logic to display all Mangaer (for HR page)
        public IEnumerable<Manager> DisplayManager()
        {
            var displayManagers = data.Managers;
            return displayManagers;
        }

        //Logic to display the count of manager
        public int countOfManager()
        {
            var countOfManagers = data.Managers.Count();
            return countOfManagers;
        }


        // Logic to display all Manager ID (for show in HR add employee page)

        public object DisplayManagersId()
        {
            var allManagerId = (from m in data.Managers
                                select m.Id);
            return allManagerId;
        }

        // Logic to display all Hr ID (for show in HR add employee page)

        public object DisplayHrId()
        {
            var allHrId = (from h in data.Hrs
                           select h.Id);
            return allHrId;
        }

        // Logic to dispaly all Courses ( for HR and Employee)
        public IEnumerable<Training> DisplayCourse()
        {
            var res = data.Training.ToList();
            return res;
        }
        //Logic to display the count of courses
        public int countOfCourse()
        {
            var countOfCourses = data.Training.Count();
            return countOfCourses;
        }
        // Logic to display all Enrolled Courses
        public IEnumerable<TrainingEnrolled> DisplayEnrolledCourses()
        {
            var res = data.TrainingEnrolleds.ToList();

            return res;
        }

        // Logic to display all Training Request
        public List<TrainingRequest> DisplayTrainingRequest()
        {
            var res = data.TrainingRequests.ToList();
            return res;
        }

        //Logi ta display all Hr ( for viewprofile )
        public List<Hr> DisplayHr()
        {
            var res = data.Hrs.ToList();
            return res;

        }


        // Logic to add new courses ( for HR Page)
        public int add_courses_data(Training c)
        {
            data.Training.Add(c);
            int isTrainingAdded = data.SaveChanges();
            return isTrainingAdded;
        }


        // Apply to new courses (for employee)
        public int applyCourse(string email, int trainingId, DateTime startDate, DateTime endDate)
        {


            if (trainingId != 0)
            {

                TrainingRequest tr = new TrainingRequest();

                tr.TrainingId = trainingId;

                tr.EmployeeEmail = email;

                /*var dateCheck = from d in data.TrainingRequests
                                join t in data.Training
                                on d.TrainingId equals t.Id
                                group*/



                /*var datecClash = (from t in data.Training
                             join trd in data.TrainingRequests on t.Id equals trd.TrainingId
                             where trd.EmployeeEmail == email  && ((t.Startdate >= startDate && t.Startdate <= endDate) || (t.Enddate <= endDate && t.Enddate >= endDate))
                             select t).Count();*/



                var date = data.Training.Join(data.TrainingRequests, t => t.Id, tr => tr.TrainingId, (t, tr) => new { t, tr }).Where(e => e.tr.EmployeeEmail == email && e.tr.EmployeeEmail == email && e.tr.ApprovalStatus == false && (e.t.Startdate >= startDate && e.t.Enddate <= startDate)).Count();

                //var date = data.Training.Join(data.TrainingRequests on t)
                int isExists = data.TrainingRequests.ToList().Where(c => c.EmployeeEmail == email && c.TrainingId == trainingId).Count();
                if (isExists > 0)

                {
                    return 0;

                }
                else if (date > 0)
                {
                    return 4;
                }
                else
                {
                    data.TrainingRequests.Add(tr);
                    int isRequested = data.SaveChanges();
                    return isRequested;
                }
            }
            else
            {
                return 0;
            }
            return 0;
        }
        // Add feedback (for employee)
        public int addFeedback(string feedbackInput, int id, int Userid)
        {
            var primary_key = (from t in data.TrainingEnrolleds
                               where t.TrainingId == id
                               && t.EmployeeId == Userid
                               select t.Id).FirstOrDefault();
            var a = data.TrainingEnrolleds.First(g => g.Id == primary_key);
            a.Feedback = feedbackInput;
            data.TrainingEnrolleds.Update(a);
            int isFeedbackUpdated = data.SaveChanges();
            return isFeedbackUpdated;

        }

        // To approval the courses (for manager)
        public int approval_status_change(int TrainingId, int EmpId, string email)
        {

            //var a = data.TrainingRequests.Find(c );
            //DataRow foundRow = data.TrainingRequests.Find(Training : 1);
            /*var mytab = db.Customers.First(g => g.CustomerId == mymodel.CustomerId);
            mytab.FirstName = mymodel.FirstName;
            mytab.LastName = mymodel.LastName;
            db.SaveChanges();*/

            var primary_key = (from t in data.TrainingRequests
                               where t.EmployeeEmail == email
                               && t.TrainingId == TrainingId
                               select t.Id).FirstOrDefault();
            var a = data.TrainingRequests.First(g => g.Id == primary_key);
            a.ApprovalStatus = true;
            data.TrainingRequests.Update(a);
            int isTrainingRequestUpdated = data.SaveChanges();


            TrainingEnrolled te = new TrainingEnrolled();
            te.EmployeeId = EmpId;
            te.TrainingId = TrainingId;
            data.TrainingEnrolleds.Add(te);
            int isApp = data.SaveChanges();
            return isApp;

        }

        public int deny_status_change(int TrainingId, string EmailId, string denyReason)
        {
            var primary_key = (from t in data.TrainingRequests
                               where t.EmployeeEmail == EmailId
                               && t.TrainingId == TrainingId
                               select t.Id).FirstOrDefault();
            var a = data.TrainingRequests.First(g => g.Id == primary_key);
            a.ApprovalStatus = false;
            a.RejectionReason = denyReason;
            data.TrainingRequests.Update(a);
            int isTrainingRequestedUpdated = data.SaveChanges();
            return isTrainingRequestedUpdated;
        }


        public object getId(string role, string email)
        {
            object id = new object();
            if (role == "hr")
            {
                id = (from h in data.Hrs
                      where h.EmailId == email
                      select h.Id).FirstOrDefault();


            }
            else if (role == "manager")
            {
                id = (from m in data.Managers
                      where m.EmailId == email
                      select m.Id).FirstOrDefault();


            }
            else if (role == "employee")
            {
                id = (from e in data.Employees
                      where e.EmailId == email
                      select e.Id).FirstOrDefault();

            }
            return id;

        }
    }
}

