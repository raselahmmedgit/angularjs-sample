﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lab.ngdemo.DataTablesHelpers;
using lab.ngdemo.Models;
using lab.ngdemo.Models.CacheManagement;

namespace lab.ngdemo.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeCacheHelper _employeeCacheHelper = new EmployeeCacheHelper();

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListAjax()
        {
            var list = _employeeCacheHelper.GetEmployees;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataTableListAjax()
        {
            var list = _employeeCacheHelper.GetEmployees;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetDataTableListAjax(DataTablesModel dataTablesModel)
        //{
        //    int totalRecordCount;
        //    int searchRecordCount;
        //    var employeeList = InMemoryEmployeeRepository.GetDataTableList(startIndex: dataTablesModel.iDisplayStart,
        //        pageSize: dataTablesModel.iDisplayLength, sortedColumns: dataTablesModel.GetSortedColumns(),
        //        totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount, searchString: dataTablesModel.sSearch);

        //    return Json(new DataTablesResponse<Employee>(items: employeeList,
        //        totalRecords: totalRecordCount,
        //        totalDisplayRecords: searchRecordCount,
        //        sEcho: dataTablesModel.sEcho));
        //}

        [HttpGet]
        public ActionResult GetDataTableListAjax(DataTablesModel dataTablesModel)
        {
            var employeeList = _employeeCacheHelper.GetEmployees.ToList();
            int totalRecords = employeeList.Count();
            const int totalDisplayRecords = 2;
            if (!string.IsNullOrEmpty(dataTablesModel.sSearch))
            {
                employeeList = employeeList.Where(item => (item.Name ?? "").Contains(dataTablesModel.sSearch)).ToList();
            }

            var viewOdjects = employeeList.Skip(dataTablesModel.iDisplayStart).Take(dataTablesModel.iDisplayLength);

            var result = from employee in viewOdjects
                         select new IComparable[] { employee.Name, employee.EmailAddress, employee.Mobile, employee.Id };

            return Json(new
            {
                sEcho = dataTablesModel.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalDisplayRecords,
                aaData = result
            },
                            JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetByIdAjax(int id)
        {
            var employee = _employeeCacheHelper.GetEmployee(id);
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveAjax(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isExistEmployee = _employeeCacheHelper.GetEmployee(employee.Id);
                    if (isExistEmployee != null)
                    {
                        isExistEmployee.Name = employee.Name;
                        _employeeCacheHelper.EditEmployee(isExistEmployee);
                        employee.IsSuccess = true;
                        employee.SuccessMessage = Constants.Messages.UpdateSuccess;
                    }
                    else
                    {
                        _employeeCacheHelper.AddEmployee(employee);
                        employee.IsSuccess = true;
                        employee.SuccessMessage = Constants.Messages.SaveSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                employee.IsSuccess = false;
                employee.ErrorMessage = Constants.Messages.ExceptionError(ex);
            }

            return Json(employee, JsonRequestBehavior.DenyGet);

        }

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var employee = new Employee();
            try
            {
                employee = _employeeCacheHelper.GetEmployee(id);
                if (employee != null)
                {
                    _employeeCacheHelper.DeleteEmployee(employee);
                    employee.IsSuccess = true;
                    employee.SuccessMessage = Constants.Messages.DeleteSuccess;
                }
                else
                {
                    employee.IsSuccess = false;
                    employee.SuccessMessage = Constants.Messages.NotFound;
                }
            }
            catch (Exception ex)
            {
                employee.IsSuccess = false;
                employee.ErrorMessage = Constants.Messages.ExceptionError(ex);
            }

            return Json(employee, JsonRequestBehavior.DenyGet);

        }
    }
}