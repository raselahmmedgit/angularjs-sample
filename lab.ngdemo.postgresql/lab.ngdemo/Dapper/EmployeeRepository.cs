using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using lab.ngdemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace lab.ngdemo.Dapper
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public List<Employee> GetAll()
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString.ToString()))
            {
                conn.Open();

                var employeeList = conn.Query<Employee>("SELECT emp_id, emp_name, emp_emailaddress FROM public.employee").ToList();

                return employeeList;
            }
        }

        public Employee Find(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                var employee = conn.Query<Employee>("SELECT emp_id, emp_name, emp_emailaddress FROM public.employee WHERE emp_id = @emp_id", new { id }).SingleOrDefault();

                return employee;
            }
        }

        public Employee Insert(Employee employee)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                const string sqlQuery = "INSERT INTO public.employee(emp_id, emp_name, emp_emailaddress) VALUES(@emp_id, @emp_name, @emp_emailaddress) RETURNING emp_id";

                var employeeId = conn.Query<int>(sqlQuery, employee).SingleOrDefault();

                employee.emp_id = employeeId;

                return employee;
            }
        }

        public Employee Update(Employee employee)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                const string sqlQuery = "UPDATE public.employee SET emp_name = @emp_name, emp_emailaddress = @emp_emailaddress WHERE emp_id = @emp_id";
                conn.Execute(sqlQuery, employee);

                return employee;
            }
        }

        public void Delete(Employee employee)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                const string sqlQuery = "DELETE FROM public.employee WHERE emp_id = @emp_id";

                conn.Execute(sqlQuery, employee);
            }
        }

        public Employee GetById(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString.ToString()))
            {
                conn.Open();

                var emp = Find(id);
                
                var employee = conn.Query("SELECT emp_id, emp_name, emp_emailaddress FROM public.employee WHERE emp_id = @emp_id", emp).SingleOrDefault();

                return employee;
            }
        }

        public List<Employee> GetFunction()
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                using (var multipleResults = conn.QueryMultiple("SELECT public.get_employees()", commandType: CommandType.StoredProcedure))
                {
                    var employees = multipleResults.Read<Employee>().ToList();

                    return employees;
                }
            }

        }
    }

    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee Find(int id);
        Employee Insert(Employee emp);
        Employee Update(Employee emp);
        void Delete(Employee employee);
        Employee GetById(int id);
        List<Employee> GetFunction();
    }
}