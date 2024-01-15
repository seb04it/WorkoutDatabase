using Microsoft.EntityFrameworkCore;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Data
{
    public class WorkoutDbContext : DbContext
    {
        public DbSet<Workout> Workouts => Set<Workout>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("WorkoutStorageDb");
        }
    }
}
