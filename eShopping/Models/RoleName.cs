using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eShopping.Models
{
    public class RoleName
    {
        public const string Admin = "admin";
        public const string Company = "company";
        public const string User = "user";
        public const string Employee = "employee";

        public const string AdminOrUser = Admin + "," + User;
        public const string AdminOrCompany = Admin + "," + Company;
        public const string AdminOrEmployee = Admin + "," + Employee;
    }
}