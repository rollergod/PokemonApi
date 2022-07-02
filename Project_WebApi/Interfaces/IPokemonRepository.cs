using Project_WebApi.Models;
using System.Collections.Generic;

namespace Project_WebApi.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string Name);
        decimal GetPokemonRating(int Id);
        bool PokemonExists(int id);
        bool CreatePokemon(int ownerId,int categoryId,Pokemon pokemon);
        bool UpdatePokemon(int ownerId,int categoryId,Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool SavePokemon();
    }
}
