using Microsoft.EntityFrameworkCore;
using VestalisQuintet.EconomyBot.Models;

namespace VestalisQuintet.EconomyBot
{
    public class VQEconomyBotDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=VQEconomyBotDb.db3");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /* カラムをユニーク指定する方法の例
            builder.Entity<User>()
                .HasIndex(m => m.DiscordId)
                .IsUnique();
            */
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}
