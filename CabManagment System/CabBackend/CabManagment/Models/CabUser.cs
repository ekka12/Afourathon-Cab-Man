using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CabManagment.Models
{
    public class CabUser
    {
        [Key]
       public int Id{get;set;}
       public string? Uname{get;set;}
       public string? Uphoneno{get;set;}
               
    }
}