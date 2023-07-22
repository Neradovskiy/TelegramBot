namespace TelegramBot.Model
{
    public class PlanForDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Training>? Trainings { get; set; } = new();
    }
}
