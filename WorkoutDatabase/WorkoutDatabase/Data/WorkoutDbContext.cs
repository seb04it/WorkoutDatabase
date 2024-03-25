using Microsoft.EntityFrameworkCore;
using WorkoutDataBase.Entities;

namespace WorkoutDataBase.Data
{
    public class WorkoutDbContext : DbContext
    {
        public DbSet<Workout> Workouts => Set<Workout>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("WorkoutAppStorageDb");
        }
    }
}