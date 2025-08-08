using Quartz;

namespace WorkersLib.Interfaces
{
    public interface ICancelJob : IJob
    {
        CancellationTokenSource CancellationTokenSource { get; }
        CancellationToken CancellationToken { get; }
    }
}
