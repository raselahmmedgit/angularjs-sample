using lab.ngdemo.Helpers;
using lab.ngdemo.Models;
using lab.ngdemo.Models.CacheManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace lab.ngdemo.Controllers
{
    public class StudentController : Controller
    {
        private StudentCacheHelper _studentCacheHelper = new StudentCacheHelper();

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListAjax()
        {
            var list = _studentCacheHelper.GetStudents;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetByIdAjax(int id)
        {
            var student = _studentCacheHelper.GetStudent(id);
            return Json(student, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveAjax(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isExistStudent = _studentCacheHelper.GetStudent(student.Id);
                    if (isExistStudent != null)
                    {
                        isExistStudent.Name = student.Name;
                        _studentCacheHelper.EditStudent(isExistStudent);
                        student.IsSuccess = true;
                        student.SuccessMessage = Constants.Messages.UpdateSuccess;
                    }
                    else
                    {
                        _studentCacheHelper.AddStudent(student);
                        student.IsSuccess = true;
                        student.SuccessMessage = Constants.Messages.SaveSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                student.IsSuccess = false;
                student.ErrorMessage = Constants.Messages.ExceptionError(ex);
            }

            return Json(student, JsonRequestBehavior.DenyGet);

        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var student = new Student();
            try
            {
                student = _studentCacheHelper.GetStudent(id);
                if (student != null)
                {
                    _studentCacheHelper.DeleteStudent(student);
                    student.IsSuccess = true;
                    student.SuccessMessage = Constants.Messages.DeleteSuccess;
                }
                else
                {
                    student.IsSuccess = false;
                    student.SuccessMessage = Constants.Messages.NotFound;
                }
            }
            catch (Exception ex)
            {
                student.IsSuccess = false;
                student.ErrorMessage = Constants.Messages.ExceptionError(ex);
            }

            return Json(student, JsonRequestBehavior.DenyGet);

        }

        #region ng-grid

        // GET: Student
        public ActionResult Grid()
        {
            return View();
        }

        public ActionResult GetPage(pageParams p)
        {
            if (p.page == 0)
            {
                p.page = 1;
            }

            var students = new Func<IQueryable<Student>>(() =>
            {
                if (string.IsNullOrWhiteSpace(p.filterText))
                    return _studentCacheHelper.GetStudents;
                else
                    return _studentCacheHelper.GetStudents.Where(x => x.Name.Contains(p.filterText));
            })();

            if (string.IsNullOrWhiteSpace(p.sortColumn) || string.IsNullOrWhiteSpace(p.sortDirection))
            {
                students = students.OrderBy(x => x.Id);
            }
            else
            {
                //students = students.OrderBy(p.sortColumn + " " + p.sortDirection);
            }

            var count = students.Count();
            var pagedCustomers = students.Skip((p.page - 1) * p.pageSize).Take(p.pageSize).ToList();

            var pageResult = new pageResult<Student>
            {
                data = pagedCustomers,
                page = p.page,
                pageSize = p.pageSize,
                total = count,
                totalPages = (int)Math.Ceiling((decimal)count / p.pageSize)
            };

            return Content(JsonConvert.SerializeObject(pageResult), "application/json");
        }

        #endregion

        #region custom grid



        #endregion
    }
}