using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCase.DistanceCalculation.Consumer.Consumer
{
    public class ApplicationSettings
    {
        public int QueueRetryCount { get; set; }
        public int QueueRetryPeriod { get; set; }
        public string ExchangeName { get; set; }
        public string DlxExchangeName { get; set; }
        public string QueueName { get; set; }
        public string DlxQueueName { get; set; }
    }
}
