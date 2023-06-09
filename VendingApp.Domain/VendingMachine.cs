using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingApp.Domain
{
    /// <summary>
    /// Торговый автомат
    /// </summary>
    [Table("VendingMachines")]
    public class VendingMachine
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Количество полок
        /// </summary>
        public int Shelf { get; set; }

        /// <summary>
        /// Количество мест на полке
        /// </summary>
        public int PlacesOnShelfCount { get; set; }

        [NotMapped] public int TotalPlaces { get { return Shelf * PlacesOnShelfCount; } }

        /// <summary>
        /// Нельзя установить продуктов больше чем уместится в аппарат
        /// </summary>
        public List<Product> Products { get; private set; } = new();

        public void AddProduct(Product product)
        {
            if (Products.Count + 1 <= TotalPlaces)
            {
                Products.Add(product);
            }
        }

        public void SetProducts(List<Product> products)
        {
            if (products.Count <= TotalPlaces)
            {
                Products = products;
            }
        }
    }
}
