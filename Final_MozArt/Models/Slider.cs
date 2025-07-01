namespace Final_MozArt.Models
{
    public class Slider : BaseEntity
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;
    }
}
