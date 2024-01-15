using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDatabase.Entities
{
    public class Workout : EntityBase
    {
        public string? WorkoutCategory { get; set; }
        public string? SongName { get; set; }
        public string? WorkoutLenght { get; set; }
        public string? LastUsed { get; set; }
        public int? TimesUsed { get; set; }

        public override string ToString() => $"Id {Id}, " +
            $"WorkoutCategory: {WorkoutCategory}, " +
            $"Song: {SongName}, " +
            $"Workout Lenght: {WorkoutLenght}, " +
            $"Last Used: {LastUsed}, " +
            $"Times Used: {TimesUsed}\t";
    }
}
