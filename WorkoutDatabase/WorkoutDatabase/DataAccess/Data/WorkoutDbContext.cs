using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.DataAccess.Data
{
    public class WorkoutDbContext : DbContext
    {

        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options)
            : base(options)
        {

        }

        public DbSet<WorkoutEntity> Workouts { get; set; }
        public DbSet<WorkoutCategoryEntity> WorkoutCategoryEntities { get; set; }
    }
}