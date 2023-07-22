using Microsoft.EntityFrameworkCore;
using TelegramBot.Model;

namespace TelegramBot.Service
{
	public class DaoTraining : IDao<Training>
	{
		ApplicationDbContext _context;

		public DaoTraining(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Training> AddAsync(Training obj)
		{
			if (obj.Clients != null)
			{
				DaoClient daoClient = new DaoClient(_context);
				List<Client> clients = new List<Client>();
				foreach (Client client in obj.Clients)
					clients.Add(await daoClient.GetAsync(client.Id));
				obj.Clients = clients;
			}

			_context.Workout.Add(obj);
			await _context.SaveChangesAsync();
			return obj;
		}

		public async Task<Training>? DeleteAsync(int id)
		{
			Training training = await GetAsync(id);
			if (training != null)
				_context.Workout.Remove(training);
			await _context.SaveChangesAsync();
			return training;
		}

		public async Task<List<Training>> GetAllAsync()
		{
			return await _context.Workout.ToListAsync();
		}

		public async Task<Training>? GetAsync(int id)
		{
			List<Training> list = new List<Training>();
			list = await _context.Workout.ToListAsync();
			foreach (Training temp in list)
				if (temp.Id == id)
					return temp;
			return null;
		}

		public async Task<Training>? FindToDateAsync(DateOnly date)
		{
			List<Training> list = new List<Training>();
			list = await _context.Workout.ToListAsync();
			foreach (Training temp in list)
				if (DateOnly.FromDateTime(temp.Time) == date)
					return temp;
			return null;
		}

		public async Task<Training> UpdateAsync(Training obj)
		{
			if (obj.Clients != null)
			{
				DaoClient daoClient = new DaoClient(_context);
				List<Client> clients = new List<Client>();
				foreach (Client client in obj.Clients)
					clients.Add(await daoClient.GetAsync(client.Id));
				obj.Clients = clients;
			}
			_context.Workout.Update(obj);
			await _context.SaveChangesAsync();
			return obj;
		}
	}
}
