using System.Collections.Generic;
using System.Web.Mvc;

namespace lab.ngdemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LineChart()
        {
            return View();
        }

        //[OutputCache(Duration = 0)]
        //public ActionResult LineChartAjax()
        //{

        //    List<object> data = new List<object>();
        //    data.Add(new[] { "Day", "Kasse", "Bonds", "Stocks", "Futures", "Options" });
        //    data.Add(new[] { 01.03, 200, 500, 100, 0, 10 });
        //    data.Add(new[] { 01.03, 300, 450, 150, 50, 30 });
        //    data.Add(new[] { 12.15, 350, 200, 180, 80, 40 });
            
        //    /////
        //    //“cols”: [
        //    //{"id":"","label":"year","type":"string"},
        //    //{"id":"","label":"sales","type":"number"},
        //    //{"id":"","label":"expenses","type":"number"}
        //    //],
        //    //“rows”: [
        //    //{"c":[{"v":"2001"},{"v":3},{"v":5}]},
        //    //{“c”:[{"v":"2002"},{"v":5},{"v":10}]},
        //    //{“c”:[{"v":"2003"},{"v":6},{"v":4}]},
        //    //{“c”:[{"v":"2004"},{"v":8},{"v":32}]},
        //    //{“c”:[{"v":"2005"},{"v":3},{"v":56}]}
        //    //]
        //    //}
        //    /////

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        [OutputCache(Duration = 0)]
        public ActionResult LineChartAjax(int id, string name)
        {

            List<object> data = new List<object>();
            data.Add(new[] { "Day", "Kasse", "Bonds", "Stocks", "Futures", "Options" });
            data.Add(new[] { 01.03, 200, 500, 100, 0, 10 });
            data.Add(new[] { 01.03, 300, 450, 150, 50, 30 });
            data.Add(new[] { 12.15, 350, 200, 180, 80, 40 });

            /////
            //“cols”: [
            //{"id":"","label":"year","type":"string"},
            //{"id":"","label":"sales","type":"number"},
            //{"id":"","label":"expenses","type":"number"}
            //],
            //“rows”: [
            //{"c":[{"v":"2001"},{"v":3},{"v":5}]},
            //{“c”:[{"v":"2002"},{"v":5},{"v":10}]},
            //{“c”:[{"v":"2003"},{"v":6},{"v":4}]},
            //{“c”:[{"v":"2004"},{"v":8},{"v":32}]},
            //{“c”:[{"v":"2005"},{"v":3},{"v":56}]}
            //]
            //}
            /////

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}