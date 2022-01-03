using EmployeeManager2.Mvc.Models;
using EmployeeManager2.Mvc.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;    ////kullanıcı giriş sayfası
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeManager2.Mvc.Controllers
{
    public class SecurityController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;/// user manager - kullanıcı hesabı oluşturma ve kullanıcı ayrıntılarını değiştirme gibi kullanıcı merkezli işlemler
        private readonly RoleManager<AppIdentityRole> roleManager;/// role manager - uygulama rollerini yönetme ve sistemden roller oluşturma ve kkaldırmayı sağlar
        private readonly SignInManager<AppIdentityUser> signinManager;  ///kullanıcıyı doğrulamayı ve doğrulaaama bilgisi yayınlamayı sağlar
        public SecurityController(UserManager<AppIdentityUser> userManager,
        RoleManager<AppIdentityRole> roleManager,
        SignInManager<AppIdentityUser> signinManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signinManager = signinManager;
        }

        public IActionResult Register() ///yeni hesap oluştur dediğimizde gelen kayıt hesabı
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Register(Register obj)
        {
            if (ModelState.IsValid)    /// eğer veri doğrulama geçerliyse
            {
                if (!roleManager.RoleExistsAsync("Manager").Result)     ///RoleExistsAsync("Manager") sistemde yönetici olup olmadığı kontrol edilir. yönetici yoksa yeni kayıt manager olur.
                {
                    AppIdentityRole role = new AppIdentityRole();
                    role.Name = "Manager";
                    role.Description = "Can perform CRUD operations.";
                    IdentityResult roleResult =
                    roleManager.CreateAsync(role).Result; ///Atanan manager rolünü oluşturmak için CreateAsync(role) çağrılır.
                }
                AppIdentityUser user = new AppIdentityUser(); ///Yeni bir kullanıcı hesabı oluşturmak için kod, yeni bir AppIdentityUser nesnesi oluşturur.
                user.UserName = obj.UserName;  /// AppIdentityUser, KullanıcıAdı, E-posta, TamAd ve Doğum Tarihi gibi çeşitli ayrıntıları tutan bir sistem kullanıcısını temsil eder.
                user.Email = obj.Email;
                user.FullName = obj.FullName;
                user.BirthDate = obj.BirthDate;
                IdentityResult result = userManager.CreateAsync(user, obj.Password).Result; ///yeni bir kullanıcı oluşturmak için CreateAsync() çağrılır. 2 parametresi vardır biri App ıdentity user nesnesi diğeri paroladır.
                if (result.Succeeded)  ///kullanıcı ekleme başarılı olursa kullanıcı manager rolüne eklenir
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();  ///AddToRoleAsync() yönetici rolüne çağırmak için bu fonksiyon kullanılır. ///Yönetci olanlar çalışanlar listesinde CRUD işlemi gerçekleştirebilir.
                    return RedirectToAction("SignIn", "Security"); ///kullanıcı kayıt edildikten sonra oturum açma sayfasına yönlendirilir.
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details!"); ///veri doğrulamaları geçerli olmazsa hata döndürür
                }
            }
            return View(obj);
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(SignIn obj)
        {
            if (ModelState.IsValid)
            {
                var result = signinManager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, false).Result; ///doğrulamaları sağlıyorsa PasswordSignInAsync() fonksiyonu ile oturum açılır.
                if (result.Succeeded) /// Oturum açma başarılıysa
                {
                    return RedirectToAction("List", "EmployeeManager");
                }
                else
                {
                    ModelState.AddModelError("", " Invalid user details!");
                }
            }
            return View(obj);
        }
        public IActionResult AccessDenied()  ///kullancıya erişilemezse
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignOut()
        {
            signinManager.SignOutAsync().Wait(); ///SignInManager sınıfının SignOutAsync() yöntemini çağırır.---SignOutAsync() yöntemi, kimlik doğrulama tanımlama bilgisini kaldırarak kullanıcının uygulamadan çıkışını imzalar.
            return RedirectToAction("SignIn", "Security"); ///oturum açma sayfasına tekrar yönlendirilir.
        }
    }
}
