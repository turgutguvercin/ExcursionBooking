using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Excursion.Models;

namespace Excursion.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
        [DataType(DataType.Date)]
        public DateTime TourDate { get; set; }
        public int NumberOfGuests { get; set; }
        public double? TotalCost { get; set; }
        public string? BookingNotes { get; set; }



    }
}