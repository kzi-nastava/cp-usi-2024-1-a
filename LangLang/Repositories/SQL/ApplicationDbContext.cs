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
        public DbSet<Language> Languages { get; set; }
        public DbSet<Exam> Exams { get; set; }

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        /*public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DatabaseCredentials databaseCredentials)
            : base(options)
        {
            _databaseCredentials = databaseCredentials;
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Course
            modelBuilder.Entity<Course>()
                .Property(e => e.Start)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Course>()
                .Property(e => e.Schedule)
                .HasConversion(new ScheduleConverter());

            modelBuilder.Entity<Language>()
                .HasKey(l => l.Name); 
            modelBuilder.Entity<Language>()
                .Property(l => l.Name)
                .IsRequired();
            modelBuilder.Entity<Language>()
                .Property(l => l.Code)
                .IsRequired();

            // Configure Course to reference Language by name
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Language)
                .WithMany()
                .IsRequired();

            // Define relationship using a shadow property
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Language)
                .WithMany()
                .HasForeignKey("LanguageName"); // Shadow property representing the foreign key
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Username=postgres;Password=123;Database=langlang;");
                //optionsBuilder.UseNpgsql(_databaseCredentials.ConnectionString);
            }
        }
    }
}
