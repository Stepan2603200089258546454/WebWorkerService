using WorkersLib.Jobs.BaseJobs;
using WorkersLib.Services;

namespace WorkersLib.Jobs
{
    /// <summary>
    /// Пример работы которая может работать параллельно [DI работает]
    /// </summary>
    public class TestJob1 : BaseSingleCancelJob
    {
        private readonly TestService _service;
        public TestJob1(TestService service)
        {
            _service = service;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"{DateTime.Now} [{_service.Id}] {nameof(TestJob1)} Start");
            
            await Task.Delay(TimeSpan.FromSeconds(10));
            CancellationToken.ThrowIfCancellationRequested();
            
            await Task.Delay(TimeSpan.FromSeconds(10));
            CancellationToken.ThrowIfCancellationRequested();
            
            await Task.Delay(TimeSpan.FromSeconds(10));
            CancellationToken.ThrowIfCancellationRequested();
            
            Console.WriteLine($"{DateTime.Now} [{_service.Id}] {nameof(TestJob1)} End");
        }
    }
}
