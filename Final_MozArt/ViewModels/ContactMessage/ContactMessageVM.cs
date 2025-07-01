namespace Final_MozArt.ViewModels.ContactMessage
{
    public class ContactMessageVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
