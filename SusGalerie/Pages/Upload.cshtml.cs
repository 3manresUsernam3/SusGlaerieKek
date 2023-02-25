using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using SusGalerie.Models;
using SusGalerie.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace SusGalerie.Pages
{
    [Authorize]
    public class UploadModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private int _squareSize;
        private int _sameAspectRatioHeight;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public ICollection<IFormFile> Upload { get; set; }
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public StoredFile StoredFile { get; set; } = default!;

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public bool IsPublic { get; set; }

        public ImageCollection ImageCollection { get; set; } = new ImageCollection();

        public Guid ImageCollectionId { get; set; }
        public Guid UserId { get; set; }

        public UploadModel(IWebHostEnvironment environment, ApplicationDbContext context, IConfiguration configuration)
        {
            _environment = environment;
            _context = context;
            _configuration = configuration;
            if (Int32.TryParse(_configuration["Thumbnails:SquareSize"], out _squareSize) == false) _squareSize = 250;
            if (Int32.TryParse(_configuration["Thumbnails:SameAspectRatioHeight"], out _sameAspectRatioHeight) == false) _sameAspectRatioHeight = 2000;
        }

        public void OnGet(Guid id)
        {
            ImageCollectionId = id;
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            UserId = Guid.Parse(userId);
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            ImageCollectionId = id;
            return RedirectToPage("/Images", new { id = ImageCollectionId });
        }
    }
}