using LangLang.Application.DTO;
using LangLang.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repositories.SQL
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DatabaseCredentials _databaseCredentials;
        public DbSet<Course> Courses { get; set; }

        public ApplicationDbContext(DatabaseCredentials databaseCredentials)
        {
            _databaseCredentials = databaseCredentials;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_databaseCredentials.ConnectionString);
        }
    }
}
