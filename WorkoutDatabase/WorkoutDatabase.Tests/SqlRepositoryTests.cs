
using Microsoft.EntityFrameworkCore;
using WorkoutDatabase.ApplicationServices.Repositories;
using WorkoutDatabase.DataAccess.Data.Entities;
using WorkoutDatabase.DataAccess.Data;

namespace WorkoutDatabase.Tests
{
    public class TestSqlRepository<T> : SqlRepository<T> where T : class, IEntity, new()
    {
        public TestSqlRepository(WorkoutDbContext dbContext) : base(dbContext) { }

        public override void SaveItem()
        {
            throw new Exception("Simulated error in SaveItem");
        }
    }

    [TestFixture]
    public class SqlRepositoryTests
    {
        private WorkoutDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<WorkoutDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            return new WorkoutDbContext(options);
        }

        [Test]
        public void AddItem_SaveItemExceptionOccurs_SqlRepositoryItemAddedEventNotInvoked()
        {
            // Arrange
            using var dbContext = CreateInMemoryDbContext();
            var repository = new TestSqlRepository<WorkoutEntity>(dbContext);
            var eventInvoked = false;
            repository.ItemAdded += (sender, args) => eventInvoked = true;

            var newItem = new WorkoutEntity { SongName = "Test Workout" };

            // Act & Assert
            Assert.Throws<Exception>(() => repository.AddItem(newItem));
            Assert.That(eventInvoked, Is.False);
        }
    }
}