using WorkoutDatabase.ApplicationServices.Repositories;
using WorkoutDatabase.DataAccess.Data.Entities;


namespace WorkoutDatabase.Tests
{
    public class JsonRepositoryTests
    {
        public class TestJsonRepository<T> : JsonRepository<T> where T : class, IEntity, new()
        {
            public TestJsonRepository(string jsonFilePath) : base(jsonFilePath) { }

            public override void SaveItem()
            {
                throw new Exception("Simulated error in SaveItem");
            }

        }

        [Test]
        public void AddItem_JsonRepositoryWorkoutAddedEventInvoked()
        {
            var jsonFilePath = "test.json";
            File.WriteAllText(jsonFilePath, "[]");

            var repository = new JsonRepository<WorkoutEntity>(jsonFilePath);
            var eventInvoked = false;

            repository.ItemAdded += (sender, args) => eventInvoked = true;

            var newItem = new WorkoutEntity { SongName = "Test Workout" };

            repository.AddItem(newItem);

            Assert.That(eventInvoked, Is.True);
        }

        [Test]

        public void AddItem_SaveItemExceptionOccurs_JsonRepositoryWorkoutAddedEventNotInvoked()
        {
            var jsonFilePath = "test.json";
            File.WriteAllText(jsonFilePath, "[]");

            var repository = new TestJsonRepository<WorkoutEntity>(jsonFilePath);
            var eventInvoked = false;
            repository.ItemAdded += (sender, args) => eventInvoked = true;

            var newItem = new WorkoutEntity { SongName = "Test Workout" };

            Assert.Throws<Exception>(() => repository.AddItem(newItem));
            Assert.That(eventInvoked, Is.False);
        }
    }
}