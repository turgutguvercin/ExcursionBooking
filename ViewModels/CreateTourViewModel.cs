using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Excursion.Models;

namespace Excursion.ViewModels
{
    public class CreateTourViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public  required string Image { get; set; }
        public int Minor { get; set; }
        public int AddressId { get; set; }
        public required Address Address { get; set; }
    }
}