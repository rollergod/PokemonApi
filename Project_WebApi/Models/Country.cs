using System.Collections.Generic;

namespace Project_WebApi.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Owner> Owners { get; set; } // У одной страны много жителей
    }
}
