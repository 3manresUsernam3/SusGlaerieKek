using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SusGalerie.Data;
using SusGalerie.Models;

namespace SusGalerie.Pages
{
    [Authorize]
    public class ImagesModel : PageModel
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
        public ImageCollection Gallery { get; set; }

        public Guid GalleryId { get; set; }

        public ImagesModel(ILogger<IndexModel> logger, IWebHostEnvironment environment, ApplicationDbContext context, UserManager<User> userManager)
        {
            _environment = environment;
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public string UserId { get; set; }

        public void OnGet(Guid id)
        {
            GalleryId = id;
            Gallery = _context.Galleries.AsNoTracking().Where(e => e.Id == id).FirstOrDefault();
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
        }
        public async Task<IActionResult> OnGetThumbnail(string filename, ThumbnailType type = ThumbnailType.Square)
        {
            StoredFile file = await _context.Files.AsNoTracking().Where(f => f.Id == Guid.Parse(filename)).SingleOrDefaultAsync();
            if ((User.Identity.IsAuthenticated && file.UploaderId == _userManager.GetUserId(HttpContext.User)) || file.Public)
            {
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
            return NotFound("user is not authenticated or file is not public");
        }
    }
}
