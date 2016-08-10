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