using Microsoft.EntityFrameworkCore;
using TelegramBot.Model;

namespace TelegramBot.Service
{
    public class DaoAbonement : IDao<Abonement>
    {
        ApplicationDbContext _context;
        public DaoAbonement(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Abonement> AddAsync(Abonement obj)
        {
            DaoStatus status = new DaoStatus(_context);
            obj.Status = await status.GetAsync(obj.Status.Id);
            _context.Abonements.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<Abonement>? DeleteAsync(int id)
        {
            Abonement abonement = await GetAsync(id);
            if (abonement != null)
                _context.Abonements.Remove(abonement);
            await _context.SaveChangesAsync();
            return abonement;
        }

        public async Task<List<Abonement>> GetAllAsync()
        {
            return await _context.Abonements.ToListAsync();
        }

        public async Task<Abonement>? GetAsync(int id)
        {
            List<Abonement> list = new List<Abonement>();
            list = await _context.Abonements.ToListAsync();
            foreach (Abonement temp in list)
                if (temp.Id == id)
                    return temp;
            return null;
        }

        public async Task<Abonement> UpdateAsync(Abonement obj)
        {
            DaoStatus status = new DaoStatus(_context);
            obj.Status = await status.GetAsync(obj.Status.Id);
            _context.Abonements.Update(obj);
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}
