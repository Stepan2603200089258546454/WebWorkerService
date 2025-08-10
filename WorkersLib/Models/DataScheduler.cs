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
        /// <summary>
        /// Группа джоб по умолчанию
        /// </summary>
        public const string DefaultGroupJob = "default";
        /// <summary>
        /// Группа джоб для одиночного запуска руками
        /// </summary>
        public const string WorkGroupJob = "work";
        /// <summary>
        /// Список посчитанных запускателей с настройками (нужно перегенирировать если нужно обновить
        /// </summary>
        private static IReadOnlyCollection<IStarterJob> _starterJobs = null;
        /// <summary>
        /// Получить список запускальщиков джобов
        /// </summary>
        public static IReadOnlyCollection<IStarterJob> GetStarterJobs()
        {
            if (_starterJobs is null or { Count: 0 })
            {
                _starterJobs = new List<IStarterJob>()
                {
                    new StarterJob<TestJob1>()
                    {
                        Enabled = JobConfiguration.TestJob1Settings.Enabled,
                        Cron = JobConfiguration.TestJob1Settings.Cron,
                        VisibleOneStart = JobConfiguration.TestJob1Settings.VisibleOneStart,
                        JobName = "Задача 1",
                        Description = "Тестовая задача 1",
                    },
                    new StarterJob<TestJob2>()
                    {
                        Enabled = JobConfiguration.TestJob2Settings.Enabled,
                        Cron = JobConfiguration.TestJob2Settings.Cron,
                        VisibleOneStart = JobConfiguration.TestJob2Settings.VisibleOneStart,
                        JobName = "Задача 2",
                        Description = "Тестовая задача 2",
                    },
                };
            }
            return _starterJobs;
        }
        /// <summary>
        /// Получить список доступных для одноразового запуска шаблонов
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyCollection<IStarterJob> GetVisibleStarterJobs() 
            => GetStarterJobs()?.Where(x => x.VisibleOneStart)?.ToList() ?? new List<IStarterJob>(capacity: 0);
        /// <summary>
        /// Запустить новый экземпляр (одиночное выполнение)
        /// </summary>
        public static async Task StartNewAsync(Guid id, CancellationToken cancellationToken = default)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            if (cancellationToken.IsCancellationRequested) return;
            IStarterJob? starter = GetStarterJobs().FirstOrDefault(x => x.Id == id && x.VisibleOneStart);
            if (cancellationToken.IsCancellationRequested) return;
            if (starter is not null) 
                await starter.StartNewAsync(scheduler, cancellationToken);
        }
        /// <summary>
        /// Запустить новые экземпляры (одиночное выполнение)
        /// </summary>
        public static async Task StartNewAsync(IEnumerable<Guid> id, CancellationToken cancellationToken = default)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;

            IEnumerable<IStarterJob> _jobs = GetStarterJobs().Where(x => id.Contains(x.Id) && x.VisibleOneStart);

            foreach (var item in _jobs)
            {
                await item.StartNewAsync(scheduler, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return;
            }
        }
        /// <summary>
        /// Старт программы
        /// </summary>
        public static async Task StartAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
            // запустим если не запускались планировщик
            if (scheduler.IsStarted == false)
                await scheduler.Start(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            // запустим что можем
            foreach (var job in GetStarterJobs().Where(x => x.Enabled))
            {
                await job.StartAsync(scheduler, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return;
            }
        }
        /// <summary>
        /// Перезапуск расписания
        /// </summary>
        public static async Task RestartAsync(CancellationToken cancellationToken = default)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            // Посылаем отмену всем доступным задачам
            IReadOnlyCollection<IJobExecutionContext> executingJobs = await scheduler.GetCurrentlyExecutingJobs(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            foreach (IJobExecutionContext execJob in executingJobs)
            {
                if (cancellationToken.IsCancellationRequested) return;
                (bool IsCancel, ICancelJob? CancelJob) res = (execJob.JobInstance as JobWrapper)?.IsCancelJob() ?? (false, null);
                if (res.IsCancel)
                {
                    res.CancelJob?.CancellationTokenSource?.Cancel();
                }
            }
            // чистим расписание
            IReadOnlyCollection<TriggerKey> jobTriggers = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup(), cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            await scheduler.UnscheduleJobs(jobTriggers, cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            IReadOnlyCollection<JobKey> jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup(), cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            await scheduler.DeleteJobs(jobKeys, cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            // запустим если не запускались
            if (scheduler.IsStarted == false)
                await scheduler.Start(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return;
            // запускаем расписание по новой
            _starterJobs = new List<IStarterJob>();
            foreach (var job in GetStarterJobs().Where(x => x.Enabled))
            {
                await job.StartAsync(scheduler, cancellationToken);
                if (cancellationToken.IsCancellationRequested) return;
            }
        }
        /// <summary>
        /// Получить информацию по запущенным джобам
        /// </summary>
        public static async Task<List<JobState>> GetListJobAsync(CancellationToken cancellationToken = default)
        {
            List<JobState> result = new List<JobState>();

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return result;
            // Получаем список групп
            IReadOnlyCollection<string> jobGroups = await scheduler.GetJobGroupNames(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return result;
            // Получаем все исполняющиеся в данный момент задачи
            IReadOnlyCollection<IJobExecutionContext> executingJobs = await scheduler.GetCurrentlyExecutingJobs(cancellationToken);
            if (cancellationToken.IsCancellationRequested) return result;
            // Получаем уникальные ключи
            HashSet<JobKey> runningJobKeys = executingJobs.Select(j => j.JobDetail.Key).ToHashSet();
            // Проходим по группам
            foreach (string group in jobGroups)
            {
                // Получаем ключи в этой группе
                IReadOnlyCollection<JobKey> jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group), cancellationToken);
                if (cancellationToken.IsCancellationRequested) return result;
                // Проходим по ключам
                foreach (JobKey jobKey in jobKeys)
                {
                    // Получаем детали задачи
                    IJobDetail? detail = await scheduler.GetJobDetail(jobKey, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) return result;
                    // Получаем триггеры для ключа
                    IReadOnlyCollection<ITrigger> triggers = await scheduler.GetTriggersOfJob(jobKey, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) return result;
                    // Получаем смещение следующего запуска
                    DateTimeOffset? nextFireTime = triggers
                        .Select(t => t.GetNextFireTimeUtc())
                        .Where(t => t.HasValue)
                        .Min(); // Ближайший запуск
                    // Получаем смещение предыдущего запуска
                    DateTimeOffset? lastFireTime = triggers
                        .Select(t => t.GetPreviousFireTimeUtc())
                        .Where(t => t.HasValue)
                        .Max(); // Последний запуск
                    // Получаем состояние тириггера
                    TriggerState triggerState = await scheduler.GetTriggerState(triggers.FirstOrDefault()?.Key, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) return result;
                    // Получаем запущенные задачи по ключу
                    IEnumerable<IJobExecutionContext> execItems = executingJobs.Where(j => j.JobDetail.Key == jobKey);
                    // Добавляем модель
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
                        Instances = execItems.OrderBy(x => x.FireTimeUtc).Select(x => new JobState.StateInstanse()
                        {
                            InstanceId = x.FireInstanceId,
                            StartTime = x.FireTimeUtc.ToLocalTime().LocalDateTime,
                            StartTimeSchedule = x.ScheduledFireTimeUtc?.ToLocalTime().LocalDateTime,
                            RunTime = DateTime.UtcNow - x.FireTimeUtc,
                            Instance = x.JobInstance as JobWrapper
                        })
                    });
                }
            }

            return result;
        }
    }
}
