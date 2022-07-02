using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_WebApi.Dto;
using Project_WebApi.Interfaces;
using Project_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviewers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int id)
        {
            if (!_reviewerRepository.ReviewerExists(id))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(id));
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviewer);
        }

        [HttpGet("reviews/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer(ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest();

            var reviewer = _reviewerRepository.GetReviewers()
                .Where(r => r.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer уже существует");
                return StatusCode(422, ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
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
        public IActionResult UpdateReviewer(int id, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (id != updatedReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(id))
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);

            if(!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время обновления");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int id)
        {
            if (!_reviewerRepository.ReviewerExists(id))
                return NotFound();

            var reviewerToDelete = _reviewerRepository.GetReviewer(id);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
                ModelState.AddModelError("", "Что-то пошло нге так во время удаления");

            return NoContent();
        }
    }
}
