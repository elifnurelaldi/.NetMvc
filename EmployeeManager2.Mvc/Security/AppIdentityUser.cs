using Microsoft.AspNetCore.Identity;   // UserName ve Email özelliklerini sağlar
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager2.Mvc.Security
{
    public class AppIdentityUser : IdentityUser
    {
        public string FullName { get; set; }  ///user Name ve Email yetmezse ek özellikler sağlar.
        public DateTime BirthDate { get; set; }
    }
}
