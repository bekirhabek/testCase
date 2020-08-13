using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCase.Location.Api.Model.Request
{
    public class LocationPostRequest
    {

        public double SourceLatitude { get; set; }

        public double SourceLongitude { get; set; }

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }
    }
}
