using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using lab.ngdemo.postgresql.Dapper;
using lab.ngdemo.postgresql.Models;
using lab.ngdemo.postgresql.ViewModels;

namespace lab.ngdemo.postgresql.Controllers
{
    public class HomeController : Controller
    {
        //AppDbContext _db = new AppDbContext();
        IEmployeeRepository _iEmployeeRepository = new EmployeeRepository();
        public ActionResult Index()
        {
            //var userList = _db.Users.ToList();
            var employeeList = _iEmployeeRepository.GetAll().ToList();
            return View();
        }

        public ActionResult HI()
        {
            return View();
        }

        public ActionResult TreeView()
        {
            return View();
        }

        [OutputCache(Duration = 0)]
        public ActionResult TreeViewAjax()
        {
            var data = new List<TreeViewViewModel>()
            {
                new TreeViewViewModel(){ Id = 1, Name = "A", Details = new List<TreeViewViewModel>()
                {
                    new TreeViewViewModel(){ Id = 1, Name = "A1", Details = new List<TreeViewViewModel>()
                                    {
                                    new TreeViewViewModel(){ Id=1, Name = "A11"},
                                    new TreeViewViewModel(){ Id=2, Name = "A22"}
                                    }
                        }
                }},
                new TreeViewViewModel(){ Id = 2, Name = "B", Details = new List<TreeViewViewModel>()
                {
                    new TreeViewViewModel(){ Id = 1, Name = "B1", Details = new List<TreeViewViewModel>()
                                    {
                                    new TreeViewViewModel(){ Id=1, Name = "B11"},
                                    new TreeViewViewModel(){ Id=2, Name = "B22"}
                                    }
                        }
                }}
            };

            var result = data.ToList();

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}