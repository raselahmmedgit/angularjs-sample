using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using lab.ngdemo.Models;
using lab.ngdemo.Models.CacheManagement;

namespace lab.ngdemo.DataTablesHelpers
{
    /// <summary>
    /// Contains sorting helpers for In Memory collections.
    /// </summary>
    public static class CollectionHelper
    {
        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.OrderBy(keySelector);
            }

            return items.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IOrderedEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.ThenBy(keySelector);
            }

            return items.ThenByDescending(keySelector);
        }
    }

    public static class InMemoryEmployeeRepository
    {
        private static EmployeeCacheHelper _employeeCacheHelper = new EmployeeCacheHelper();
        private static IList<Employee> GetEmployees()
        {
            return _employeeCacheHelper.GetEmployees.ToList();
        }

        public static IList<Employee> GetDataTableList(int startIndex,
            int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,
            out int totalRecordCount,
            out int searchRecordCount,
            string searchString)
        {
            var employeeList = GetEmployees();

            totalRecordCount = employeeList.Count;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                employeeList = employeeList.Where(c => c.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            searchRecordCount = employeeList.Count;

            IOrderedEnumerable<Employee> sortedEmployeeList = null;
            foreach (var sortedColumn in sortedColumns)
            {
                switch (sortedColumn.PropertyName)
                {
                    case "Name":
                        sortedEmployeeList = sortedEmployeeList == null ? employeeList.CustomSort(sortedColumn.Direction, cust => cust.Name)
                            : sortedEmployeeList.CustomSort(sortedColumn.Direction, cust => cust.Name);
                        break;
                }
            }

            return sortedEmployeeList.Skip(startIndex).Take(pageSize).ToList();
        }
    }
}