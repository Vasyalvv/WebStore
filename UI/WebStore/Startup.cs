using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Clients.Employees;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestApi;
using WebStore.Services.Data;
using WebStore.Services.Services.InCookies;
using WebStore.Services.Services.InMemory;
using WebStore.Services.Services.InSQL;

namespace WebStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("Default"))
                //.LogTo(Console.WriteLine)
                );

            services.AddIdentity<User, Role>()
                //.AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            #region Identity stores custom implementation

            services.AddTransient<IUserStore<User>, UsersClient>();
            services.AddTransient<IUserRoleStore<User>, UsersClient>();
            services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            services.AddTransient<IUserEmailStore<User>, UsersClient>();
            services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            services.AddTransient<IUserLoginStore<User>, UsersClient>();
            services.AddTransient<IUserClaimStore<User>, UsersClient>();

            services.AddTransient<IRoleStore<Role>, RolesClient>();

            #endregion

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 3;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore-lvv";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddTransient<WebStoreDbInitializer>();

            //services.AddTransient<IEmployeesData, InMemoryEmployeeData>();
            services.AddTransient<IEmployeesData, EmployeesClient>();
            //services.AddTransient<IProductData, InMemoryProductData>();
            //services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IProductData, ProductsClient>();
            services.AddScoped<ICartServices, InCookiesCartService>();
            //services.AddScoped<IOrderService, SqlOrderService>();
            services.AddScoped<IOrderService, OrdersClient>();
            services.AddScoped<IValuesService, ValuesClient>();

            services
                .AddControllersWithViews(
                //mvc=>mvc.Conventions.Add(new ActionDescriptionAttribute("123"))
                )
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDbInitializer db)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.Map();
            //app.Use();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGet("/Greetings", async context =>
                {
                    await context.Response.WriteAsync($"Hello World!\n{Configuration["Greetings"]}");
                });

                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
