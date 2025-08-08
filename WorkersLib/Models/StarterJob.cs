using Quartz;
using System.Xml.Linq;
using WorkersLib.Interfaces;

namespace WorkersLib.Models
{
    /// <summary>
    /// Класс хранения данных для последующего запуска джоба
    /// </summary>
    public class StarterJob<T> : IStarterJob where T : class, IJob
    {
        public StarterJob()
        {
            Id = Guid.NewGuid();
            _type = typeof(T);
        }
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// Тип задачи
        /// </summary>
        public Type _type { get; private set; }
        /// <summary>
        /// Имя задачи
        /// </summary>
        public string JobName => _type.Name;
        /// <summary>
        /// Описание задачи
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// Расписание
        /// </summary>
        public required string Cron { get; set; }
        /// <summary>
        /// Включена к запуску
        /// </summary>
        public required bool Enabled { get; set; }
        /// <summary>
        /// Видима для одиночного запуска
        /// </summary>
        public required bool VisibleOneStart { get; set; }

        public async Task StartAsync(IScheduler scheduler)
        {
            await BaseStarterJob<T>.Start(
                scheduler,
                JobName,
                DataScheduler.WorkGroupJob,
                Description,
                Cron);
        }

        public async Task StartNewAsync(IScheduler scheduler)
        {
            await BaseStarterJob<T>.Start(
                scheduler,
                $"New Istanse [{JobName}] ({Guid.NewGuid()})",
                DataScheduler.DefaultGroupJob,
                Description);
        }
    }
}
