using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace _09_01_InterimKantoor.Models
{
    public class Klant
    {
        public string KlantId { get; set; } = default!;
        [PersonalData]
        public string Naam { get; set; } = default!;
        [PersonalData]
        public string Voornaam { get; set; } = default!;
        [PersonalData]
        public string Gemeente { get; set; } = default!;
        [PersonalData]
        public string Postcode { get; set; } = default!;
        [PersonalData]
        public string Straat { get; set; } = default!;
        [PersonalData]
        public string Huisnummer { get; set; } = default!;
        [PersonalData]
        public string Bankrekeningnummer { get; set; } = default!;
        [JsonIgnore]
        public IList<KlantJob> klantjobs { get; set; } = new List<KlantJob>();

    }
}
