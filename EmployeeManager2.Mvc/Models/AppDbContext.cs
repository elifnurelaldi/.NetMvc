using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeManager2.Mvc.Models
{
    public class AppDbContext : DbContext // ef core model
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }
        public DbSet<Employee> Employees { get; set; }/// ne kadar table class açarsan onları buraya ekliyosun -
        public DbSet<Country> Countries { get; set; }
    }
}
