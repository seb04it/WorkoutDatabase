using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.Entities;

namespace WorkoutDatabase.Data.Entities
{
    public class SongEntities : EntityBase
    {
        public string? SongName { get; set; }
        public string? ArtistName { get; set; }
        public TimeSpan? SongLength { get; set; }
        public DateTime? LastUsed { get; set; }

        public override string ToString() => $"Id {Id}, " +
            $"Song: {SongName}, " +
            $"Artist: {ArtistName}, " +
            $"Workout Lenght: {SongLength?.ToString("mm\\:ss")}, " +
            $"Last Used: {LastUsed?.ToString("dd\\.MM\\.yyyy HH\\:mm\\:ss")}";
    }
}
