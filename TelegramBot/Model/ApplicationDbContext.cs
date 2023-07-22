using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Model
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Worker> Workers { get; set; }
		public DbSet<Client> Clients { get; set; }
		public DbSet<Abonement> Abonements { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public DbSet<PlanForDay> PlanForDay { get; set; }
		public DbSet<Training> Workout { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				 .AddJsonFile("appsettings.json")
				 .Build();
			optionsBuilder.UseNpgsql(configuration.GetConnectionString("MyConnection"));
		}
	}
}

