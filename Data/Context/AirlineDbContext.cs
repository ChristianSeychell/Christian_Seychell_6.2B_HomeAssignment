using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class AirlineDbContext : IdentityDbContext
    {

        public AirlineDbContext(DbContextOptions<AirlineDbContext> options)
            : base(options)
        { }

        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<Flight> flight {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }


    }
}
