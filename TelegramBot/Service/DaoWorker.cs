using Microsoft.EntityFrameworkCore;
using TelegramBot.Model;

namespace TelegramBot.Service
{
    public class DaoWorker : IDao<Worker>
    {
        ApplicationDbContext _context;

       public DaoWorker(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Worker> AddAsync(Worker obj)
        {
            if (obj.Clients != null)
            {
                DaoClient daoClient = new DaoClient(_context);
                List<Client> clients = new List<Client>();
                foreach (Client client in obj.Clients)
                    clients.Add(await daoClient.GetAsync(client.Id));
                obj.Clients = clients;
            }
            _context.Workers.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<Worker>? DeleteAsync(int id)
        {
            Worker worker = await GetAsync(id);
            if (worker != null)
                _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        public async Task<List<Worker>> GetAllAsync()
        {
            return await _context.Workers.ToListAsync();
        }

        public async Task<Worker>? GetAsync(int id)
        {
            List<Worker> list = new List<Worker>();
            list = await _context.Workers.ToListAsync();
            foreach (Worker temp in list)
                if (temp.Id == id)
                    return temp;
            return null;
        }

        public async Task<Worker> UpdateAsync(Worker obj)
        {
            if (obj.Clients != null)
            {
                DaoClient daoClient = new DaoClient(_context);
                List<Client> clients = new List<Client>();
                foreach (Client client in obj.Clients)
                    clients.Add(await daoClient.GetAsync(client.Id));
                obj.Clients = clients;
            }
            _context.Workers.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}
