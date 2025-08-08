using Quartz;

namespace WorkersLib.Interfaces
{
    public interface IStarterJob
    {
        Guid Id { get; }
        Type _type { get; }
        string JobName => nameof(_type);
        string Description { get; }
        string Cron { get; }
        bool Enabled { get; }
        void Start(IScheduler scheduler);
        void StartNew(IScheduler scheduler);
    }
}
