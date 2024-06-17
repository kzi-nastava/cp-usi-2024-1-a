using LangLang.Application.DTO;
using LangLang.Domain.Model;
using Microsoft.EntityFrameworkCore;
using LangLang.Repositories.SQL.ComplexDataConverters;
using Microsoft.Extensions.Configuration;
using System;

namespace LangLang.Repositories.SQL
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DatabaseCredentials _databaseCredentials;
        public DbSet<Course> Courses { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Tutor> Tutors { get; set; }


        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DatabaseCredentials databaseCredentials)
            : base(options)
        {
            _databaseCredentials = databaseCredentials;
        }

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

            modelBuilder.Entity<Exam>()
                .Property(e => e.Time)
                .HasColumnType("timestamp without time zone");

            // Configure Exam to reference Language by name
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Language)
                .WithMany()
                .IsRequired();

            // Define relationship using a shadow property
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Language)
                .WithMany()
                .HasForeignKey("LanguageName"); // Shadow property representing the foreign key

            modelBuilder.Entity<Tutor>()
                .Property(t => t.BirthDate)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<Tutor>()
                .Property(t => t.DateAdded)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<Tutor>()
                .Property(t => t.KnownLanguages)
                .HasConversion(new KnownLanguagesConverter());
            modelBuilder.Entity<Tutor>()
                .Property(t => t.RatingCounts)
                .HasConversion<int[]>();
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddUserSecrets<App>()
                    .Build();
                DatabaseCredentials databaseCredentials =
                    new DatabaseCredentials(
                        config["Database:Host"] ?? "",
                        config.GetValue<int>("Database:Port"),
                        config["Database:Username"] ?? "",
                        config["Database:Password"] ?? "",
                        config["Database:DatabaseName"] ?? ""
                    );
                optionsBuilder.UseNpgsql(databaseCredentials.ConnectionString);
            }
        }
    }
}
