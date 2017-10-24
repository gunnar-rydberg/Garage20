using System;

namespace Garage20.Models
{
    public class KvittoViewModel
    {
      
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public Vehicle Vehicle { get; set; }
        public double price { get; set; }

    }
}