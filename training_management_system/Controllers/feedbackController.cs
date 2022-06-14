using Microsoft.AspNetCore.Mvc;
using tms_lib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace training_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class feedbackController : ControllerBase
    {
        Training_Management_SystemContext dc=new Training_Management_SystemContext();
        //Class1 dc=new Class1();
        // GET: api/<feedbackController>
        [HttpGet]
        public IEnumerable<Feedback> Get()
        {
            var res = dc.Feedbacks;
            return res;
        }
        // GET api/<feedbackController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<feedbackController>
        [HttpPost]
        public Feedback Post(Feedback s)
        {
            dc.Feedbacks.Add(s);
            int i = dc.SaveChanges();
            if (i > 0)
            {
                return s;
            }
            else
            {
                return null;
            }
        }

        // PUT api/<feedbackController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<feedbackController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
