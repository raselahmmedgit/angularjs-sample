using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab.ConsoleService.Helper;

namespace lab.ConsoleService
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Schedule Execute OnStart() 1: " + DateTime.Now.ToString("F"));

                BootStrapper.Run();

                Console.WriteLine("Schedule Execute OnStart() 2: " + DateTime.Now.ToString("F"));

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ExceptionHelper.Manage(ex, true);
            }
        }
    }
}
