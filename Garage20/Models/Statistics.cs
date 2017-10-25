using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Statistics
    {
        public Dictionary<string, int> Types { get; set; } = new Dictionary<string, int>();
        public int Wheels { get; set; }

        [DisplayFormat(DataFormatString = "{0:%d} day(s) {0:hh\\:mm\\:ss}")]
        public TimeSpan Time { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}