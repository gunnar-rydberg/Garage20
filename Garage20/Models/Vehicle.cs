using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Vehicle
    {

        public int Id { get; set; }
        public string Type { get; set; }
        [Required]
        public string RegNo { get; set; }
        [Required]
        public string Color { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please write number")]
        public int NoWheels { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public DateTime Date { get; set; }
                                          

    }
}