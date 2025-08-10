using Quartz;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using WorkersLib.Interfaces;

namespace WorkersLib.Models
{
    /// <summary>
    /// Класс хранения данных для последующего запуска задачи
    /// </summary>
    [Description("Класс хранения данных для последующего запуска задачи")]
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
        [Description("Идентификатор для запуска")]
        public Guid Id { get; private set; }
        /// <summary>
        /// Тип задачи
        /// </summary>
        [JsonIgnore]
        [Description("Тип задачи")]
        public Type _type { get; private set; }
        /// <summary>
        /// Имя задачи
        /// </summary>
        [Description("Имя задачи")]
        public required string JobName { get; set; }
        /// <summary>
        /// Описание задачи
        /// </summary>
        [Description("Описание задачи")]
        public required string Description { get; set; }
        /// <summary>
        /// Расписание
        /// </summary>
        [Description("Расписание (Cron формат)")]
        public required string Cron { get; set; }
        /// <summary>
        /// Включена к запуску
        /// </summary>
        [Description("Включена к запуску")]
        public required bool Enabled { get; set; }
        /// <summary>
        /// Видима для одиночного запуска
        /// </summary>
        [Description("Включена для одиночного запуска")]
        public required bool VisibleOneStart { get; set; }

        public async Task StartAsync(IScheduler scheduler, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await BaseStarterJob<T>.Start(
                scheduler,
                JobName,
                DataScheduler.WorkGroupJob,
                Description,
                Cron,
                cancellationToken);
        }

        public async Task StartNewAsync(IScheduler scheduler, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await BaseStarterJob<T>.Start(
                scheduler,
                $"New Istanse [{JobName}] ({Guid.NewGuid()})",
                DataScheduler.DefaultGroupJob,
                Description,
                cancellationToken);
        }
    }
}
