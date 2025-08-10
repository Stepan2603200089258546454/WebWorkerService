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
        /// <summary>
        /// Создать джоб
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="group">Группа</param>
        /// <param name="description">Описание</param>
        /// <returns>Джоб</returns>
        private static IJobDetail CreateJobDetails(string name, string group, string description)
        {
            IJobDetail jobDetail = JobBuilder.Create<T>()
                .WithIdentity(name, group)
                .WithDescription(description)
                .Build();
            return jobDetail;
        }
        /// <summary>
        /// Создать триггер (одноразовый запуск)
        /// </summary>
        /// <param name="name">Имя джобы</param>
        /// <param name="group">Группа джобы</param>
        /// <param name="description">Описание джобы</param>
        /// <returns>Триггер</returns>
        private static ITrigger CreateJobTrigger(string name, string group, string description)
        {
            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity(name + "Trigger", group)  // идентифицируем триггер с именем и группой
                .WithDescription(description)           // описание
                .StartNow()                             // запуск сразу после начала выполнения
                .Build();                               // создаем триггер
            return trigger;
        }
        /// <summary>
        /// Создать триггер (запуск по расписанию)
        /// </summary>
        /// <param name="name">Имя джобы</param>
        /// <param name="group">Группа джобы</param>
        /// <param name="description">Описание джобы</param>
        /// <param name="cron">Cron расписание</param>
        /// <returns>Триггер</returns>
        private static ITrigger CreateJobTrigger(string name, string group, string description, string cron)
        {
            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity(name + "Trigger", group)  // идентифицируем триггер с именем и группой
                .WithDescription(description)           // описание
                .WithCronSchedule(cron)                 // Запуск по расписанию (cron)
                .Build();                               // создаем триггер
            return trigger;
        }
        /// <summary>
        /// Запустить одиночное выполнение
        /// </summary>
        public static async Task Start(IScheduler scheduler, string name, string group, string description, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await scheduler.ScheduleJob(CreateJobDetails(name, group, description), CreateJobTrigger(name, group, description), cancellationToken);
        }
        /// <summary>
        /// Запустить выполнение по расписанию
        /// </summary>
        public static async Task Start(IScheduler scheduler, string name, string group, string description, string cron, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await scheduler.ScheduleJob(CreateJobDetails(name, group, description), CreateJobTrigger(name, group, description, cron), cancellationToken);
        }
    }
}
