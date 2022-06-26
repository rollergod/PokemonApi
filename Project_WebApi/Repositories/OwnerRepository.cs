﻿using AutoMapper;
using Project_WebApi.Data;
using Project_WebApi.Interfaces;
using Project_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WebApi.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OwnerRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Owner GetOwner(int id)
        {
            return _context.Owners.FirstOrDefault(o => o.Id == id);
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokemonId)
        {
            return _context.PokemonOwners
                .Where(o => o.Pokemon.Id == pokemonId)
                .Select(o => o.Owner)
                .ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners
                .Where(o => o.Owner.Id == ownerId)
                .Select(p => p.Pokemon)
                .ToList();
        }

        public bool OwnerExists(int id)
        {
            return _context.Owners.Any(o => o.Id == id);
        }
    }
}