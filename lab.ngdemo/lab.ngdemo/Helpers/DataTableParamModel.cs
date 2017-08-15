using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Helpers
{
    public class DataTableParamModel
    {
        // Request sequence number sent by DataTable, same value must be returned in response
        public string sEcho { get; set; }

        // Text used for filtering
        public string sSearch { get; set; }

        public string sSearch_0 { get; set; }

        public string sSearch_1 { get; set; }

        public string sSearch_2 { get; set; }

        // Number of records that should be shown in table
        public int iDisplayLength { get; set; }

        // First record that should be shown(used for paging)
        public int iDisplayStart { get; set; }

        // Number of columns in table
        public int iColumns { get; set; }

        // Number of columns that are used in sorting
        public int iSortingCols { get; set; }

        // Comma separated list of column names
        public string sColumns { get; set; }
    }
}