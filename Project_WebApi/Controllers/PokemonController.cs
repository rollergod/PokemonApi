using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_WebApi.Dto;
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
        public IMapper _mapper { get; }

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemon);
        }

        //[HttpGet("{name}")]
        //[ProducesResponseType(400,Type = typeof(Pokemon))]
        //[ProducesResponseType(400)]
        //public IActionResult GetPokemon(string name)
        //{
        //    if (name.Length == 0 || string.IsNullOrEmpty(null))
        //        return BadRequest();

        //    var pokemon = _pokemonRepository.GetPokemon(name);

        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    return Ok(pokemon);
        //}

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200,Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(id);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);
        }

    }
}
