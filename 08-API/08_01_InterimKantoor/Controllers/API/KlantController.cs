using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _08_01_InterimKantoor.Data;
using _08_01_InterimKantoor.Models;

namespace _08_01_InterimKantoor.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public KlantController(IUnitOfWork context)
        {
            _context = context;
        }

        // GET: api/Klant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Klant>>> GetKlanten()
        {
          if (_context.KlantRepository == null)
          {
              return NotFound();
          }
          var jobs = await _context.KlantRepository.GetAllAsync();
            return Ok(jobs);
        }

        // GET: api/Klant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Klant>> GetKlant(string id)
        {
          if (_context.KlantRepository == null)
          {
              return NotFound();
          }
            var klant = await _context.KlantRepository.GetByIdAsync(id);

            if (klant == null)
            {
                return NotFound();
            }

            return klant;
        }

        // PUT: api/Klant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutKlant(string id, Klant klant)
        {
            if (id != klant.KlantId)
            {
                return BadRequest();
            }

            _context.KlantRepository.Update(klant);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KlantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Klant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Klant>> PostKlant(Klant klant)
        {
          if (_context.KlantRepository == null)
          {
              return Problem("Entity set 'InterimKantoorContext.Klanten'  is null.");
          }
            await _context.KlantRepository.AddAsync(klant);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (KlantExists(klant.KlantId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKlant", new { id = klant.KlantId }, klant);
        }

        // DELETE: api/Klant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKlant(string id)
        {
            if (_context.KlantRepository == null)
            {
                return NotFound();
            }
            var klant = await _context.KlantRepository.GetByIdAsync(id);
            if (klant == null)
            {
                return NotFound();
            }

            _context.KlantRepository.Delete(klant);
            _context.SaveChanges();

            return NoContent();
        }

        private bool KlantExists(string id)
        {
            return (_context.KlantRepository.GetByIdAsync(id).IsCompletedSuccessfully);
        }
    }
}
