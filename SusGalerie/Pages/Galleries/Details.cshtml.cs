using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Data;
using SusGalerie.Models;

namespace SusGalerie.Pages.Galleries
{
    public class DetailsModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public DetailsModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public ImageCollection ImageCollection { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Galleries == null)
            {
                return NotFound();
            }

            var imagecollection = await _context.Galleries.FirstOrDefaultAsync(m => m.Id == id);
            if (imagecollection == null)
            {
                return NotFound();
            }
            else 
            {
                ImageCollection = imagecollection;
            }
            return Page();
        }
    }
}
