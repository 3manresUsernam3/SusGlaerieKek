using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SusGalerie.Models;
using System.ComponentModel.DataAnnotations;

namespace SusGalerie.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly SusGalerie.Data.ApplicationDbContext _context;

        public EditModel(SusGalerie.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

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
            User = user;
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
            User.UserName = User.Email;
            User.NormalizedEmail = User.Email.ToUpper();
            User.NormalizedUserName = User.UserName.ToUpper();
            if ((Input.NewPassword != null || Input.NewPassword != "") && Input.NewPassword.Equals(Input.ConfirmPassword))
            {
                User.PasswordHash = new PasswordHasher<User>().HashPassword(User, Input.NewPassword);
            }
            _context.Attach(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
