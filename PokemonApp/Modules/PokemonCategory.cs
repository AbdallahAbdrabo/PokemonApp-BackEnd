﻿namespace PokemonApp.Modules
{
    public class PokemonCategory
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }

        public Pokemon? Pokemon { get; set; }

        public Category? category { get; set; }
    }
}
