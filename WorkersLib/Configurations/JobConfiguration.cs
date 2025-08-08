using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkersLib.Configurations
{
    internal static class JobConfiguration
    {
        private static IConfigurationManager? configuration;
        public static void SetConfig(IConfigurationManager config)
        {
            configuration = config;
        }

        internal static class TestJob1Settings
        {
            /// <summary>
            /// Включен или нет
            /// </summary>
            public static bool Enabled => Convert.ToBoolean(configuration?
                .GetSection("JobOption")?
                .GetSection("TestJob1Settings")?
                ["Enabled"] ?? true.ToString());
            /// <summary>
            /// Расписание джоба
            /// </summary>
            public static string Cron => configuration?
                .GetSection("JobOption")?
                .GetSection("TestJob1Settings")?
                ["Cron"] ?? "0/3 * * ? * *"; //каждые 3 сек
        }
        internal static class TestJob2Settings
        {
            /// <summary>
            /// Включен или нет
            /// </summary>
            public static bool Enabled => Convert.ToBoolean(configuration?
                .GetSection("JobOption")?
                .GetSection("TestJob2Settings")?
                ["Enabled"] ?? true.ToString());
            /// <summary>
            /// Расписание джоба
            /// </summary>
            public static string Cron => configuration?
                .GetSection("JobOption")?
                .GetSection("TestJob2Settings")?
                ["Cron"] ?? "0/3 * * ? * *"; //каждые 3 сек
        }
    }
}
