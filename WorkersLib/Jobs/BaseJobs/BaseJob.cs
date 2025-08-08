using Quartz;

namespace WorkersLib.Jobs.BaseJobs
{
    /// <summary>
    /// Базовая реализация задачи
    /// </summary>
    public abstract class BaseJob : IJob
    {
        public virtual CancellationToken CancellationToken { get; set; } = default;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await RunAsync(CancellationToken);
            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected abstract Task RunAsync(CancellationToken cancellationToken = default);
    }
}
