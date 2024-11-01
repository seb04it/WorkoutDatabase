﻿
namespace WorkoutDatabase.DataAccess.Data.Entities
{
    public class WorkoutEntity : EntityBase
    {
        public string? WorkoutCategory { get; set; }
        public string? SongName { get; set; }
        public string? ArtistName { get; set; }
        public TimeSpan? WorkoutLength { get; set; }
        public DateTime? LastUsed { get; set; }

        public override string ToString() => $"Id {Id}, " +
            $"WorkoutCategory: {WorkoutCategory}, " +
            $"Song: {SongName}, " +
            $"Artist: {ArtistName}, " +
            $"Workout Lenght: {WorkoutLength?.ToString("mm\\:ss")}, " +
            $"Last Used: {LastUsed?.ToString("dd\\.MM\\.yyyy HH\\:mm\\:ss")}";
    }
}
