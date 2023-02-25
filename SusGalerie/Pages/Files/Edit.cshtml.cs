using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Data;
using SusGalerie.Models;

namespace SusGalerie.Pages.Files
{
    public class EditModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public EditModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StoredFile StoredFile { get; set; } = default!;

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
            StoredFile = storedfile;
            ViewData["GalleryId"] = new SelectList(_context.Galleries, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(StoredFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoredFileExists(StoredFile.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Images", new { id = StoredFile.GalleryId });
        }

        private bool StoredFileExists(Guid id)
        {
            return _context.Files.Any(e => e.Id == id);
        }
    }
}
