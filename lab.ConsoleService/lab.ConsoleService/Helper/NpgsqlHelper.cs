using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using lab.ConsoleService.Model;
using Npgsql;

namespace lab.ConsoleService.Helper
{
    public static class NpgsqlHelper
    {
        public static void GetAll()
        {

            try
            {
                // Specify connection options and open an connection
                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
                {
                    conn.Open();

                    // Define a query
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT emp_id, emp_name, emp_emailaddress FROM public.employee", conn);

                    // Execute a query
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    //string sql = "SELECT * FROM employee";
                    //// data adapter making request from our connection
                    //NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);

                    // Read all rows and output the first column in each row
                    while (dr.Read())
                        Console.Write("{0}\n", dr[0]);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }


        public static List<Employee> GetEmployeeList()
        {
            try
            {
                IDbConnection db = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                //IDbConnection db = new NpgsqlConnection(String.Format("Server=localhost;Port=5432;User Id=postgres;Password=sa123456;Database=test_db;"));

                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
                {
                    conn.Open();

                    var employeeList = conn.Query<Employee>("SELECT emp_id, emp_name, emp_emailaddress FROM public.employee").ToList();

                    return employeeList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Employee GetEmployee(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                var employee = conn.Query<Employee>("SELECT emp_id, emp_name, emp_emailaddress FROM public.employee WHERE emp_id = @emp_id", new { id }).SingleOrDefault();

                return employee;
            }
        }

        public static Employee InsertEmployee(Employee employee)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                const string insertQuery = "INSERT INTO public.employee(emp_id, emp_name, emp_emailaddress) VALUES(@emp_id, @emp_name, @emp_emailaddress)";

                var employeeId = conn.Query<int>(insertQuery, employee).SingleOrDefault();

                employee.emp_id = employeeId;

                return employee;
            }

        }

        public static Employee UpdateEmployee(Employee employee)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                const string updateSqlQuery = "UPDATE public.employee SET emp_name = @emp_name, emp_emailaddress = @emp_emailaddress WHERE emp_id = @emp_id";
                conn.Execute(updateSqlQuery, employee);

                return employee;
            }

        }

        public static void DeleteEmployee(Employee employee)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString.ToString()))
            {
                conn.Open();

                const string deleteSqlQuery = "DELETE FROM public.employee WHERE emp_id = @emp_id";

                conn.Execute(deleteSqlQuery, employee);
            }
        }
    }
}
