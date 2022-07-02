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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(countries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryById(int id)
        {
            if (!_countryRepository.CountryExists(id))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        //[HttpGet("{id}")]
        //[ProducesResponseType(200,Type = typeof(IEnumerable<Owner>))]
        //[ProducesResponseType(400)]
        //public IActionResult GetOwnersFromCountry(int id)
        //{
        //    if (!_countryRepository.CountryExists(id))
        //        return NotFound();

        //    var owners = _mapper.Map<CountryDto>(_countryRepository.GetOwnersFromCountry(id));

        //    if (!ModelState.IsValid)
        //        return BadRequest();

        //    return Ok(owners);
        //}

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry(CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest();

            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
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
        public IActionResult UpdateCountry(int id, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (id != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_countryRepository.UpdateCountry(countryMap))
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
        public IActionResult DeleteCountry(int id)
        {
            if (!_countryRepository.CountryExists(id))
                return NotFound();

            var countryToDelete = _countryRepository.GetCountry(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countryToDelete))
                ModelState.AddModelError("", "Что-то пошло не так во время удаления");

            return NoContent();
        }


    }
}
