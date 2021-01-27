using System;
using System.Collections.Generic;

#nullable disable

namespace ImporterConsoleApp.Models
{
    public partial class StockEntity
    {
        public Guid Id { get; set; }
        public string PointOfSale { get; set; }
        public string ProductId { get; set; }
        public DateTime DateStock { get; set; }
        public int Stock {get; set;}
    }
}
