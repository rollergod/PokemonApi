using AutoMapper;
using Project_WebApi.Data;
using Project_WebApi.Interfaces;
using Project_WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project_WebApi.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ReviewRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return SaveReview();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return SaveReview();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return SaveReview();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == id);
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
        {
            return _context.Reviews
                .Where(r => r.Pokemon.Id == pokemonId)
                .ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        public bool SaveReview()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return SaveReview();
        }
    }
}
