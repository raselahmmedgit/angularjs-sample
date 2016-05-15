using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab.ConsoleService.Helper;
using lab.ConsoleService.Model;

namespace lab.ConsoleService
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                NpgsqlHelper.GetAll();

                NpgsqlHelper.GetEmployeeList();

                var employee = new Employee {emp_id = 3, emp_name = "Azim", emp_emailaddress = "azim@gmail.com"};

                //NpgsqlHelper.InsertEmployee(employee);
                //NpgsqlHelper.UpdateEmployee(employee);
                //NpgsqlHelper.DeleteEmployee(employee);

                //Console.WriteLine("Schedule Execute OnStart() 1: " + DateTime.Now.ToString("F"));

                //BootStrapper.Run();

                //Console.WriteLine("Schedule Execute OnStart() 2: " + DateTime.Now.ToString("F"));

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ExceptionHelper.Manage(ex, true);
            }
        }
    }
}
