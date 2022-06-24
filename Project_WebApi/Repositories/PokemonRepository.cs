using Project_WebApi.Data;
using Project_WebApi.Interfaces;
using Project_WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project_WebApi.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly AppDbContext context;

        public PokemonRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Pokemon GetPokemon(int id)
        {
            return context.Pokemons.FirstOrDefault(pokemon => pokemon.Id == id);
        }

        public Pokemon GetPokemon(string Name)
        {
            return context.Pokemons.FirstOrDefault(pokemon => pokemon.Name == Name);
        }

        public decimal GetPokemonRating(int Id)
        {
            var review = context.Reviews.Where(pokemon => pokemon.Pokemon.Id == Id);

            if (review.Count() <= 0)
                return 0;

            return (decimal)review.Sum(p => p.Rating) / review.Count();
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return context.Pokemons.OrderBy(x => x.Id).ToList();
        }

        public bool PokemonExists(int id)
        {
            return context.Pokemons.Any(p => p.Id == id);
        }
    }
}
