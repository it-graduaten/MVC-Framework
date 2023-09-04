// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using _09_01_InterimKantoor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _09_01_InterimKantoor.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public IndexModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [PersonalData]
            [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
            public string Voornaam { get; set; }

            [PersonalData]
            [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
            public string Achternaam { get; set; }

            [PersonalData]
            public Geslacht GeslachtSelect { get; set; }

            [PersonalData]
            [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
            public string Straat { get; set; }

            [PersonalData]
            public int? Huisnummer { get; set; }

            [PersonalData]
            public string Postcode { get; set; }

            [PersonalData]
            [MaxLength(50, ErrorMessage = "De ingevulde naam is te lang. Maximale lengte is 50.")]
            public string Gemeente { get; set; }

            [PersonalData]
            public DateTime Geboortedatum { get; set; }
        }

        private async Task LoadAsync(CustomUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var Achternaam = await Task.FromResult(user.Achternaam);
            var Voornaam = await Task.FromResult(user.Voornaam);
            var Geboortedatum = await Task.FromResult(user.Geboortedatum);
            var GeslachtSelect = await Task.FromResult(user.GeslachtSelect);
            var Straat = await Task.FromResult(user.Straat);
            var Huisnummer = await Task.FromResult(user.Huisnummer);
            var Postcode = await Task.FromResult(user.Postcode);
            var Gemeente = await Task.FromResult(user.Gemeente);

            DateTime Datum = Geboortedatum ?? new DateTime(1970, 1, 1);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Achternaam = Achternaam,
                Voornaam = Voornaam,
                Geboortedatum = Datum,
                GeslachtSelect = GeslachtSelect,
                Straat = Straat,
                Huisnummer = Huisnummer,
                Postcode = Postcode,
                Gemeente = Gemeente
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.Achternaam = Input.Achternaam;
            user.Voornaam = Input.Voornaam;
            user.Geboortedatum = Input.Geboortedatum;
            user.GeslachtSelect = Input.GeslachtSelect;
            user.Straat = Input.Straat;
            user.Huisnummer = Input.Huisnummer;
            user.Postcode = Input.Postcode;
            user.Gemeente = Input.Gemeente;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
