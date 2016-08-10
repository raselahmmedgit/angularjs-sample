using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.postgresql.ViewModels
{
    public class AssignRoleModel
    {
        public AssignRoleModel()
        {
            IsAssigned = false;
        }

        public string RoleName { get; set; }

        public bool IsAssigned { get; set; }
    }
}