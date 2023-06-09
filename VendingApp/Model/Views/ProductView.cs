using VendingApp.Domain;

namespace VendingApp.Model.Views
{
    public class ProductView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Count { get; set; }
        public string? Image { get; set; }
        public int Code { get; set; }

        public ProductView(Product product)
        {
            if (product.Code == null)
                throw new Exception();
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Count = product.Count;
            Image = product.Image;
            Code = (int)product.Code;
        }
    }
}
