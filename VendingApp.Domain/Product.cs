using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingApp.Domain
{
    [Table("Products")]
    public class Product
    {
        [Key] public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Стоимость товара
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Количество товара
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Адрес картинки
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Код товара на витрине
        /// </summary>
        public int? Code { get; set; }

        public bool SetCount(int count)
        {
            if (Count - count > 0) 
            { 
                Count -= count;
                return true;
            }
            return false;
        }

        public VendingMachine VendingMachine { get; } = null!;
    }
}
