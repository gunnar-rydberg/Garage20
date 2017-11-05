using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Member
    {
        [Display(Name = "Member Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }

    }
}