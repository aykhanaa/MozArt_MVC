namespace Final_MozArt.ViewModels.Product
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; } 
        public string Hower { get; set; } 

        public string CategoryName { get; set; }

        public string BrandName { get; set; }

        public DateTime CreateDate { get; set; }
        public int CategoryId { get; set; }
    }



}
