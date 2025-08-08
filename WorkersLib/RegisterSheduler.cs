using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkersLib.Configurations;
using WorkersLib.Jobs;
using WorkersLib.Models;

namespace WorkersLib
{
    public static class RegisterScheduler
    {
        public static void Register(IServiceCollection services, IConfigurationManager configurationManager)
        {
            RegisterServices(services);
            RegisterConfig(configurationManager);
        }
        public static void RegisterServices(IServiceCollection services)
        {
            // при каждом запросе новый
            services.AddTransient<JobFactory>();
            // на коннект один
            services.AddScoped<TestJob1>();
            services.AddScoped<TestJob2>();
        }

        public static void RegisterConfig(IConfigurationManager configurationManager)
        {
            JobConfiguration.SetConfig(configurationManager);
        }
    }
}
