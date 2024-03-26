using Microsoft.EntityFrameworkCore;
using WorkoutDataBase.Entities;
using WorkoutDataBase.Repositories;

namespace WorkoutDatabase.Tests
{
    public class SqlRepositoryTests
    {
        public class TestDbContext : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase("TestDatabase");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Workout>();
            }
        }

        public class TestSqlRepository<T> : SqlRepository<T> where T : class, IEntity, new()
        {
            public TestSqlRepository(DbContext dbContext) : base(dbContext)
            {
            }

            public override void SaveWorkout()
            {
                throw new Exception("Simulated error in SaveWorkout");
            }
        }

        [Test]
        public void AddWorkout_SqlRepositoryWorkoutAddedEventInvoked()
        {
            // Arrange
            var dbContext = new TestDbContext();
            var workoutRepository = new SqlRepository<Workout>(dbContext);
            var eventInvoked = false;
            workoutRepository.WorkoutAdded += (sender, args) => eventInvoked = true;

            // Act
            workoutRepository.AddWorkout(new Workout());

            // Assert
            Assert.IsTrue(eventInvoked);
        }

        [Test]
        public void AddWorkout_SaveWorkoutExceptionOccurs_SqlRepositoryWorkoutAddedEventNotInvoked()
        {
            // Arrange
            var dbContext = new TestDbContext();
            var workoutRepository = new TestSqlRepository<Workout>(dbContext);
            var eventInvoked = false;
            workoutRepository.WorkoutAdded += (sender, args) => eventInvoked = true;

            // Assert
            Assert.Throws<Exception>(() => workoutRepository.SaveWorkout());
            Assert.IsFalse(eventInvoked);
        }
    }
}
