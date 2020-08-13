using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCase.DistanceCalculation.Consumer.Model
{
    public class DistanceCalculationQueueModel
    {

        public double SourceLatitude { get; set; }

        public double SourceLongitude { get; set; }

        public double DestinationLatitude { get; set; }

        public double DestinationLongitude { get; set; }

    }
}
