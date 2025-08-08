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
        }

        public string Name { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? NextRun { get; set; }
        public DateTime? LastRun { get; set; }
        public string State { get; set; } = string.Empty;
        public bool IsRunning { get; set; }
        public int Count { get; set; }
        public IEnumerable<StateInstanse> Instances { get; set; }
    }
}
