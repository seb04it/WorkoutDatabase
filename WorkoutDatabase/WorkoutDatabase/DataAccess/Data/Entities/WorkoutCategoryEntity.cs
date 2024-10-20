using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDatabase.DataAccess.Data.Entities
{
    public class WorkoutCategoryEntity : EntityBase
    {
        public string CategoryName { get; set; }

        public override string ToString() => $"Id {Id}, " +
            $"WorkoutCategory: {CategoryName}";
    }
}
