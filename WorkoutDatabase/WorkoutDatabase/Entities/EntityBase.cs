using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDataBase.Entities
{
    abstract public class EntityBase : IEntity
    {
        public int Id { get; set; }
    }
}
