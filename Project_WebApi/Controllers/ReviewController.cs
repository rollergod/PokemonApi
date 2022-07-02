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
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;

        public ReviewController(IReviewRepository reviewRepository,
                                IMapper mapper,
                                IReviewerRepository reviewerRepository,
                                IPokemonRepository pokemonRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(review);
        }

        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int pokemonId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokemonId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest();

            var review = _reviewRepository.GetReviews()
                .Where(r => r.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "Review уже существует");
                return StatusCode(422, ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
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
        public IActionResult UpdateReview(int id, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (id != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return NotFound();

            var reviewToDelete = _reviewRepository.GetReview(id);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReview(reviewToDelete))
                ModelState.AddModelError("", "Что-то пошло не так во время удаления");

            return NoContent();
        }
    }
}
