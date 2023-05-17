namespace TelegramBot.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long ChatId { get; set; }
        public Abonement? Abonement { get; set; }
        public DateTime StartTrainig { get; set; }
        public DateTime EndTrainig { get; set;}
    }
}
