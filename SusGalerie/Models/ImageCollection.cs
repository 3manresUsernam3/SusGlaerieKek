using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SusGalerie.Models
{
    public class ImageCollection
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = String.Empty;
        public bool Public { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<StoredFile> Images { get; set; }
    }
}
