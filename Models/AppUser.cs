using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Excursion.Models
{
    public class AppUser: IdentityUser
    {
        
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public ICollection<Booking>? Bookings { get; set;}
        
    }
}