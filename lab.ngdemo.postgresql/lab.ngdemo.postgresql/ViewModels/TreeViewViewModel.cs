using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace lab.ngdemo.postgresql.ViewModels
{
    public class TreeViewViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TreeViewViewModel> Details { get; set; }
           
    }
}