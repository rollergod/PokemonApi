using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRelationships.Models;

namespace TestRelationships
{
    public class Seed
    {
        public AppDbContext _context { get; set; }
        public Seed(AppDbContext context)
        {
            _context = context;
        }

        public void AddData()
        {
            Person person = new Person { Name = "Andrei" };
            if (!_context.Books.Any())
            {
                var books = new List<Book>()
                {
                    new Book()
                    {
                        BookName = "Test",
                        PeopleBooks = new List<Person>()
                        {
                            new Person() {Name = "Renat"},
                            new Person() {Name = "Abdul"},
                        }
                    },new Book()
                    {
                        BookName = "Test2",
                        PeopleBooks = new List<Person>()
                        {
                            new Person() {Name = "Maka"},
                            new Person() {Name = "Akam"},
                        }
                    },new Book()
                    {
                        BookName = "Mishka",
                        PeopleBooks = new List<Person>()
                        {
                            person,
                             new Person() {Name = "Eliza"},
                        }
                    },new Book()
                    {
                        BookName = "Volshki",
                        PeopleBooks = new List<Person>()
                        {
                            person,
                             new Person() {Name = "Fedor"},
                        }
                    }
                };
                _context.Books.AddRange(books);
                _context.SaveChanges();
            }
        }
    }
}
