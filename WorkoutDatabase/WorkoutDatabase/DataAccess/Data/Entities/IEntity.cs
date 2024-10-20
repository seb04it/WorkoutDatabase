using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDatabase.DataAccess.Data.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
    }
}
