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
            services.AddDbContext<AppDbContext>(    ///app db contexti asp.net core un DI kapsay�c�s�naa dahil ediyorsunuz
            options => options.UseSqlServer  ///appDbContext veritaban�ndan haberdar edilir
            (this.config.GetConnectionString("AppDb")));
            services.AddDbContext<AppIdentityDbContext>(options =>options.UseSqlServer(this.config.GetConnectionString("AppDb"))); ///app identity db contexti asp.net core un DI kapsay�c�s�naa dahil ediyorsunuz. appIdentityDbContext veritaban�ndan haberdar edilir
            services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();///AddEntityFrameworkStores() => kimlik veri depolar�n�n bir EF Core uygulamas�n� ekler.
            services.ConfigureApplicationCookie(opt => ///kullan�c�ya tan�mlama hakk� verir ve bu �erezdir �erez �zellileri yap�land�rl�r.
            {
                opt.LoginPath = "/Security/SignIn"; ///yeni kullan�c�ysa kullan�c� signin sayfas�na y�nlendrilir.
                opt.AccessDeniedPath = "/Security/AccessDenied"; ///eri�im engellendi - �rne�in, bir kullan�c� ge�erli kimlik bilgileriyle oturum a�abilir ancak Y�netici rol�ne ait olmayabilir. bu gibi hatatlarda error mesaj� verir izin vermez
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) //// asp.net core ortam de�i�keni devolopment ise
            { 
                app.UseDeveloperExceptionPage(); 
            }
            app.UseStaticFiles();  ///bu fonksiyonla javascript dosyalar�, resim dosyalar� ve stil dosyalar� gibi statik dosyalara ula��rs�n�z
            app.UseRouting(); ///endpoint routing sa�layan bir arayaz�l�m
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {    ///routing endpointe ba�lar.
                endpoints.MapControllerRoute(  ///e�leme
                name: "default",
                pattern:
               "{controller=EmployeeManager}/{action=List}/{id?}"); /// controller bo�sa employe manager gelir action bo�sa list gelir
            });
        }
    }
}
