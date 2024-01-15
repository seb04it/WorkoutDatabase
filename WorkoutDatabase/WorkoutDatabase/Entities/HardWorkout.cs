namespace WorkoutDatabase.Entities
{
    public class HardWorkout : Workout
    {
        public override string ToString() => base.ToString() + " (HARD)";
    }
}
