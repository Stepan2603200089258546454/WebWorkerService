using Quartz;
using WorkersLib.Interfaces;

namespace WorkersLib.Jobs.BaseJobs
{
    /// <summary>
    /// Реализация отменяемой сингл задачи
    /// </summary>
    public abstract class BaseSingleCancelJob : BaseSingleJob, ICancelJob
    {
        public CancellationTokenSource CancellationTokenSource { get; internal set; } = new CancellationTokenSource();
        public override CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
