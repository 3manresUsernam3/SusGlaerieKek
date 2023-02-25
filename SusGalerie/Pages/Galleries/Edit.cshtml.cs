using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Data;
using SusGalerie.Models;

namespace SusGalerie.Pages.Galleries
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public EditModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ImageCollection Gallery { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Galleries == null)
            {
                return NotFound();
            }

            var gallery = await _context.Galleries.FirstOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }
            Gallery = gallery;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            _context.Attach(Gallery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GalleryExists(Gallery.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Index");
        }

        private bool GalleryExists(Guid id)
        {
            return _context.Galleries.Any(e => e.Id == id);
        }
    }
}
