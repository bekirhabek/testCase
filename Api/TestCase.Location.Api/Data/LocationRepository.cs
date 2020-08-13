using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCase.Location.Api.Data.Entity;

namespace TestCase.Location.Api.Data
{
    public class LocationRepository : ILocationRepository
    {
        private readonly LocationContext _context;

        public LocationRepository(LocationContext context)
        {
            this._context = context;
        }

        public async Task<Locations> InsertAsync(Locations location)
        {
            await _context.AddAsync(location);

            await _context.SaveChangesAsync();

            return location;
        }
    }
}
