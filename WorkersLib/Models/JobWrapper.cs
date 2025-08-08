using Microsoft.Extensions.DependencyInjection;
using Quartz;
using WorkersLib.Interfaces;

namespace WorkersLib.Models
{
    public class JobWrapper : IJob, IDisposable
    {
        private readonly IServiceScope? _serviceScope;
        private readonly IJob? _job;

        public JobWrapper(IServiceScopeFactory serviceScopeFactory, Type jobType)
        {
            _serviceScope = serviceScopeFactory.CreateScope();
            _job = ActivatorUtilities.CreateInstance(_serviceScope.ServiceProvider, jobType) as IJob;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return _job?.Execute(context) ?? Task.CompletedTask;
        }

        public (bool IsCancel, ICancelJob? CancelJob) IsCancelJob()
        {
            if(_job is ICancelJob cancel)
                return (true, cancel);
            else
                return (false, null);
        }

        public void Dispose()
        {
            (_job as IDisposable)?.Dispose();
            _serviceScope?.Dispose();
        }
    }
}
