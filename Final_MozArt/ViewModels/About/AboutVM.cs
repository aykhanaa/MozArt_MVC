namespace Final_MozArt.ViewModels.About
{
    public class AboutVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
