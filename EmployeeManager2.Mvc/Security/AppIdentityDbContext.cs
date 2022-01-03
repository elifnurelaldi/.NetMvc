using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager2.Mvc.Security
{
    public class AppIdentityDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string> ///temel alınan kullanıcı ve rol veri deposuyla iletişim kurar.                                                                    
    {                                                                                               /// Bu örnekte, kullanıcı ve rol ayrıntılarını Northwind veritabanının kendisinde depolarsınız.     
        public AppIdentityDbContext                                                                 // TUser parametresi, uygulamanın kullanıcısının türünü belirtir              
        (DbContextOptions<AppIdentityDbContext> options) : base(options)                            //TRole parametresi, uygulamanın rolünün türünü belirtir.
        { }
    }
}
