namespace OnlineStore.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ImagePath { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }

    }
}

