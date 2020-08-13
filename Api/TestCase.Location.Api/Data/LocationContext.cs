using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCase.Location.Api.Data.Entity;

namespace TestCase.Location.Api.Data
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options)
           : base(options)
        {
        }


        public DbSet<Locations> Location { get; set; }

    }
}
