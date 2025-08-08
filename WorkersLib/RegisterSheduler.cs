using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkersLib.Configurations;
using WorkersLib.Jobs;
using WorkersLib.Models;
using WorkersLib.Services;

namespace WorkersLib
{
    /// <summary>
    /// Класс регистрации сервисов заданий
    /// </summary>
    public static class RegisterScheduler
    {
        /// <summary>
        /// старт джобов
        /// </summary>
        public static async Task RunWorkersAsync(this IHost host)
        {
            await DataScheduler.StartAsync(host.Services);
        }
        public static void AddWorkers(this IHostApplicationBuilder builder)
        {
            builder.Services.RegisterServices(); 
            builder.Configuration.RegisterConfig();
        }
        private static void RegisterServices(this IServiceCollection services)
        {
            // Quartz настройки
            services.AddQuartz(q =>
            {
                // это значения по умолчанию
                // Использовать простой загрузчик типов
                q.UseSimpleTypeLoader();
                // Расписание в памяти
                q.UseInMemoryStore(); 
                // Настройки пула потоков
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });
            });
            // Quartz хостинг
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            // [при каждом запросе новый] Фабрика
            services.AddSingleton<JobFactory>();
            // [на коннект один] Jobs
            services.AddScoped<TestJob1>();
            services.AddScoped<TestJob2>();
            // [на коннект один] Services
            services.AddScoped<TestService>();
        }
        private static void RegisterConfig(this IConfigurationManager configurationManager)
        {
            JobConfiguration.SetConfig(configurationManager);
        }
    }
}
