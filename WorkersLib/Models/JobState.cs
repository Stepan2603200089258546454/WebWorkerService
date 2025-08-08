namespace WorkersLib.Models
{
    /// <summary>
    /// Класс состояния работы
    /// </summary>
    public class JobState()
    {
        public class StateInstanse
        {
            public DateTime? StartTime { get; set; }
            public DateTime? StartTimeSchedule { get; set; }
            public TimeSpan? RunTime { get; set; }
            public JobWrapper? Instance { get; set; }
        }
        /// <summary>
        /// Имя задачи
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Группа задачи
        /// </summary>
        public string Group { get; set; } = string.Empty;
        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Дата следующего запуска
        /// </summary>
        public DateTime? NextRun { get; set; }
        /// <summary>
        /// Дата предыдущего запуска
        /// </summary>
        public DateTime? LastRun { get; set; }
        /// <summary>
        /// Описание состояния
        /// </summary>
        public string State { get; set; } = string.Empty;
        /// <summary>
        /// Выполняется или нет
        /// </summary>
        public bool IsRunning { get; set; }
        /// <summary>
        /// Список запусков
        /// </summary>
        public IEnumerable<StateInstanse>? Instances { get; set; }
        /// <summary>
        /// Кол-во запусков
        /// </summary>
        public int Count => Instances?.Count() ?? 0;
    }
}
