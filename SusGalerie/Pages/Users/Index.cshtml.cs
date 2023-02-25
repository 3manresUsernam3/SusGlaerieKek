using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Models;

namespace SusGalerie.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public IndexModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<User> User { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                User = await _context.Users.ToListAsync();
            }
        }
    }
}
