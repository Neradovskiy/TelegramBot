namespace TelegramBot.Model
{
	//db name Workout
	public class Training
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }
		public List<Client>? Clients { get; set; } = new List<Client>();
	}
}
