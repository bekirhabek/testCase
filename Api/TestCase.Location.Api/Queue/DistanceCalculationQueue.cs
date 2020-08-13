using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCase.Location.Api.Data.Entity;

namespace TestCase.Location.Api.Queue
{
    public class DistanceCalculationQueue
    {
        public DistanceCalculationQueue(
            double destinationLatitude,
            double destinationLongitude,
            double sourceLatitude,
            double sourceLongitude)
        {

            DestinationLatitude = destinationLatitude;
            DestinationLongitude = destinationLongitude;
            SourceLatitude = sourceLatitude;
            SourceLongitude = sourceLongitude;
        }

        public double SourceLatitude { get; set; }

        public double SourceLongitude { get; set; }

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }

    }
}
