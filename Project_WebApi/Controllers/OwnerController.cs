﻿using AutoMapper;
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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository repository, IMapper mapper)
        {
            _ownerRepository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owners);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerById(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemons")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByOwnerId(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

    }
}