using lab.ngdemo.postgresql.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lab.ngdemo.postgresql.Models;

namespace lab.ngdemo.postgresql.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepository _iEmployeeRepository = new EmployeeRepository();

        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JQGrid()
        {
            return View();
        }

        // for display jqGrid
        public ActionResult GetEmployees()
        {
            var list = _iEmployeeRepository.GetAll().ToList();

            //No of total records
            int totalRecords = (int)list.Count;
            //Calculate total no of page  
            int totalPages = 1;   // (int)Math.Ceiling((float)totalRecords / (float)Rows);
            var getdata = new
            {
                total = totalPages,
                page = 1,
                records = totalRecords,
                rows = (
                    from p in list
                    select new
                    {
                        cell = new string[] { 
							p.emp_id.ToString(),
                            p.emp_id.ToString(),
                            p.emp_name,
							p.emp_emailaddress,
							"<a id='lnkDetailsEmployee_" + p.emp_id + "' class='lnkAppModal btn btn-default' href='/Employee/Details/" + p.emp_id + "'>Details</a>",
                            "<a id='lnkEditEmployee_" + p.emp_id + "' class='lnkAppModal btn btn-default' href='/Employee/Edit/" + p.emp_id + "'>Edit</a>",
							"<a id='lnkDeleteEmployee_" + p.emp_id + "' class='lnkAppDelete btn btn-default' href='/Employee/Delete/" + p.emp_id + "'>Delete</a>" }
                    }).ToArray()
            };
            return Json(getdata);
        }
        
        //
        // GET: /Employee/Details/By ID

        public PartialViewResult Details(int id)
        {
            Employee employee = _iEmployeeRepository.GetById(id);

            //return View(employee);
            return PartialView("_Details", employee);
            //return View("_Details", employee);
        }

        //
        // GET: /Employee/Create

        public PartialViewResult Create()
        {
            //return View();
            return PartialView("_Create");
            //return View("_Create");
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _iEmployeeRepository.Insert(employee);

                    //return RedirectToAction("Index");
                    //return Content(Boolean.TrueString);
                    return RedirectToAction("Index", "Employee");
                }

                //return View(employee);
                //return PartialView("_Create", employee);
                //return Content("Please review your form.");
                return View("_Create", employee);
            }
            catch (Exception ex)
            {
                //return Content("Error Occured!");
                //return RedirectToAction("Index", "Employee");
                return View("_Create", employee);
            }

        }

        //
        // GET: /Employee/Edit/By ID

        public PartialViewResult Edit(int id)
        {
            Employee employee = _iEmployeeRepository.GetById(id);

            //return View(employee);
            return PartialView("_Edit", employee);
            //return View("_Edit", employee);
        }

        //
        // POST: /Employee/Edit/By ID

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _iEmployeeRepository.Update(employee);

                    //return RedirectToAction("Index");
                    //return Content(Boolean.TrueString);
                    return RedirectToAction("Index", "Employee");
                }

                //return View(employee);
                //return PartialView("_Edit", employee);
                //return Content("Please review your form.");
                return View("_Edit", employee);
            }
            catch (Exception ex)
            {
                //return Content("Error Occured!");
                //return RedirectToAction("Index", "Employee");
                return View("_Edit", employee);
            }
        }

        //
        // GET: /Employee/Delete/By ID

        public PartialViewResult Delete(int id)
        {
            Employee employee = _iEmployeeRepository.GetById(id);

            //return View(employee);
            return PartialView("_Delete", employee);
            //return View("_Delete", employee);
        }

        //
        // POST: /Employee/Delete/By ID

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = _iEmployeeRepository.GetById(id);
            try
            {
                //Employee employee = _iEmployeeRepository.Employees.Find(id);
                if (employee != null)
                {
                    _iEmployeeRepository.Delete(employee);

                    //return RedirectToAction("Index");
                    //return Content(Boolean.TrueString);
                    return RedirectToAction("Index", "Employee");
                }

                //return Content("Please review your form.");
                return View("_Delete", employee);
            }
            catch (Exception ex)
            {
                //return Content("Error Occured!");
                return View("_Delete", employee);
            }
        }

    }
}