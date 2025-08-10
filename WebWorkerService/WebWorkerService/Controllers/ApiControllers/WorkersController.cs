using Microsoft.AspNetCore.Mvc;
using WorkersLib.Interfaces;
using WorkersLib.Models;

namespace WebWorkerService.Controllers.ApiControllers
{
    public class WorkersController : BaseApiController
    {
        [HttpGet]
        [EndpointDescription("Получить список выполняющихся работ")]
        public async Task<IEnumerable<JobState>> GetList(CancellationToken cancellationToken)
        {
            return await DataScheduler.GetListJobAsync(cancellationToken);
        }
        [HttpGet]
        [EndpointDescription("Получить список возможных одиночных запусков")]
        public IReadOnlyCollection<IStarterJob> GetStartersList()
        {
            return DataScheduler.GetVisibleStarterJobs();
        }
        [HttpPost]
        [EndpointDescription("Запустить работы")]
        public async Task StartNews(IEnumerable<Guid> id, CancellationToken cancellationToken)
        {
            await DataScheduler.StartNewAsync(id, cancellationToken);
        }
    }
}
