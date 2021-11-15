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

namespace GG.WebPageMVC
{
    public static class StartupExtencions
    {
        public static IApplicationBuilder AddFirstData(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {

                    //  var travels = context.TravelPackages.ToList();

                    //if (travels is not null)
                    //{
                    //    if (travels.Count == 0)
                    //    {

                    var pathBase = Environment.CurrentDirectory;

                            var csvPath = Path.Combine(pathBase, "Metodos datos.csv");

                    var config = new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null, HeaderValidated = null };

                            using (StreamReader reader = new StreamReader(@"C:\Users\alexi\source\repos\global getaways inc\GG.WebPageMVC\wwwroot\Metodos datos.csv") )
                            {
                        using (var csvreader = new CsvReader(reader, config) )
                                {

                                    var recors = csvreader.GetRecords<PrivateTravelPackage>();

                                    if (recors is not null)
                                    {
                                       var list = recors.ToList();
                                    }

                                    context.SaveChanges();


                                }
                                



                        //    }

                            

                        //}
                    }



                }




                return app;
            }
        }
    }
}

