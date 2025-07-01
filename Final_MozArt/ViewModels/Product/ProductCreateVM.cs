using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Product
{
    public class ProductCreateVM
    {
        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Name must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than 0.")]
        public decimal Price { get; set; }

        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Description must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please select a brand")]
        public int BrandId { get; set; }

        [Required]
        public ICollection<int> ColorIds { get; set; }

        [Required]
        public ICollection<int> TagIds { get; set; }

        [Required(ErrorMessage = "Please upload an image.")]
        public ICollection<IFormFile> Photos { get; set; }
    }
}
