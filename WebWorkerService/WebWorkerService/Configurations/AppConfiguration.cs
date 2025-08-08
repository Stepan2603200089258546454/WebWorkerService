namespace WebWorkerService.Configurations
{
    public static class AppConfiguration
    {
        private static IConfigurationManager? configuration;
        public static void SetConfig(IConfigurationManager config)
        {
            configuration = config;
        }

        /// <summary>
        /// Время обновления страницы с расписанием работ
        /// </summary>
        public static int? RefreshSecondsJobsUI 
            => string.IsNullOrWhiteSpace(configuration?.GetSection("UI")?["RefreshSecondsJobsUI"]) == false
            ? Convert.ToInt32(configuration?.GetSection("UI")?["RefreshSecondsJobsUI"] ?? 0.ToString())
            : null;

    }
}
