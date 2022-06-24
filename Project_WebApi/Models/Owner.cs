using System.Collections.Generic;

namespace Project_WebApi.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public Country Country { get; set; } // У жителя одна страна
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
