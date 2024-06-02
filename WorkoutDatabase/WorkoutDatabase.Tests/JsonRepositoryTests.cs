//using WorkoutDatabase.Data.Repositories;
//using WorkoutDatabase.Entities;

//namespace WorkoutDataase.Tests
//{
//    public class JsonRepositoryTests
//    {
//        public class TestJsonRepository<T> : JsonRepository<T> where T : class, IEntity, new()
//        {
//            public override void SaveWorkout()
//            {
//                throw new Exception("Simulated error in SaveWorkout");
//            }
//        }

//        [Test]
//        public void AddWorkout_JsonRepositoryWorkoutAddedEventInvoked()
//        {
//            // Arrange
//            var repository = new JsonRepository<WorkoutEntities>();
//            var eventInvoked = false;
//            repository.WorkoutAdded += (sender, args) => eventInvoked = true;

//            // Act
//            repository.AddWorkout(new WorkoutEntities());

//            // Assert
//            Assert.IsTrue(eventInvoked);
//        }

//        [Test]

//        public void AddWorkout_SaveWorkoutExceptionOccurs_JsonRepositoryWorkoutAddedEventNotInvoked()
//        {
//            var workoutRepository = new TestJsonRepository<WorkoutEntities>();
//            var eventInvoked = false;
//            workoutRepository.WorkoutAdded += (sender, args) => eventInvoked = true;

//            //Assert
//            Assert.Throws<Exception>(() => workoutRepository.SaveWorkout());
//            Assert.IsFalse(eventInvoked);
//        }
//    }
//}