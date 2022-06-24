using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRelationships.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Book Book { get; set; } // У одного человека одна книга
        //public int CityId { get; set; }
        //public City City { get; set; }
    }
}
