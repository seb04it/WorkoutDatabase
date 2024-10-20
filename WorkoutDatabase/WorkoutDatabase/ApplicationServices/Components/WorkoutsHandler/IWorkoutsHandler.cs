using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDatabase.ApplicationServices.Components.CategoryHandler
{
    public interface IWorkoutsHandler
    {
        string CategoryChoice();
        string FurtherCategoryChoice();
        DateTime DateTimeUpdate();
    }
}
