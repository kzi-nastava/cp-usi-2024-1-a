using LangLang.Application.DTO;
using LangLang.Domain.Model;
using Microsoft.EntityFrameworkCore;
using LangLang.Repositories.SQL.ComplexDataConverters;

namespace LangLang.Repositories.SQL
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DatabaseCredentials _databaseCredentials;
        public DbSet<Course> Courses { get; set; }
        //public DbSet<Language> Languages { get; set; }  

        public ApplicationDbContext()
        {}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DatabaseCredentials databaseCredentials)
        : base(options) { 
            _databaseCredentials = databaseCredentials;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .OwnsOne(c => c.Language)
                .Property(l => l.Name)
                .HasColumnName("LanguageName");

            modelBuilder.Entity<Course>()
                .OwnsOne(c => c.Language)
                .Property(l => l.Code)
                .HasColumnName("LanguageCode");

            modelBuilder.Entity<Course>()
               .Property(e => e.Start)
               .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Course>()
                .Property(e => e.Schedule)
                .HasConversion(new ScheduleConverter());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_databaseCredentials.ConnectionString);
            }
        }

    }
}
