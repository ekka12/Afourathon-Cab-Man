using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CabManagment.Models;

namespace DAL
{
    public class ContextCollection : DbContext
    {
        public DbSet<CabUser> CabUser {get;set;}
        public DbSet<CabDriver> CabDriver{get;set;}
        public DbSet<Cab> Cab {get;set;}
        public DbSet<BookedCab> BookedCab {get;set;}
        public ContextCollection(DbContextOptions<ContextCollection> options) : base(options){}
    }
}