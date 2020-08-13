using EasyNetQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCase.DistanceCalculation.Consumer.Model;

namespace TestCase.DistanceCalculation.Consumer.Consumer
{
    public class QueueConsumer
    {
        private readonly IBus _bus;
        private readonly ApplicationSettings _config;

        public QueueConsumer(
            IBus bus,
            ApplicationSettings config)
        {
            _bus = bus;
            this._config = config;
        }
        public async Task Consume()
        {
            var notifyExchange = _bus.Advanced.ExchangeDeclare(_config.ExchangeName, EasyNetQ.Topology.ExchangeType.Direct);
            var notifyDlxExchange = _bus.Advanced.ExchangeDeclare(_config.DlxExchangeName, EasyNetQ.Topology.ExchangeType.Direct);

            var notifyQueue = _bus.Advanced.QueueDeclare(_config.QueueName, deadLetterExchange: notifyDlxExchange.Name);
            var notifyDlxQueue = _bus.Advanced.QueueDeclare(_config.DlxQueueName, deadLetterExchange: notifyExchange.Name, perQueueMessageTtl: _config.QueueRetryPeriod);

            _bus.Advanced.Bind(notifyExchange, notifyQueue, string.Empty);
            _bus.Advanced.Bind(notifyDlxExchange, notifyDlxQueue, string.Empty);

            _bus.Advanced.Consume(notifyQueue, async (body, properties, info) =>
            {
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var model = System.Text.Json.JsonSerializer.Deserialize<DistanceCalculationQueueModel>(message);


                    var distance = DistanceTo(model.SourceLatitude, model.SourceLongitude, model.DestinationLatitude, model.DestinationLongitude);

                    //Save File

                    string curFile = GenerateFileName();

                    if (!File.Exists(curFile))
                    {
                        File.CreateText(curFile);
                    }

                    StringBuilder txtLine = new StringBuilder();

                    txtLine
                        .Append(model.SourceLatitude).Append(" ")
                        .Append(model.SourceLongitude).Append(" ")
                        .Append(model.DestinationLatitude).Append(" ")
                        .Append(model.DestinationLongitude).Append(" ")
                        .Append(distance)
                    .AppendLine();

                    File.AppendAllText(curFile, txtLine.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception("Hesaplama yapılamadı. " + ex.Message);
                }
            });
            await Task.CompletedTask;
        }

        private string GenerateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHH") + ".txt";
        }


        private double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
    }
}
