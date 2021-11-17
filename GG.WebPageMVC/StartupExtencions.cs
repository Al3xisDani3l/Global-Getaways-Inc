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

namespace GG.WebPageMVC
{
    public static class StartupExtencions
    {

       
        public static IApplicationBuilder AddFirstData(this IApplicationBuilder app)
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

                            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null, HeaderValidated = null };

                            using (StreamReader reader = new StreamReader(csvPath))
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
    }
}

