namespace XSing.Core.db
{
    using env;
    using etc;
    using Microsoft.EntityFrameworkCore;
    using models;

    public class SingContext : DbContext
    {
        public DbSet<Record> Records { get; set; }
        public DbSet<Setting> Settings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Env.ContentPath.WithCombine("xsing.db")}");
        }
    }
}