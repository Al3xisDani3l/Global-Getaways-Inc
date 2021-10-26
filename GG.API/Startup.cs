using GG.Infrastructure.Options;
using GG.Infrastructure.Repositories;
using GG.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using GG.Core;
using GG.Api.Automapper;
using GG.Database;


namespace GG.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {



            services.AddAutoMapper(auto =>
            {
                auto.AddProfile(
                    new AutomapperInitializer(
                       Configuration.GetSection("AutomapperProfile").Get<AutomapperProfile>())
                    );
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                 builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            });

       

            services.AddDbContext<GGContext>(options => { options.UseNpgsql(Configuration.GetConnectionString("GGDB_postgres")); options.UseLazyLoadingProxies(); options.EnableDetailedErrors(); }) ;
          //services.AddDbContext<GGDBContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("GGDB")); options.UseLazyLoadingProxies(); });

            services.Configure<PasswordOptions>(conf => Configuration.GetSection("PasswordOptions").Bind(conf));

            #region Entidades de dominio

            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddTransient(typeof(IAccountRepository), typeof(AccountRepository));         
            services.AddSingleton<IPasswordService, PasswordService>();

           


            #endregion

            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "GG API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                doc.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))


                };
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
             //   app.UpdateDatabase();
             //    app.AddAdminister(Configuration);
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

           

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "GG API Documentation");


            });

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(async context =>
            {
                context.Response.Redirect("swagger");
            });
        }

      
    }

    public static class StartupExtencions
    {
        public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
               .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<GGContext>())
                {

                    //var result = context.Database.CanConnect();

                    //var createscrip = context.Database.GenerateCreateScript();

                    context.Database.Migrate();


                }


            }
               
            return app;
        }

        public static IApplicationBuilder AddAdminister(this IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices
               .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<GGContext>())
                {

                    var password = serviceScope.ServiceProvider.GetService<IPasswordService>();

                    PrivateUser administer = new PrivateUser();

                    administer.Email = configuration["Administer:email"].ToString();
                    administer.Phone = configuration["Administer:telefono"].ToString();
                    administer.Name = configuration["Administer:nombre"].ToString(); ;
                    administer.Birthday = DateTime.Parse(configuration["Administer:fechaNacimiento"].ToString());
                    administer.Lastname = configuration["Administer:apellido"].ToString();
                    administer.Password = password.Hash(configuration["Administer:password"].ToString());
                    administer.Role = RoleType.Administrator;
                    administer.GoogleUUID = null;
                   

                    if (context.Users.FirstOrDefaultAsync(u => u.Email == administer.Email) == null)
                    {
                        context.Users.Add(administer);
                        context.SaveChanges();
                    }

                   

                }


            }

            return app;
        }
    }

}
