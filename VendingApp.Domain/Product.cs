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
        /// Количество товара
        /// </summary>
        public int Count { get; private set; }

        public bool SetCount(int count)
        {
            if (Count - count > 0) 
            { 
                Count -= count;
                return true;
            }
            return false;
        }
    }
}
