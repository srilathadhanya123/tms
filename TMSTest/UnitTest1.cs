using NUnit.Framework;
using tms_lib;

namespace TMSTest
{
    public class Tests
    {

        Class1 ob = new Class1();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Loginhr()
        {

            int hr = ob.login("pranav@gmail.com", "pranav","hr");
            int manager = ob.login("anusha@gmail.com", "anusha", "manager");
            int employee = ob.login("srilatha@gmail.com.com", "srilatha", "employee");
            Assert.AreEqual(1, hr);
            Assert.AreEqual(1, manager);
            Assert.AreEqual(1,hr);
        }


        [Test]
        public void displaycourse()
        {

            var item = ob.DisplayCourse();
            Assert.AreEqual(8,8);


            //var item = ob.displaydatabyname("idly");

            //Assert.AreEqual(1, item.Count);

        }



        [Test]
        public void displayemployee()
        {

            var item = ob.DisplayEmployee();
            Assert.AreEqual(7, 7);



        }



        [Test]
        public void displaymanager()
        {

            var item = ob.DisplayManager();
            Assert.AreEqual(6, 6);



        }


        [Test]
        public void displayhr()
        {

            var item = ob.DisplayHr();
            Assert.AreEqual(6, item.Count);



        }





    }
}



