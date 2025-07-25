namespace GoalGridApp.Data
{
    public class DailyTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string? Category { get; set; } = "General";
    }
}
