using System;
using GG.Core;
using GG.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GG.WebPageMVC.Areas.Identity.Pages.Account;

[assembly: HostingStartup(typeof(GG.WebPageMVC.Areas.Identity.IdentityHostingStartup))]
namespace GG.WebPageMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.UseKestrel();
           
            builder.ConfigureServices((context, services) => {

               
            });


        }
    }
}