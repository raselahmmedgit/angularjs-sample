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

        [OutputCache(Duration = 0)]
        public ActionResult LineChartAjax()
        {
            {
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

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}