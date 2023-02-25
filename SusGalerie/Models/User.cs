using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SusGalerie.Models
{
    public class User : IdentityUser
    {
        public ICollection<ImageCollection> Galleries { get; set; }
    }
}
