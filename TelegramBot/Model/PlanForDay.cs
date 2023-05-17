namespace TelegramBot.Model
{
    public class PlanForDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Client>? Clients { get; set; } = new();
    }
}
