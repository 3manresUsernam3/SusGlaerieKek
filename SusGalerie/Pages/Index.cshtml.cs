using SusGalerie.Data;
using SusGalerie.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SusGalerie.Pages
{
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public List<StoredFileListViewModel> Files { get; set; } = new List<StoredFileListViewModel>();
        public List<StoredFileListViewModel> PublicFiles { get; set; } = new List<StoredFileListViewModel>();
        public List<ImageCollection> Galleries { get; set; } = new List<ImageCollection>();

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment, ApplicationDbContext context, UserManager<User> userManager)
        {
            _environment = environment;
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public string UserId { get; set; }

        public void OnGet()
        {
            UserId = _userManager.GetUserId(HttpContext.User);

            Files = _context.Files.AsNoTracking().Include(f => f.Uploader).Include(f => f.Thumbnails).Select(f => new StoredFileListViewModel
            {
                Id = f.Id,
                ContentType = f.ContentType,
                OriginalName = f.OriginalName,
                UploaderId = f.UploaderId,
                Uploader = f.Uploader,
                UploadedAt = f.UploadedAt,
                ThumbnailCount = f.Thumbnails.Count,
                Columns = f.Columns,
                Rows = f.Rows,
                Public = f.Public,
                Description = f.Description,
                GalleryId = f.GalleryId,
                Gallery = f.Gallery
            }).OrderByDescending(e => e.UploadedAt).ToList();

            Galleries = _context.Galleries.AsNoTracking().OrderByDescending(e => e.Created).ToList();
        }
        public IActionResult OnGetDownload(string filename)
        {
            filename = Path.GetFileName(filename);
            var fullName = Path.Combine(_environment.ContentRootPath, "Uploads", filename);
            if (System.IO.File.Exists(fullName)) // existuje soubor na disku?
            {
                var fileRecord = _context.Files.Find(Guid.Parse(filename));
                if (fileRecord != null) // je soubor v databázi?
                {
                    return PhysicalFile(fullName, fileRecord.ContentType, fileRecord.OriginalName);
                    // vrať ho zpátky pod původním názvem a typem
                }
                else
                {
                    ErrorMessage = "There is no record of such file.";
                    return RedirectToPage();
                }
            }
            else
            {
                ErrorMessage = "There is no such file.";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnGetThumbnail(string filename, ThumbnailType type = ThumbnailType.Square)
        {
            StoredFile file = await _context.Files.AsNoTracking().Where(f => f.Id == Guid.Parse(filename)).SingleOrDefaultAsync();
            if (file == null)
            {
                return NotFound("no record for this file");
            }
            Thumbnail thumbnail = await _context.Thumbnails.AsNoTracking().Where(t => t.FileId == Guid.Parse(filename) && t.Type == type).SingleOrDefaultAsync();
            if (thumbnail != null)
            {
                return File(thumbnail.Blob, file.ContentType);
            }
            return NotFound("no thumbnail for this file");
        }
    }

    public class StoredFileListViewModel
    {
        public Guid Id { get; set; }
        public User Uploader { get; set; }
        public string UploaderId { get; set; }
        public DateTime UploadedAt { get; set; }
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool Public { get; set; }
        public int ThumbnailCount { get; set; }
        public Guid GalleryId { get; set; }
        public ImageCollection Gallery { get; set; } = new ImageCollection();
    }
}
