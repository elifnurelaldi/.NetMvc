using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManager2.Mvc.Models
{
    [Table("Countries")] //countries veritabanıyla eşleme
    public class Country
    {
        public int CountryID { get; set; }
        public string Name { get; set; }

    }
}
