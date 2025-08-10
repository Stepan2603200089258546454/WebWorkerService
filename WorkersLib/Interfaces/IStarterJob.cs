using Quartz;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WorkersLib.Interfaces
{
    [Description("Интерфейс хранения данных для последующего запуска задачи")]
    public interface IStarterJob
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Description("Идентификатор для запуска")]
        Guid Id { get; }
        /// <summary>
        /// Тип задачи
        /// </summary>
        [JsonIgnore]
        [Description("Тип задачи")]
        Type _type { get; }
        /// <summary>
        /// Имя задачи
        /// </summary>
        [Description("Имя задачи")]
        string JobName { get; }
        /// <summary>
        /// Описание задачи
        /// </summary>
        [Description("Описание задачи")]
        string Description { get; }
        /// <summary>
        /// Расписание
        /// </summary>
        [Description("Расписание (Cron формат)")]
        string Cron { get; }
        /// <summary>
        /// Включена к запуску
        /// </summary>
        [Description("Включена к запуску")]
        bool Enabled { get; }
        /// <summary>
        /// Видима для одиночного запуска
        /// </summary>
        [Description("Включена для одиночного запуска")]
        bool VisibleOneStart { get; }
        
        Task StartAsync(IScheduler scheduler, CancellationToken cancellationToken = default);
        Task StartNewAsync(IScheduler scheduler, CancellationToken cancellationToken = default);
    }
}
