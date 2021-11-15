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

                    var travels = context.TravelPackages.ToList();

                    if (travels is not null)
                    {
                        if (travels.Count == 0)
                        {


                        }
                    }



                }




                return app;
            }
        }
    }
}

