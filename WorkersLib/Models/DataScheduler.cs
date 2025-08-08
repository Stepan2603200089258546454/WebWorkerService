using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Globalization;
using System.Runtime;
using WorkersLib.Configurations;
using WorkersLib.Interfaces;
using WorkersLib.Jobs;

namespace WorkersLib.Models
{
    /// <summary>
    /// [Основной] класс взаимодействия с джобами
    /// </summary>
    public static partial class DataScheduler
    {
        public const string DefaultGroupJob = "default";

        private static List<IStarterJob> _starterJobs = null;
        /// <summary>
        /// Получить список запускальщиков джобов
        /// </summary>
        public static List<IStarterJob> GetStarterJobs()
        {
            if (_starterJobs == null)
            {
                _starterJobs = new List<IStarterJob>()
                {
                    new StarterJob<TestJob1>()
                    {
                        Enabled = JobConfiguration.TestJob1Settings.Enabled,
                        Cron = JobConfiguration.TestJob1Settings.Cron,
                        Description = "Тестовая задача 1",
                    },
                    new StarterJob<TestJob2>()
                    {
                        Enabled = JobConfiguration.TestJob2Settings.Enabled,
                        Cron = JobConfiguration.TestJob2Settings.Cron,
                        Description = "Тестовая задача 2",
                    },
                };
            }
            return _starterJobs;
        }
        /// <summary>
        /// Запустить новый экземпляр (одиночное выполнение)
        /// </summary>
        public static async void StartNew(Guid id)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            GetStarterJobs().FirstOrDefault(x => x.Id == id)?.StartNew(scheduler);
        }
        /// <summary>
        /// Запустить новые экземпляры (одиночное выполнение)
        /// </summary>
        public static async Task StartNewAsync(IEnumerable<Guid> id, CancellationToken cancellationToken = default(CancellationToken))
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;

            IEnumerable<IStarterJob> _jobs = GetStarterJobs().Where(x => id.Contains(x.Id));

            foreach (var item in _jobs)
            {
                if (cancellationToken.IsCancellationRequested) return;

                item.StartNew(scheduler);
            }
        }
        /// <summary>
        /// Старт программы
        /// </summary>
        public static async Task Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
            await scheduler.Start();

            foreach (var job in GetStarterJobs().Where(x => x.Enabled))
            {
                job.Start(scheduler);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static async Task Restart()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            IReadOnlyCollection<JobKey> jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
            await scheduler.DeleteJobs(jobKeys);

            await scheduler.Start();
            foreach (var job in GetStarterJobs().Where(x => x.Enabled))
            {
                job.Start(scheduler);
            }
        }
        /// <summary>
        /// Получить информацию по запущенным джобам
        /// </summary>
        public static async Task<List<JobState>> GetListJobAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);

            IReadOnlyCollection<string> jobGroups = await scheduler.GetJobGroupNames();
            List<JobState> result = new List<JobState>();

            // Получаем все исполняющиеся в данный момент задачи
            IReadOnlyCollection<IJobExecutionContext> executingJobs = await scheduler.GetCurrentlyExecutingJobs();

            HashSet<JobKey> runningJobKeys = executingJobs.Select(j => j.JobDetail.Key).ToHashSet();

            foreach (string group in jobGroups)
            {
                IReadOnlyCollection<JobKey> jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));

                foreach (JobKey jobKey in jobKeys)
                {
                    IJobDetail? detail = await scheduler.GetJobDetail(jobKey);
                    IReadOnlyCollection<ITrigger> triggers = await scheduler.GetTriggersOfJob(jobKey);

                    DateTimeOffset? nextFireTime = triggers
                        .Select(t => t.GetNextFireTimeUtc())
                        .Where(t => t.HasValue)
                        .Min(); // Ближайший запуск

                    DateTimeOffset? lastFireTime = triggers
                        .Select(t => t.GetPreviousFireTimeUtc())
                        .Where(t => t.HasValue)
                        .Max(); // Последний запуск

                    TriggerState triggerState = await scheduler.GetTriggerState(triggers.FirstOrDefault()?.Key);

                    IEnumerable<IJobExecutionContext> execItems = executingJobs.Where(j => j.JobDetail.Key == jobKey);

                    int count = execItems.Count();

                    result.Add(new JobState
                    {
                        Name = jobKey.Name,
                        Group = jobKey.Group,
                        Description = detail?.Description ?? triggers.FirstOrDefault()?.Description ?? "-",
                        NextRun = nextFireTime?.LocalDateTime,
                        LastRun = lastFireTime?.LocalDateTime,
                        State = triggerState switch
                        {
                            TriggerState.Normal => "Запланирована",
                            TriggerState.Paused => "Приостановлена",
                            TriggerState.Blocked => "Заблокирована",
                            TriggerState.Error => "Ошибка",
                            TriggerState.Complete => "Завершена",
                            TriggerState.None => "Не существует",
                            _ => triggerState.ToString()
                        },
                        IsRunning = runningJobKeys.Contains(jobKey),
                        Count = count,
                        Instances = execItems.OrderBy(x => x.FireTimeUtc).Select(x => new JobState.StateInstanse()
                        {
                            StartTime = x.FireTimeUtc.ToLocalTime().LocalDateTime,
                            StartTimeSchedule = x.ScheduledFireTimeUtc?.ToLocalTime().LocalDateTime,
                            RunTime = DateTime.UtcNow - x.FireTimeUtc
                        })
                    });
                }
            }

            return result;
        }
    }
}
