﻿namespace Final_MozArt.Models
{
    public class ProductImage : BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;
        public bool IsHover { get; set; } = false;
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
