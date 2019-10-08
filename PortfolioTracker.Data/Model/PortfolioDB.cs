using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Model
{    
    public class PortfolioDb : DbContext
    {
        public PortfolioDb() //: base("name=DefaultConnection")
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseInme(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=StoreDB;");
        }

        //public DbSet<Trade> Trade { get; set; }
        //public DbSet<Stock> Stock { get; set; }
    }
}
