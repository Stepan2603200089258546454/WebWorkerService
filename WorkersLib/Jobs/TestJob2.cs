using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace WorkersLib.Jobs
{
    [DisallowConcurrentExecution]
    public class TestJob2 : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public TestJob2(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            //using (IServiceScope scope = serviceScopeFactory.CreateScope())
            //{
            //    MailServices services = scope.ServiceProvider.GetService<MailServices>();
            //}
        }
    }
}
