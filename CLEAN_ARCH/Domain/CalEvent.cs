namespace Domain
{
    public enum CalEventStatus
    {
        Active,
        Disabled,
        Deleted
    }

    public enum CalEventRepeat
    {
        Once,
        Yearly,
        Monthly,
        EveryXdays,
    }

    public class CalEventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CalEvent
    {
        public int Id { get; set; }

        public CalEventCategory? Category { get; set; }

        public CalEventRepeat Repeat { get; set; }

        public CalEventStatus Status { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Started { get; set; }

        public DateTime? Created { get; set; }

        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int? EveryXDays { get; set; }

        public string Description { get; set; }

        public TimeSpan Time { get; set; }
    }
}
