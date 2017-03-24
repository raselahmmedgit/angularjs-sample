using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Helpers
{
    public class CustomGridParam
    {
        public string Keyword { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string FilterText { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}