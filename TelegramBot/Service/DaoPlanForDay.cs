using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TelegramBot.Model;

namespace TelegramBot.Service
{
    public class DaoPlanForDay : IDao<PlanForDay>
    {
        ApplicationDbContext _context;

        public DaoPlanForDay(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlanForDay> AddAsync(PlanForDay obj)
        {
            if (obj.Clients != null)
            {
                DaoClient daoClient = new DaoClient(_context);
                List<Client> clients = new List<Client>();
                foreach (Client client in obj.Clients)
                    clients.Add(await daoClient.GetAsync(client.Id));
                obj.Clients = clients;
            }
            _context.PlanForDay.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<PlanForDay>? DeleteAsync(int id)
        {
            PlanForDay plan = await GetAsync(id);
            if (plan != null)
                _context.PlanForDay.Remove(plan);
            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<List<PlanForDay>> GetAllAsync()
        {
            return await _context.PlanForDay.ToListAsync();
        }

        public async Task<PlanForDay>? GetAsync(int id)
        {
            List<PlanForDay> list = new List<PlanForDay>();
            list = await _context.PlanForDay.ToListAsync();
            foreach (PlanForDay temp in list)
                if (temp.Id == id)
                    return temp;
            return null;
        }

        public async Task<PlanForDay>? FindToDateAsync(DateOnly date)
        {
            List<PlanForDay> list = new List<PlanForDay>();
            list = await _context.PlanForDay.ToListAsync();
            foreach (PlanForDay temp in list)
                if (DateOnly.FromDateTime(temp.Date.Date) == date)
                    return temp;
            return null;
        }

        public async Task<PlanForDay> UpdateAsync(PlanForDay obj)
        {
            if (obj.Clients != null)
            {
                DaoClient daoClient = new DaoClient(_context);
                List<Client> clients = new List<Client>();
                foreach (Client client in obj.Clients)
                    clients.Add(await daoClient.GetAsync(client.Id));
                obj.Clients = clients;
            }
            _context.PlanForDay.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}


