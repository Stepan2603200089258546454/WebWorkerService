using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Spi;

namespace WorkersLib.Models
{
    /// <summary>
    /// Фабрика запуска джобов
    /// </summary>
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScopeFactory serviceScopeFactory;

        public JobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new JobWrapper(serviceScopeFactory, bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}
