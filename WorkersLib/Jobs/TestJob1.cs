using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkersLib.Jobs
{
    public class TestJob1 : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public TestJob1(IServiceScopeFactory serviceScopeFactory)
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
