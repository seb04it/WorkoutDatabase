
namespace WorkoutDatabase.DataAccess.Data.Entities
{
    public class WorkoutCategoryEntity : EntityBase
    {
        public string CategoryName { get; set; }

        public override string ToString() => $"Id {Id}, " +
            $"WorkoutCategory: {CategoryName}";
    }
}
