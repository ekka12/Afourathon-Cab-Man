using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CabManagment.Models
{
    public class CabDriver
    {
        [Key]
        public int  Id{get;set;}    
        public string? DName{get;set;}  
        public string? DEmail{get;set;}
        public string? DPhoneNo{get;set;}
        
    }
}