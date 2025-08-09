namespace WebWorkerService.Configurations
{
    public class AppConfiguration
    {
        private readonly IConfiguration? configuration;
        public AppConfiguration(IConfiguration config)
        {
            configuration = config;
        }

        /// <summary>
        /// Время обновления страницы с расписанием работ
        /// </summary>
        public int? RefreshSecondsJobsUI 
            => string.IsNullOrWhiteSpace(configuration?.GetSection("UI")?["RefreshSecondsJobsUI"]) == false
            ? Convert.ToInt32(configuration?.GetSection("UI")?["RefreshSecondsJobsUI"] ?? 0.ToString())
            : null;

    }
}
