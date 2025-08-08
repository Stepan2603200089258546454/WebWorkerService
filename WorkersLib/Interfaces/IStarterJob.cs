using Quartz;

namespace WorkersLib.Interfaces
{
    public interface IStarterJob
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Тип задачи
        /// </summary>
        Type _type { get; }
        /// <summary>
        /// Имя задачи
        /// </summary>
        string JobName => _type.Name;
        /// <summary>
        /// Описание задачи
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Расписание
        /// </summary>
        string Cron { get; }
        /// <summary>
        /// Включена к запуску
        /// </summary>
        bool Enabled { get; }
        /// <summary>
        /// Видима для одиночного запуска
        /// </summary>
        bool VisibleOneStart { get; }
        Task StartAsync(IScheduler scheduler);
        Task StartNewAsync(IScheduler scheduler);
    }
}
