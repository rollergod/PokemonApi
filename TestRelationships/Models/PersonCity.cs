using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRelationships.Models
{
    public class PersonCity // Many TO many table
    {
        public int PersonId { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public Person Person { get; set; }
    }
}
