﻿namespace Final_MozArt.Models
{
    public class About : BaseEntity
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;

    }
}
