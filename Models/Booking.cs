using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Excursion.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        [ForeignKey("Tour")]
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
        public DateTime TourDate { get; set; }
        public int NumberOfGuests { get; set; }
        public double TotalCost { get; set; }
        public string? BookingNotes { get; set; }
        public AppUser? AppUser { get; set; }
        


    }
}