using Quartz;

namespace WorkersLib.Jobs.BaseJobs
{
    /// <summary>
    /// Реализация сингл задачи. Повторяет базовую реализацию
    /// </summary>
    [DisallowConcurrentExecution]
    public abstract class BaseSingleJob : BaseJob
    {

    }
}
