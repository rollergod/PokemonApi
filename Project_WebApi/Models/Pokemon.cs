using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Project_WebApi.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        //[JsonIgnore] - Атрибут JsonIgnore позволяет исключить из сериализации определенное свойство
        public ICollection<Review> Reviews { get; set; } // У покемона может быть много обзоров.
        public ICollection<PokemonOwner> PokemonOwners { get; set; } // У покемона может быть много владельцев
        public ICollection<PokemonCategory> PokemonCategories { get; set; }

    }
}
