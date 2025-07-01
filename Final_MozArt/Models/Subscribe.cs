using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.Models
{
    public class Subscribe : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
