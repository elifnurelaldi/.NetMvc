using EmployeeManager2.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager2.Mvc
{
    public class EmployeeManagerController : Controller    //Crud işlemleri controllerde yapılır
    {
        private AppDbContext db = null;     //AppDbContext nesnesini oluşturuyorsun
        public EmployeeManagerController(AppDbContext db)
        {
            this.db = db;
        }

        private void FillCountries()     //ülkeleri liste haline çeviriyosun liste olucak çünkü--update ve insert de kullanabilmek için
        {
            List<SelectListItem> countries =   //Linq to entities query
            (from c in db.Countries
             orderby c.Name ascending
             select new SelectListItem()
             {
                 Text = c.Name,
                 Value = c.Name
             }).ToList();
            ViewBag.Countries = countries;
        }

        [AllowAnonymous] /// herkes erişebilir
        public IActionResult List()       //Çalışanları listeye dönüştürmek içindir. ayrıca List.cshtml oluşturulacak
        {
            List<Employee> model = (from e in db.Employees
                                    orderby e.EmployeeID
                                    select e).ToList();
            return View(model); //modeli viewe gönderiyor
        }

        [Authorize(Roles = "Manager")] ////sadece yöneticilerden giriş yapan erişebilir.
        public IActionResult Insert()  /// insert ile ilgili 2 action ----ilki inserte basıldığında ekleme sayfasının gelmesi için
        {
            FillCountries();
            return View();
        }

        [HttpPost]
        public IActionResult Insert(Employee model) //ikincisi ise ekledikten sonra(formu gönderdikten sonra) sonra olanlar için--- form post yöntemiyle gönderilir.
        {                                             //formu gönderdikten sonra asp.netcore formu otomatik doldurur buna model binding denir.
            FillCountries();
            if (ModelState.IsValid) ///uyarı vermiyorsa veritabanına ekler data annotations
            {
                db.Employees.Add(model);
                db.SaveChanges();
                ViewBag.Message = "Employee inserted successfully";
            }
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Update(int id)
        {
            FillCountries();
            Employee model = db.Employees.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Employee model)
        {
            FillCountries();
            if (ModelState.IsValid)
            {
                db.Employees.Update(model);
                db.SaveChanges();
                ViewBag.Message = "Employee updated successfully";
            }
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            Employee model = db.Employees.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int employeeID)
        {
            Employee model = db.Employees.Find(employeeID);
            db.Employees.Remove(model);
            db.SaveChanges();
            TempData["Message"] = "Employee deleted successfully";
            return RedirectToAction("List"); /// Kullanıcıyı çalışan listesine yönlendirir
        }


    }
}
