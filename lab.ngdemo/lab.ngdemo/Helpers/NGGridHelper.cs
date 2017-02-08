using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Helpers
{
    public class pageParams
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string filterText { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }

    public class pageResult<T>
    {
        public IList<T> data { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public int totalPages { get; set; }
    }
}