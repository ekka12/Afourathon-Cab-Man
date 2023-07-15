using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CabManagment.SAL.Interface;
using CabManagment.Models;
using DAL;

namespace CabManagment.SAL
{
    public class CabService : ICabService
    {
        private readonly ContextCollection ctx;
        
        public CabService(ContextCollection ctx){
            this.ctx = ctx;
        }
           //Cab user list
        public List<CabUser> ShowAll(){
            
            var cabuse = from cb in ctx.CabUser select cb;
            return cabuse.ToList();
        }
           //cab list
        public List<Cab> ShowCab(){
             var cabuse = from cb in ctx.Cab select cb;
             return cabuse.ToList();
        }
          //driver list
        public List<CabDriver> ShowDriver(){
             var cabuse = from cb in ctx.CabDriver select cb;
             return cabuse.ToList();
        }
        //Booked List
        public List<BookedCab> ShowBooking(){
            var cabuse = from cb in ctx.BookedCab select cb;
            return cabuse.ToList();
        }

        public bool AddUser(CabUser cuser){
            bool status = false;
            ctx.CabUser.Add(cuser);
            ctx.SaveChanges();
            status = true;
            return status;
        }

        public bool AddBooking(BookedCab booker){
            bool status = false;
            ctx.BookedCab.Add(booker);
            ctx.SaveChanges();
            status = true;
            return status;
        }

         public bool RemoveBooking(int id){
             bool status = false;
             var rembook = ctx.BookedCab.FirstOrDefault(rb=>rb.Id==id);
             ctx.BookedCab.Remove(rembook);
             ctx.SaveChanges();
             status = true;
             return status;
         }

         public bool UpdateBooking(BookedCab booker){
             bool status = false;
             var rembook = ctx.BookedCab.FirstOrDefault(rb=>rb.Id==booker.Id);
             
             rembook.Id=booker.Id;
             rembook.Model=booker.Model;
             rembook.RegistrationNo=booker.RegistrationNo;
             rembook.Colour = booker.Colour;
             rembook.DName = booker.DName;
             rembook.DEmail=booker.DEmail;
             rembook.DPhoneNo=booker.DPhoneNo;

             ctx.BookedCab.Update(rembook);
             ctx.SaveChanges();
             status = true;
             return status;
         }
    }

}