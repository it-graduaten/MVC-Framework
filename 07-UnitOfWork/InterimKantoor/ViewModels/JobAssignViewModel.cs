using InterimKantoor.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace InterimKantoor.ViewModels
{
    public class JobAssignViewModel
    {
        public string KlantId { get; set; } = default!;
        public int JobId { get; set; } 
        public List<SelectListItem> Klanten { get; set; } = new List<SelectListItem>();
        public string JobOmschrijving { get; set; } = default!;
    }
}
