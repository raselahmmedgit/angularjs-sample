using lab.ngdemo.Models;
using lab.ngdemo.Models.CacheManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}