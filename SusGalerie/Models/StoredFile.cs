using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SusGalerie.Models;

namespace SusGalerie.Models
{
    public class StoredFile
    {
        [Key]
        public Guid Id { get; set; } // identifikátor souboru a název fyzického souboru
        [ForeignKey("UserId")]
        public User Uploader { get; set; } // kdo soubor nahrál
        [Required]
        public string UploaderId { get; set; } // identifikátor uživatele, který soubor nahrál
        [Required]
        public DateTime UploadedAt { get; set; } = DateTime.Now; // datum a čas nahrání souboru
        [Required]
        public string OriginalName { get; set; } // původní název souboru
        [Required]
        public string ContentType { get; set; } // druh obsahu v souboru (MIME type)
        public ICollection<Thumbnail> Thumbnails { get; set; } // kolekce všech možných náhledů
        public DateTime DateTaken { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int Rows { get; set; } = 1;
        public int Columns { get; set; } = 1;
        public bool Public { get; set; }
        public Guid GalleryId { get; set; }
        public ImageCollection Gallery { get; set; }
    }
}
