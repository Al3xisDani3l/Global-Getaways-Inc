using GG.Core;
using GG.Data;
using GG.Infrastructure.Repositories;
using GG.WebPageMVC.Automapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;
using GG.Infrastructure;
using GG.WebPageMVC.Areas.Identity.Pages.Account;

namespace GG.WebPageMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GG.Infrastructure.Options.PasswordOptions>(conf => Configuration.GetSection("PasswordOptions").Bind(conf));

            services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            //añadimos los servicios de sendgrid con el patron singleton
            services.AddSingleton(new SendGridClient(Configuration["sendgridkey"]));

            //Añadimos el servicio de mensajeria
            services.AddTransient<IEmailSender, EmailSender>();


            services.AddAutoMapper(auto =>
            {
                auto.AddProfile(
                    new AutomapperInitializer(
                       Configuration.GetSection("AutomapperProfile").Get<AutomapperProfile>())
                    );
            });


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection"));
                
            }

               );
           
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<PrivateUser>(options =>

            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
               
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            //agregamos los logins externos de la app
            services.AddAuthentication()
                .AddFacebook("Facebook",options =>
                {
                    options.ClientId = Configuration["FacebookOptions:client_id"];
                    options.ClientSecret = Configuration["FacebookOptions:client_secret"];
                })
               .AddLinkedIn("Linkedin",options =>
               {
                   options.ClientId = Configuration["LinkedinOptions:client_id"];
                   options.ClientSecret = Configuration["LinkedinOptions:client_secret"];
               })
                 .AddGoogle("Google", options => {

                   
                     options.ClientId = Configuration["GoogleOptions:client_id"];
                     options.ClientSecret = Configuration["GoogleOptions:client_secret"];
                     options.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                     options.SaveTokens = true;

                 }); ;


            services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));



            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.AddUsers();
                app.AddTravelsPackages();
                app.SetLikes();
                app.SetReviews();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
               
               
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

            });


           


        }

    }
}
