﻿namespace MinimalAPIsMovies2.DTOs
{
    public class CreateActorDTO
    {
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
