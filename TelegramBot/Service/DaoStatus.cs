using Microsoft.EntityFrameworkCore;
using TelegramBot.Model;

namespace TelegramBot.Service
{
    public class DaoStatus : IDao<Status>
    {
        ApplicationDbContext _context;
       public DaoStatus(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Status> AddAsync(Status obj)
        {
            _context.Statuses.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<Status>? DeleteAsync(int id)
        {
            Status status = await GetAsync(id);
            if (status != null)
                _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return status; 
        }

        public async Task<List<Status>> GetAllAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<Status>? GetAsync(int id)
        {
            List<Status> list = new List<Status>();
            list = await _context.Statuses.ToListAsync();
            foreach (Status temp in list)
                if (temp.Id == id)
                    return temp;
            return null;
        }

        public async Task<Status> UpdateAsync(Status obj)
        {
            _context.Statuses.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}
