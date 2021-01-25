using System;
using System.Collections.Generic;

#nullable disable

namespace ImporterConsoleApp.Models
{
    public partial class Stock
    {
        public Guid Id { get; set; }
        public int PointOfSale { get; set; }
        public int ProductId { get; set; }
        public DateTime DateStock { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
