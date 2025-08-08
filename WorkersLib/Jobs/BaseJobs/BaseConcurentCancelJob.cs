using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkersLib.Interfaces;

namespace WorkersLib.Jobs.BaseJobs
{
    /// <summary>
    /// Реализация отменяемой мультистартовой задачи
    /// </summary>
    public abstract class BaseConcurrentCancelJob : BaseConcurrentJob, ICancelJob
    {
        public CancellationTokenSource CancellationTokenSource { get; internal set; } = new CancellationTokenSource();
        public override CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}
