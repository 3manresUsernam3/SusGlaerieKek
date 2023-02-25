using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Data;
using SusGalerie.Models;

namespace SusGalerie.Pages.Galleries
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public DeleteModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ImageCollection Gallery { get; set; }

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
            else
            {
                Gallery = gallery;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Galleries == null)
            {
                return NotFound();
            }
            var gallery = await _context.Galleries.FindAsync(id);

            if (gallery != null)
            {
                Gallery = gallery;
                _context.Galleries.Remove(Gallery);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
