using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SusGalerie.Data;
using SusGalerie.Models;

namespace SusGalerie.Pages.Galleries
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public CreateModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ImageCollection Gallery { get; set; } = new ImageCollection();

        public IActionResult OnGet(string id)
        {
            Gallery.UserId = id;
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            _context.Galleries.Add(Gallery);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
