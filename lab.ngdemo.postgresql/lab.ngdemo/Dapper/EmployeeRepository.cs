using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using lab.ngdemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Dapper
{
    public class EmployeeRepository : IEmployeeRepository
    {
        //private string strCon = String.Format("Server=localhost;Port=5432;User Id=postgres;Password=sa123456;Database=test_db;");
        private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString);
        //private IDbConnection _db = new SqlConnection(String.Format("Server=localhost;Port=5432;User Id=postgres;Password=sa123456;Database=test_db;"));
        public List<Employee> GetAll()
        {
            return this._db.Query<Employee>("SELECT * FROM employee").ToList();
        }

        public Employee Find(int id)
        {
            return this._db.Query<Employee>("SELECT * FROM employee WHERE emp_id = @emp_id", new { id }).SingleOrDefault();
        }

        public Employee Add(Employee emp)
        {
            var sqlQuery = "INSERT INTO employee (emp_name, emp_emailaddress) VALUES(@emp_name, @emp_emailaddress); " + "SELECT CAST(SCOPE_IDENTITY() as int)";
            var empId = this._db.Query<int>(sqlQuery, emp).Single();
            emp.emp_id = empId;
            return emp;
        }

        public Employee Update(Employee emp)
        {
            var sqlQuery =
                "UPDATE employee " +
                "SET emp_name = @emp_name, " +
                "    emp_emailaddress  = @emp_emailaddress, " +
                "WHERE emp_id = @emp_id";
            this._db.Execute(sqlQuery, emp);
            return emp;
        }

        public void Remove(int id)
        {
            var sqlQuery =
                "DELETE FROM employee " +
                "WHERE emp_id = @emp_id";
            this._db.Execute(sqlQuery);
        }

        public Employee GetById(int id)
        {
            using (var multipleResults = this._db.QueryMultiple("GetEmployeeByID", new { Id = id }, commandType: CommandType.StoredProcedure))
            {
                var emp = multipleResults.Read<Employee>().SingleOrDefault();
                
                return emp;
            }
        }
    }

    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee Find(int id);
        Employee Add(Employee emp);
        Employee Update(Employee emp);
        void Remove(int id);
        Employee GetById(int id);
    }
}