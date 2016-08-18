using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Helpers
{
    public static class MessageResourceHelper
    {
        public static string Save { get { return "Saved Successfully."; } }
        public static string Update { get { return "Updated Successfully."; } }
        public static string Delete { get { return "Deleted Successfully."; } }
        public static string Add { get { return "Added Successfully."; } }
        public static string Edit { get { return "Edited Successfully."; } }
        public static string Remove { get { return "Removed Successfully."; } }
        public static string UnhandelledError { get { return "We are facing some problem while processing the current request. Please try again later."; } }
        public static string UnAuthenticated { get { return "You are not authenticated user."; } }
        public static string NullError { get { return "Requested object could not found."; } }
        public static string NullReferenceExceptionError { get { return "There are one or more required fields that are missing."; } }

    }
}