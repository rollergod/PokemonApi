using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_WebApi.Dto;
using Project_WebApi.Interfaces;
using Project_WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        public IPokemonRepository _pokemonRepository { get; }
        public IReviewRepository _reviewRepository { get; }
        public IMapper _mapper { get; }

        public PokemonController(IPokemonRepository pokemonRepository,
                                 IReviewRepository reviewRepository,
                                 IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
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
        [ProducesResponseType(200, Type = typeof(Pokemon))]
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
        [ProducesResponseType(200, Type = typeof(Pokemon))]
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreated)
        {
            if (pokemonCreated == null)
                return BadRequest();

            var pokemon = _pokemonRepository.GetPokemons()
                .Where(p => p.Name.Trim().ToUpper() == pokemonCreated.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon уже существует");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreated);

            if (!_pokemonRepository.CreatePokemon(ownerId,categoryId,pokemonMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время сохранения");
                return StatusCode(500, ModelState);
            }

            return Ok("Создано успешно");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int id,
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (id != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if(!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время обновления");
                return StatusCode(500,ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();

            var reviewsToDelete = _reviewRepository.GetReviewsOfAPokemon(id);

            var pokemonToDelete = _pokemonRepository.GetPokemon(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
                ModelState.AddModelError("", "Что-то пошло не так во время удаления reviews");


            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
                ModelState.AddModelError("", "Что-то пошло не так во время удаления pokemon");

            return NoContent();
        }
    }
}
