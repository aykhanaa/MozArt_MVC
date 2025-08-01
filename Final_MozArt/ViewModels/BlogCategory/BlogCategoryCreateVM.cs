﻿using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.BlogCategory
{
    public class BlogCategoryCreateVM
    {
        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Name must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Name { get; set; }

        
    }
}
