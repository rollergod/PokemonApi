using System.Collections.Generic;

namespace Project_WebApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; } // У одной категории может быть много покемонов, у одного покемона может быть много категорий
    }
}
