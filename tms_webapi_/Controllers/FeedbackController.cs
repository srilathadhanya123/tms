using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace tms_webapi_.Controllers
{
  
    public class FeedbackController : ApiController
    {
        training_management_systemEntities dc = new training_management_systemEntities();

        public training_enrolled Post(training_enrolled e)
        {


            // write logic to add some thing to database

            dc.training_enrolled.Add(e);
            int i = dc.SaveChanges();
            if (i > 0)
            {
                return e;
            }
            else
            {
                return null;
            }


        }
    }


    }

