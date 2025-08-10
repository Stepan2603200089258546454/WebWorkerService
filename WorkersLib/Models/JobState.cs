using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WorkersLib.Models
{
    /// <summary>
    /// Класс состояния работы
    /// </summary>
    [Description("Состояние работы")]
    public class JobState()
    {
        [Description("Состояние экземпляра запуска")]
        public class StateInstanse
        {
            [Description("Текстовое поле номера запуска")]
            public string InstanceId { get; set; } = string.Empty;
            [Description("Дата запуска задачи")]
            public DateTime? StartTime { get; set; }
            [Description("Планируемая дата запуска задачи")]
            public DateTime? StartTimeSchedule { get; set; }
            [Description("Общее время работы")]
            public TimeSpan? RunTime { get; set; }
            [JsonIgnore]
            public JobWrapper? Instance { get; set; }
        }
        /// <summary>
        /// Имя задачи
        /// </summary>
        [Description("Имя задачи")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Группа задачи
        /// </summary>
        [Description("Группа задачи")]
        public string Group { get; set; } = string.Empty;
        /// <summary>
        /// Описание задачи
        /// </summary>
        [Description("Описание задачи")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Дата следующего запуска
        /// </summary>
        [Description("Дата следующего запуска")]
        public DateTime? NextRun { get; set; }
        /// <summary>
        /// Дата предыдущего запуска
        /// </summary>
        [Description("Дата предыдущего запуска")]
        public DateTime? LastRun { get; set; }
        /// <summary>
        /// Описание состояния
        /// </summary>
        [Description("Описание состояния")]
        public string State { get; set; } = string.Empty;
        /// <summary>
        /// Выполняется или нет
        /// </summary>
        [Description("Выполняется или нет")]
        public bool IsRunning { get; set; }
        /// <summary>
        /// Список запусков
        /// </summary>
        [Description("Список запусков")]
        public IEnumerable<StateInstanse>? Instances { get; set; }
        /// <summary>
        /// Кол-во запусков
        /// </summary>
        [Description("Кол-во запусков")]
        public int Count => Instances?.Count() ?? 0;
    }
}
