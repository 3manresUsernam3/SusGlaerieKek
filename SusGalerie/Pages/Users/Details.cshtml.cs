using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Models;

namespace SusGalerie.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public DetailsModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
