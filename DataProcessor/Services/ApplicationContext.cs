using DataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace DataProcessor.Services {
    public class ApplicationContext : DbContext {
        public DbSet<ModuleModel> Modules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string connectionString = ConfigurationManager.AppSettings["SqlLite"];
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
