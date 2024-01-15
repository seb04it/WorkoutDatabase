using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Repositories
{
    public class WorkoutRepository
    {
        private readonly List<Workout> _workouts = new();

        public void Add(Workout workout)
        {
            workout.Id = _workouts.Count + 1;
            _workouts.Add(workout);
        }

        public void Save()
        {
            foreach(var workout in _workouts)
            {
                Console.WriteLine(workout);
            }
        }

        public Workout GetById(int id)
        {
            return _workouts.Single(item => item.Id == id);
        }
    }
}
