using Microsoft.AspNetCore.Mvc;
using Project_WebApi.Interfaces;
using Project_WebApi.Models;
using System.Collections.Generic;

namespace Project_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        public IPokemonRepository _pokemonRepository { get; }
        public PokemonController(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

    }
}
