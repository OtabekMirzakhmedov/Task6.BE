using Microsoft.EntityFrameworkCore;

namespace Task6.BE
{
    public class MailDbContext: DbContext
    {
        public DbSet<Email> Emails { get; set; }

        public MailDbContext(DbContextOptions<MailDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Email>().HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }

    }
}
