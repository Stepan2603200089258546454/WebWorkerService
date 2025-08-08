using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkersLib.Models
{
    /// <summary>
    /// Класс запуска джобов
    /// </summary>
    /// <typeparam name="T">класс реализующий IJob</typeparam>
    public static class BaseStarterJob<T> where T : class, IJob
    {
        private static IJobDetail CreateJobDetails()
        {
            IJobDetail jobDetail = JobBuilder.Create<T>().Build();
            return jobDetail;
        }
        private static IJobDetail CreateJobDetails(string name, string group, string description)
        {
            IJobDetail jobDetail = JobBuilder.Create<T>()
                .WithIdentity(name, group)
                .WithDescription(description)
                .Build();
            return jobDetail;
        }

        private static ITrigger CreateJobTrigger(string name, string group, string description)
        {
            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity($"New Istanse [{name}] ({Guid.NewGuid()})", group) // идентифицируем триггер с именем и группой
                .WithDescription(description)           // описание
                .StartNow()                             // запуск сразу после начала выполнения
                                                        //.WithSimpleSchedule(x => x              // настраиваем выполнение действия
                                                        //    .WithIntervalInMinutes(1)           // через 1 минуту
                                                        //    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер
            return trigger;
        }
        private static ITrigger CreateJobTrigger(string name, string group, string description, string cron)
        {
            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity(name, group)              // идентифицируем триггер с именем и группой
                .WithDescription(description)           // описание
                //.StartNow()                             // запуск сразу после начала выполнения
                .WithCronSchedule(cron)                 // Запуск по рассписанию (cron)
                .Build();                               // создаем триггер
            return trigger;
        }

        /// <summary>
        /// Запустить одиночное выполнение
        /// </summary>
        public static async Task Start(IScheduler scheduler, string name, string group, string description)
        {
            await scheduler.ScheduleJob(CreateJobDetails(), CreateJobTrigger(name, group, description)); // начинаем выполнение работы
        }
        /// <summary>
        /// Запустить выполнение по расписанию
        /// </summary>
        public static async Task Start(IScheduler scheduler, string name, string group, string description, string cron)
        {
            await scheduler.ScheduleJob(CreateJobDetails(name, group, description), CreateJobTrigger(name, group, description, cron)); // начинаем выполнение работы
        }
    }
}
