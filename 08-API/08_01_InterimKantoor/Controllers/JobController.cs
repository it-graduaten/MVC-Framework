using _08_01_InterimKantoor.Data;
using _08_01_InterimKantoor.Models;
using _08_01_InterimKantoor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace _08_01_InterimKantoor.Controllers
{
    public class JobController : Controller
    {
        private readonly IUnitOfWork _context;
        public JobController(IUnitOfWork context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            JobIndexViewModel viewModel = new JobIndexViewModel();
            viewModel.Vacatures = await _context.JobRepository.Search().Include(x => x.KlantJobs).ThenInclude(x => x.Klant).ToListAsync();                 
            return View(viewModel);
        }
        
        public IActionResult Detail(int? id)
        {
            Job Job = _context.JobRepository.Search().Where(k => k.Id == id).Include(k => k.KlantJobs)
                                                .ThenInclude(k => k.Klant)
                                                .First();
            if (Job != null)
            {
                JobDetailViewModel viewModel = new JobDetailViewModel();
                {
                    viewModel.Job = Job;
                };
                return View(viewModel);
            }
            else
            {
                JobIndexViewModel viewModel = new JobIndexViewModel();
                {
                    viewModel.Vacatures = _context.JobRepository.Search().Include(x => x.KlantJobs).ThenInclude(x => x.Klant).ToList();
                };
                return View(viewModel);
            }
        }
 
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _context.JobRepository.AddAsync(new Job()
                {
                    Omschrijving = viewModel.Omschrijving,
                    StartDatum = viewModel.StartDatum,
                    EindDatum = viewModel.EindDatum,
                    Locatie = viewModel.Locatie,
                    IsBadge = viewModel.IsBadge,
                    IsKleding = viewModel.IsKleding,
                    IsWerkschoenen = viewModel.IsWerkschoenen,
                    AantalPlaatsen = viewModel.AantalPlaatsen
                });
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);

        }
        [HttpGet]
        public async Task<IActionResult> Assign(int id)
        {
            var klanten = await _context.KlantRepository.GetAllAsync();
            Job job = _context.JobRepository.Search().Where(x => x.Id == id).Include(k => k.KlantJobs).ThenInclude(k => k.Klant).First();

            JobAssignViewModel viewModel = new()
            {
                JobId = job.Id,
                JobOmschrijving = job.Omschrijving,
                Klanten = klanten.Select(k => new SelectListItem
                    {
                        Value = k.KlantId.ToString(),
                        Text = k.Naam
                    }).ToList()
        };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Assign(JobAssignViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _context.KlantJobRepository.Search().Where(x=> x.JobId == viewModel.JobId && x.KlantId==viewModel.KlantId).ToList();

                if (result.Count == 0)
                {
                    _context.KlantJobRepository.AddAsync(new KlantJob
                    {
                        KlantId = viewModel.KlantId,
                        JobId = viewModel.JobId
                    });

                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ErrorShowViewModel Error = new()
                    {
                        Error = viewModel.KlantId + " is reeds toegewezen aan deze vacature!"
                    };
                    return View("Error",Error);
                }
            }
            return View(viewModel);
         
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var Job = _context.JobRepository.Search().Include(x => x.KlantJobs).ThenInclude(x => x.Klant).FirstOrDefault(x => x.Id == id);
            if (Job == null) return NotFound();

            var klanten = await _context.KlantRepository.GetAllAsync();

            JobEditViewModel viewModel = new JobEditViewModel
            {
                Id = id,
                Omschrijving = Job.Omschrijving,
                StartDatum = Job.StartDatum,
                EindDatum = Job.EindDatum,
                Locatie = Job.Locatie,
                IsBadge = Job.IsBadge,
                IsKleding = Job.IsKleding,
                IsWerkschoenen = Job.IsWerkschoenen,
                AantalPlaatsen = Job.AantalPlaatsen,

                KlantJobs = Job.KlantJobs.Select(kj => new KlantJobListViewModel
                {
                    KlantID = kj.KlantId,
                }).ToList(),

                Klanten = klanten.Select(c => new SelectListItem
                {
                    Value = c.KlantId,
                    Text = c.Naam + " " + c.Voornaam
                })
                .ToList()
        };

            return View(viewModel);
        }


        // Job updaten
        [HttpPost]
        public IActionResult Edit(JobEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var job = _context.JobRepository.Search().Include(b => b.KlantJobs)
                    .FirstOrDefault(b => b.Id == viewModel.Id);

                if (job == null)
                {
                    return NotFound();
                }

                job.Id = viewModel.Id;
                job.Omschrijving = viewModel.Omschrijving;
                job.StartDatum = viewModel.StartDatum;
                job.EindDatum = viewModel.EindDatum;
                job.Locatie = viewModel.Locatie;
                job.IsBadge = viewModel.IsBadge;
                job.IsKleding = viewModel.IsKleding;
                job.IsWerkschoenen = viewModel.IsWerkschoenen;
                job.AantalPlaatsen = viewModel.AantalPlaatsen;

                //Hou bij welke KlantID's verwijderd zijn.
                var removedKlantIDs = job.KlantJobs
                    .Where(existingKlantJob => !viewModel.KlantJobs.Any(newKlantJob => newKlantJob.KlantID == existingKlantJob.KlantId))
                    .Select(existingKlantJob => existingKlantJob.KlantId)
                    .ToList();

                var existingKlantIds = new HashSet<string>();

                foreach (var viewModelKlantJob in viewModel.KlantJobs)
                    {
                        if (existingKlantIds.Contains(viewModelKlantJob.KlantID))
                        {
                            ModelState.AddModelError(string.Empty, "Er zijn duplicaten in de klantjobs.");
                            viewModel.KlantJobs = _context.KlantJobRepository.Search()
                                .Where(kj => kj.JobId == viewModel.Id)
                                .Select(kj => new KlantJobListViewModel
                                {
                                    KlantID = kj.KlantId
                                }).ToList();

                            viewModel.Klanten = _context.KlantRepository.Search()
                                .Select(k => new SelectListItem
                                {
                                    Value = k.KlantId,
                                    Text = k.Naam + " " + k.Voornaam
                                }).ToList();

                            return View(viewModel);
                       }

                        existingKlantIds.Add(viewModelKlantJob.KlantID);

                        var klantjob = job.KlantJobs
                            .FirstOrDefault(kj => kj.KlantId == viewModelKlantJob.KlantID);

                        if (klantjob != null)
                        {
                            klantjob.KlantId = viewModelKlantJob.KlantID;
                        }
                        else
                        {
                            KlantJob klantjobnieuw = new()
                            {
                                JobId = job.Id,
                                KlantId = viewModelKlantJob.KlantID
                            };
                        job.KlantJobs.Add(klantjobnieuw);
                        }

                    }
                // Verwijder de klantIDs die niet langer aan de job zijn toegewezen
                foreach (var removedKlantID in removedKlantIDs)
                {
                    var klantjobToRemove = job.KlantJobs.FirstOrDefault(kj => kj.KlantId == removedKlantID);
                    if (klantjobToRemove != null)
                    {
                        job.KlantJobs.Remove(klantjobToRemove);
                    }
                }

                _context.JobRepository.Update(job);
                _context.SaveChanges();

                    return RedirectToAction("index");
                }

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Job = await _context.JobRepository.GetByIdAsync(id);
            if (Job == null)
            {
                return NotFound();
            }

            JobDeleteViewModel viewModel = new JobDeleteViewModel()
            {
                JobId = Job.Id,
                Omschrijving = Job.Omschrijving
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.JobRepository.GetByIdAsync(id);

            if(job == null)
            {
                return NotFound();
            }
            _context.JobRepository.Delete(job);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        

    }
}
