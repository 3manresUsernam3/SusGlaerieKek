using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Data;
using SusGalerie.Models;

namespace SusGalerie.Pages.Files
{
    public class DeleteModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public DeleteModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StoredFile StoredFile { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Files == null)
            {
                return NotFound();
            }

            var storedfile = await _context.Files.FirstOrDefaultAsync(m => m.Id == id);

            if (storedfile == null)
            {
                return NotFound();
            }
            else
            {
                StoredFile = storedfile;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Files == null)
            {
                return NotFound();
            }
            var storedfile = await _context.Files.FindAsync(id);

            if (storedfile != null)
            {
                StoredFile = storedfile;
                _context.Files.Remove(StoredFile);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Images", new { id = StoredFile.GalleryId });
        }
    }
}
