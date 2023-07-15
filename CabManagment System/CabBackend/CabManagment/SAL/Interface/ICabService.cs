using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CabManagment.Models;

namespace CabManagment.SAL.Interface
{
    public interface ICabService
    {
        public List<CabUser> ShowAll();
        public bool AddUser(CabUser cuser);
        public bool AddBooking(BookedCab booker);
       
        public List<Cab> ShowCab();
        public List<CabDriver> ShowDriver();
        public List<BookedCab> ShowBooking();

        public bool RemoveBooking(int id);
        public bool UpdateBooking(BookedCab booker);
    }
}