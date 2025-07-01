namespace Final_MozArt.ViewModels.Layout
{
    public class HeaderVM
    {
        public string UserFullName { get; set; }
        public int BasketCount { get; set; }
        public int WishlistCount { get; set; }
        public string UserId { get; set; }
        public Dictionary<string, string> Setting { get; set; }
    }
}
