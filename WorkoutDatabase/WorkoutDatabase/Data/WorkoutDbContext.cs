using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Data
{
    public class WorkoutDbContext : DbContext
    {

        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options)
            : base(options)
        {

        }
        
        public DbSet<WorkoutEntities> Workouts { get; set; }
    }
}