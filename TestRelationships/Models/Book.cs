using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRelationships.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        /// <summary>
        /// Владельцы книг
        /// </summary>
        public ICollection<Person> PeopleBooks { get; set; } // У одной книги может быть много владельцев
    }
}
