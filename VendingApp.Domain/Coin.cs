using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingApp.Domain
{
    [Table("Coins")]
    public class Coin
    {
        [Key] public int Id { get; set; }

        /// <summary>
        /// Номинал
        /// </summary>
        public int Denomination { get; set; }

        /// <summary>
        /// Заблокирована ли монета
        /// </summary>
        public bool IsBlocked { get; set; }
    }
}
