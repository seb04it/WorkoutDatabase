using Microsoft.EntityFrameworkCore;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Data
{
    public class WorkoutDbContext : DbContext
    {
        public DbSet<WorkoutEntities> Workouts => Set<WorkoutEntities>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("WorkoutAppStorageDb");
        }
    }
}