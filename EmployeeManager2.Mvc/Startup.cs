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
            services.AddDbContext<AppDbContext>(    ///app db contexti asp.net core un DI kapsayıcısınaa dahil ediyorsunuz
            options => options.UseSqlServer  ///appDbContext veritabanından haberdar edilir
            (this.config.GetConnectionString("AppDb")));
            services.AddDbContext<AppIdentityDbContext>(options =>options.UseSqlServer(this.config.GetConnectionString("AppDb"))); ///app identity db contexti asp.net core un DI kapsayıcısınaa dahil ediyorsunuz. appIdentityDbContext veritabanından haberdar edilir
            services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();///AddEntityFrameworkStores() => kimlik veri depolarının bir EF Core uygulamasını ekler.
            services.ConfigureApplicationCookie(opt => ///kullanıcıya tanımlama hakkı verir ve bu çerezdir çerez özellileri yapılandırlır.
            {
                opt.LoginPath = "/Security/SignIn"; ///yeni kullanıcıysa kullanıcı signin sayfasına yönlendrilir.
                opt.AccessDeniedPath = "/Security/AccessDenied"; ///erişim engellendi - Örneğin, bir kullanıcı geçerli kimlik bilgileriyle oturum açabilir ancak Yönetici rolüne ait olmayabilir. bu gibi hatatlarda error mesajı verir izin vermez
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) //// asp.net core ortam değişkeni devolopment ise
            { 
                app.UseDeveloperExceptionPage(); 
            }
            app.UseStaticFiles();  ///bu fonksiyonla javascript dosyaları, resim dosyaları ve stil dosyaları gibi statik dosyalara ulaşırsınız
            app.UseRouting(); ///endpoint routing sağlayan bir arayazılım
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {    ///routing endpointe bağlar.
                endpoints.MapControllerRoute(  ///eşleme
                name: "default",
                pattern:
               "{controller=EmployeeManager}/{action=List}/{id?}"); /// controller boşsa employe manager gelir action boşsa list gelir
            });
        }
    }
}
