using lab.ngdemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Models.CacheManagement
{
    public class EmployeeCacheHelper
    {
        public CacheManager Cache { get; set; }

        public EmployeeCacheHelper()
        {
            Cache = new CacheManager();
        }
        public List<Employee> GetEmployees
        {
            get
            {
                List<Employee> _employeeList = new List<Employee>
                            {
                                new Employee {Id=1, Name = "Rasel", EmailAddress = "rasel@mail.com", Mobile = "01911-555555"},
                                new Employee {Id=2, Name = "Sohel", EmailAddress = "sohel@mail.com", Mobile = "01911-666666"},
                                new Employee {Id=3, Name = "Safin", EmailAddress = "safin@mail.com", Mobile = "01911-777777"},
                                new Employee {Id=4, Name = "Mim", EmailAddress = "mim@mail.com", Mobile = "01911-888888"},
                                new Employee {Id=5, Name = "Bappi", EmailAddress = "bappi@mail.com", Mobile = "01911-999999"}
                            };
                string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
                string cacheKey = Constants.CacheKey.EmployeeList + appConstant;
                if (!CacheManager.ICache.IsSet(cacheKey))
                {
                    CacheManager.ICache.Set(cacheKey, _employeeList);
                }
                else
                {
                    _employeeList = CacheManager.ICache.Get(cacheKey) as List<Employee>;
                }

                return _employeeList;
            }
        }

        public IQueryable<Employee> GetCustomGridEmployees
        {
            get
            {
                List<Employee> _employeeList = new List<Employee>
                            {
                                new Employee {Id=1, Name = "Rasel", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Employee {Id=2, Name = "Sohel", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Employee {Id=3, Name = "Safin", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Employee {Id=4, Name = "Mim", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Employee {Id=5, Name = "Bappi", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20},

                                new Employee {Id=6, Name = "Rasel 2", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Employee {Id=7, Name = "Sohel 2", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Employee {Id=8, Name = "Safin 2", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Employee {Id=9, Name = "Mim 2", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Employee {Id=10, Name = "Bappi 2", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20},

                                new Employee {Id=11, Name = "Rasel 3", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Employee {Id=12, Name = "Sohel 3", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Employee {Id=13, Name = "Safin 3", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Employee {Id=14, Name = "Mim 3", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Employee {Id=15, Name = "Bappi 3", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20},

                                new Employee {Id=16, Name = "Rasel 4", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Employee {Id=17, Name = "Sohel 4", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Employee {Id=18, Name = "Safin 4", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Employee {Id=19, Name = "Mim 4", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Employee {Id=20, Name = "Bappi 4", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20}
                            };
                string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
                string cacheKey = Constants.CacheKey.StudentList + appConstant;
                if (!CacheManager.ICache.IsSet(cacheKey))
                {
                    CacheManager.ICache.Set(cacheKey, _employeeList);
                }
                else
                {
                    _employeeList = CacheManager.ICache.Get(cacheKey) as List<Employee>;
                }

                return _employeeList.AsQueryable();
            }
        }

        public IQueryable<Employee> GetEmployeesBySearch(DataTableParamModel param)
        {
            var list = GetCustomGridEmployees;
            var resultList = list.Skip(param.iDisplayStart).Take(param.iDisplayLength).AsQueryable();
            return resultList;
        }

        public Employee GetEmployee(int id)
        {
            var employee = new Employee();
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            string cacheKey = Constants.CacheKey.Employee + appConstant;
            if (!CacheManager.ICache.IsSet(cacheKey))
            {
                employee = GetEmployees.FirstOrDefault(item => item.Id == id);
                CacheManager.ICache.Set(cacheKey, employee);
            }
            else
            {
                employee = CacheManager.ICache.Get(cacheKey) as Employee;
            }
            return employee;
        }

        public void AddEmployee(Employee employee)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.EmployeeAdd + appConstant;

            List<Employee> _employeeList = new List<Employee>();
            _employeeList = GetEmployees.ToList();
            _employeeList.Add(employee);

            string cacheKeyList = Constants.CacheKey.EmployeeList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _employeeList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _employeeList);
            }

        }

        public void EditEmployee(Employee employee)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.EmployeeEdit + appConstant;

            List<Employee> _employeeList = new List<Employee>();
            _employeeList = GetEmployees.ToList();
            var editEmployee = _employeeList.FirstOrDefault(item => item.Id == employee.Id);
            editEmployee = employee;

            string cacheKeyList = Constants.CacheKey.EmployeeList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _employeeList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _employeeList);
            }

        }

        public void DeleteEmployee(Employee employee)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.EmployeeDelete + appConstant;

            List<Employee> _employeeList = new List<Employee>();
            _employeeList = GetEmployees.ToList();
            var employeeRemove = _employeeList.FirstOrDefault(item => item.Id == employee.Id);
            _employeeList.Remove(employeeRemove);

            string cacheKeyList = Constants.CacheKey.EmployeeList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _employeeList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _employeeList);
            }

        }
    }
}