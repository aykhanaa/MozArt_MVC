namespace Final_MozArt.Models
{
    public class BlogComment : BaseEntity
    { public string AppUserId { get; set; }
      public AppUser AppUser { get; set; }
      public string Name { get; set; }
      public string Email { get; set; }
      public string Comment { get; set; }
      public int BlogId { get; set; }
      public Blog Blog { get; set; }
    }
}
