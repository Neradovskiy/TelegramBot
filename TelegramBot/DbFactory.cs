using Microsoft.EntityFrameworkCore;
using TelegramBot.Model;

namespace TelegramBot
{
    public class DbFactory
    {
        private readonly IServiceScopeFactory scopeFactory;
        public DbFactory(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public ApplicationDbContext GetContext()
        {
            var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        }

    }
}
