namespace TelegramBot.Model
{
    public class Worker
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long ChatId { get; set; }
        public List<Client>? Clients { get; set; } = new();
        public PlanForDay? Plan { get; set; }
    }
}
