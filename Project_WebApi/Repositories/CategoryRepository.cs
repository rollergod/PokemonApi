using Project_WebApi.Data;
using Project_WebApi.Interfaces;
using Project_WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Project_WebApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }


        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Pokemon> GetPokemonByCategory(int id)
        {
            return _context.PokemonCategories
                .Where(p => p.CategoryId == id)
                .Select(p => p.Pokemon).ToList();
        }
        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return SaveCategory();
        }

        public bool SaveCategory()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return SaveCategory();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return SaveCategory();
        }
    }
}
