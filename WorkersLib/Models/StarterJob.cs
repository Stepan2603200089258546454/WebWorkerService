using Quartz;
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
        /// 
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Type _type { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string JobName => _type.Name;
        /// <summary>
        /// 
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public required string Cron { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public required bool Enabled { get; set; }

        public async void Start(IScheduler scheduler)
        {
            await BaseStarterJob<T>.Start(
                scheduler,
                JobName,
                DataScheduler.DefaultGroupJob,
                Description,
                Cron);
        }

        public async void StartNew(IScheduler scheduler)
        {
            await BaseStarterJob<T>.Start(
                scheduler,
                JobName,
                DataScheduler.DefaultGroupJob,
                Description);
        }
    }
}
