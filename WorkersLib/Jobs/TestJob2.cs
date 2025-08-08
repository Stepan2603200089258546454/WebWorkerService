using WorkersLib.Jobs.BaseJobs;
using WorkersLib.Services;

namespace WorkersLib.Jobs
{
    /// <summary>
    /// Пример работы которая не может работать параллельно [DI работает]
    /// </summary>
    public class TestJob2 : BaseSingleJob
    {
        private readonly TestService _service;
        public TestJob2(TestService service)
        {
            _service = service;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"{DateTime.Now} [{_service.Id}] {nameof(TestJob2)} Start");
            
            await Task.Delay(TimeSpan.FromSeconds(10));
            
            Console.WriteLine($"{DateTime.Now} [{_service.Id}] {nameof(TestJob2)} End");
        }
    }
}
