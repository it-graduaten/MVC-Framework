using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _09_01_InterimKantoor.Data;
using _09_01_InterimKantoor.Models;
using _09_01_InterimKantoor.ViewModels;

namespace _09_01_InterimKantoor.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public JobController(IUnitOfWork context)
        {
            _context = context;
        }

        
        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
          if (_context.JobRepository == null)
          {
              return NotFound();
          }
            var jobs = await _context.JobRepository.Search().Include(x=>x.KlantJobs).ThenInclude(x=>x.Klant).ToListAsync();
            return Ok(jobs);
        }
        
        // GET: api/Job/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
          if (_context.JobRepository == null)
          {
              return NotFound();
          }
            var job = await _context.JobRepository.GetByIdAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }
        
        // PUT: api/Job/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutJob(int id, Job job)
        {
            if (id != job.Id)
            {
                return BadRequest();
            }

            _context.JobRepository.Update(job);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
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

        //POST
        [HttpPost("Assign")]
        public async Task<ActionResult<KlantJob>> Assign(KlantJob klantjob)
        {
            if (_context.KlantJobRepository == null)
            {
                return Problem("Entity set 'InterimKantoorContext.KlantJobs'  is null.");
            }

            var result = _context.KlantJobRepository.Search().Where(x => x.JobId == klantjob.JobId && x.KlantId == klantjob.KlantId).ToList();

            if (result.Count != 0)
            {
                return Problem("Klant is al toegewezen aan deze job.");
            }

            await _context.KlantJobRepository.AddAsync(klantjob);
            _context.SaveChanges();

            return CreatedAtAction("GetJob", new { id = klantjob.Id }, klantjob);

        }

        // POST: api/Job
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        public async Task<ActionResult<Job>> PostJob(Job job)
        {
          if (_context.JobRepository == null)
          {
              return Problem("Entity set 'InterimKantoorContext.Jobs'  is null.");
          }
            await _context.JobRepository.AddAsync(job);
            _context.SaveChanges();

            return CreatedAtAction("GetJob", new { id = job.Id }, job);
        }

        // DELETE: api/Job/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            if (_context.JobRepository == null)
            {
                return NotFound();
            }
            var job = await _context.JobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.JobRepository.Delete(job);
            _context.SaveChanges();

            return NoContent();
        }

        private bool JobExists(int id)
        {
            return (_context.JobRepository.GetByIdAsync(id).IsCompletedSuccessfully);
        }
        
    }
}
