using System;
using System.Collections.Generic;

#nullable disable

namespace ImporterConsoleApp.Models
{
    public partial class LogIngestion
    {
        public Guid Id { get; set; }
        public DateTime IngestionTimestamp { get; set; }
        public short FinalState { get; set; }
        public long RowsIngested { get; set; }
        public string Error { get; set; }
    }
}
