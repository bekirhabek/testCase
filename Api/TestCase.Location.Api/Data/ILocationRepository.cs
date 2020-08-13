using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCase.Location.Api.Data.Entity;

namespace TestCase.Location.Api.Data
{
    public interface ILocationRepository
    {

        Task<Locations> InsertAsync(Locations location);

    }
}
