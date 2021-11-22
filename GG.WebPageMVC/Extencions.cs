using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GG.Core;
using GG.Data;
using GG.Infrastructure.Repositories;
using GG.WebPageMVC.Automapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CsvHelper;
using System.IO;
using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace GG.WebPageMVC
{
    public static class Extencions
    {

       
        public static IApplicationBuilder AddTravelsPackages(this IApplicationBuilder app)
        {
           
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                //Obtenemos el entorno donde se ejecuta la aplicacion
                IWebHostEnvironment _env = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {



                   var travels = context.TravelPackages.ToList();

                    if (travels is not null)
                    {
                        if (travels.Count == 0)
                        {

                            var pathBase = _env.WebRootPath;

                            var csvPath = Path.Combine(pathBase, "Csv", "TravelPackages.csv");

                            var config = new CsvConfiguration(new CultureInfo("es-MX")) { MissingFieldFound = null, HeaderValidated = null };

                            using (StreamReader reader = new StreamReader(csvPath, Encoding.GetEncoding(1252)))
                            {
                                using (var csvreader = new CsvReader(reader, config))
                                {

                                    var records = csvreader.GetRecords<PrivateTravelPackage>();

                                    if (records is not null)
                                    {
                                        var Packages = records.ToList();

                                        context.TravelPackages.AddRange(Packages);


                                    }

                                    context.SaveChanges();


                                }



                            }

                        }
                   
                        
                    }



                }




                return app;
            }
        }


        public static IApplicationBuilder AddUsers(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                //Obtenemos el entorno donde se ejecuta la aplicacion
                IWebHostEnvironment _env = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {


                    var users = context.Users.ToList();

                    if (users.Count == 0)
                    {

                        var pathBase = _env.WebRootPath;

                        var csvPath = Path.Combine(pathBase, "Csv", "Users.csv");

                        var config = new CsvConfiguration(new CultureInfo("es-MX")) { MissingFieldFound = null, HeaderValidated = null };

                        using (StreamReader reader = new StreamReader(csvPath, Encoding.GetEncoding(1252)))
                        {
                            using (var csvreader = new CsvReader(reader, config))
                            {

                                var records = csvreader.GetRecords<PrivateUser>();

                                if (records is not null)
                                {
                                    var rUsers = records.ToList();

                                    var _userManager = serviceScope.ServiceProvider.GetService<UserManager<PrivateUser>>();

                                    foreach (var user in rUsers)
                                    {
                                        user.EmailConfirmed = true;
                                        user.PhoneNumberConfirmed = true;

                                        user.UserName = user.Email; 
                                        var result = _userManager.CreateAsync(user, user.Password).Result;
                                    }


                                }

                               


                            }
                        }


                    }
                }



            }

                    return app;
        }

        public static IApplicationBuilder SetLikes(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                //Obtenemos el entorno donde se ejecuta la aplicacion
                IWebHostEnvironment _env = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {


                    var users = context.Users.ToList();
                    var travels = context.TravelPackages.ToList();
                    var likes = context.LikedPackages.ToList();
      

                    if (users.Count > 0 && travels.Count > 0 && likes.Count == 0)
                    {

                        foreach (var user in users)
                        {
                            //asignamos una cantidad aleatoria de me gustas por usuario
                            var cantidadLikes = new Random(2304).Next(15, 45);                 

                            for (int i = 0; i < cantidadLikes; i++)
                            {
                                //usamos para determinar si ya ha sido likeado este paquete
                                List<int> indexUsed = new List<int>();
                                int indexTravel = 0;
                                var indexRandom = new Random();

                                do
                                {
                                     indexTravel = indexRandom.Next(0, travels.Count - 1);

                                    

                                } while (indexUsed.Contains(indexTravel));

                                indexUsed.Add(indexTravel);
                                var travel = travels[indexTravel];

                              
                                user.MyLikes.Add(new LikedPackage() { UserId = user.Id, TravelPackageId = travel.Id });


                            }

                        }

                        context.SaveChanges();

                    }
                }



            }

            return app;

        }

        public static IApplicationBuilder SetReviews(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                //Obtenemos el entorno donde se ejecuta la aplicacion
                IWebHostEnvironment _env = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {


                    var users = context.Users.ToList();
                    var travels = context.TravelPackages.OrderBy(t => t.Id).ToList();
                    var reviews = context.Ratings.ToList();


                    if (users.Count > 0 && travels.Count > 0 && reviews.Count == 0)
                    {
                        var pathBase = _env.WebRootPath;

                        var csvPath = Path.Combine(pathBase, "Csv", "Ratings.csv");

                        var config = new CsvConfiguration(new CultureInfo("es-MX")) { MissingFieldFound = null, HeaderValidated = null };

                        using (StreamReader reader = new StreamReader(csvPath, Encoding.GetEncoding(1252)))
                        {
                            using (var csvreader = new CsvReader(reader, config))
                            {

                                var records = csvreader.GetRecords<PrivateRating>();

                                if (records is not null)
                                {
                                    var rRatings = records.ToList();


                                    for (int i = 0; i < rRatings.Count; i++)
                                    {

                                        rRatings[i].IdTravelPackageNavigation = travels[i % travels.Count];

                                        rRatings[i].IdUserNavigation = users[i % users.Count];


                                    }
                                    context.Ratings.AddRange(rRatings);
                                }
                            }
                        }
                    

                        context.SaveChanges();

                    }
                }




            }

            return app;

        }

        public static IApplicationBuilder TrainModelFirs(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _recommender = serviceScope.ServiceProvider.GetService<RecommenderSystem>();
                 _recommender.TrainModelAsync();
            }

            return app;

        }
        public static ICollection<PrivateTravelPackage> RatingPrediction(this ICollection<PrivateTravelPackage> travelPackages, string userId)
        {
            foreach (var item in travelPackages)
            {
                item.RatingPrediction = GGMLModel.Predict(userId, item).Score;
            }

            return travelPackages.OrderByDescending(p => p.RatingPrediction).ToList();

        }



    }
}

