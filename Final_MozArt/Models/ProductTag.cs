﻿namespace Final_MozArt.Models
{
    public class ProductTag : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product{ get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
