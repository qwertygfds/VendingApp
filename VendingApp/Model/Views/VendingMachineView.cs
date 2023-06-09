using VendingApp.Domain;

namespace VendingApp.Model.Views
{
    public class VendingMachineView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Shelf { get; set; }
        public int PlacesOnShelfCount { get; set; }
        public int TotalPlaces { get; set; }
        public List<ProductView> Products { get; set; } = new();
        public VendingMachineView(VendingMachine vendingMachine)
        {
            Id = vendingMachine.Id;
            Name = vendingMachine.Name;
            Shelf = vendingMachine.Shelf;
            PlacesOnShelfCount = vendingMachine.PlacesOnShelfCount;
            TotalPlaces = vendingMachine.TotalPlaces;
            Products = vendingMachine.Products.Select(x => new ProductView(x)).ToList();
        }
    }
}
