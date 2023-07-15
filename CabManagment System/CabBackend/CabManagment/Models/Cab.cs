using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CabManagment.Models
{
    public class Cab
    {
        [Key]
        public int Id{get;set;}
        public string? RegistrationNo{get;set;}
        public string? Model{get;set;}
        public string? Colour{get;set;}
    }
}