using EmployeeManager2.Mvc.Models;
using EmployeeManager2.Mvc.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager2.Mvc
{
    public class Startup
    {
        private IConfiguration config = null;
        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ///services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(    ///app db contexti asp.net core un DI kapsayýcýsýnaa dahil ediyorsunuz
            options => options.UseSqlServer  ///appDbContext veritabanýndan haberdar edilir
            (this.config.GetConnectionString("AppDb")));
            services.AddDbContext<AppIdentityDbContext>(options =>options.UseSqlServer(this.config.GetConnectionString("AppDb"))); ///app identity db contexti asp.net core un DI kapsayýcýsýnaa dahil ediyorsunuz. appIdentityDbContext veritabanýndan haberdar edilir
            services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();///AddEntityFrameworkStores() => kimlik veri depolarýnýn bir EF Core uygulamasýný ekler.
            services.ConfigureApplicationCookie(opt => ///kullanýcýya tanýmlama hakký verir ve bu çerezdir çerez özellileri yapýlandýrlýr.
            {
                opt.LoginPath = "/Security/SignIn"; ///yeni kullanýcýysa kullanýcý signin sayfasýna yönlendrilir.
                opt.AccessDeniedPath = "/Security/AccessDenied"; ///eriþim engellendi - Örneðin, bir kullanýcý geçerli kimlik bilgileriyle oturum açabilir ancak Yönetici rolüne ait olmayabilir. bu gibi hatatlarda error mesajý verir izin vermez
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) //// asp.net core ortam deðiþkeni devolopment ise
            { 
                app.UseDeveloperExceptionPage(); 
            }
            app.UseStaticFiles();  ///bu fonksiyonla javascript dosyalarý, resim dosyalarý ve stil dosyalarý gibi statik dosyalara ulaþýrsýnýz
            app.UseRouting(); ///endpoint routing saðlayan bir arayazýlým
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {    ///routing endpointe baðlar.
                endpoints.MapControllerRoute(  ///eþleme
                name: "default",
                pattern:
               "{controller=EmployeeManager}/{action=List}/{id?}"); /// controller boþsa employe manager gelir action boþsa list gelir
            });
        }
    }
}
