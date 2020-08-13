using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestCase.Location.Api.Data;
using TestCase.Location.Api.Data.Entity;
using TestCase.Location.Api.Model.Request;
using TestCase.Location.Api.Queue;
using TestCase.Location.Api.Settings;

namespace TestCase.Location.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IBus _bus;
        private readonly QueueSettings _queueSettings;

        public LocationController(
            ILocationRepository locationRepository,
            IBus bus,
            QueueSettings queueSettings)
        {
            this._locationRepository = locationRepository;
            this._bus = bus;
            this._queueSettings = queueSettings;
        }

        // POST: api/Location
        [HttpPost]
        public async Task<IActionResult> Post(LocationPostRequest request)
        {
            try
            {
                var location = new Locations
                {
                    DestinationLatitude = request.DestinationLatitude,
                    DestinationLongitude = request.DestinationLongitude,
                    SourceLatitude = request.SourceLatitude,
                    SourceLongitude = request.SourceLongitude
                };

                await _locationRepository.InsertAsync(location);

                var @event = new DistanceCalculationQueue(location.DestinationLatitude, location.DestinationLongitude, location.SourceLatitude, location.SourceLongitude);

                await _bus.Advanced.PublishAsync(new Exchange(_queueSettings.DistanceCalculationName), "",
                                                 false, new MessageProperties(),
                                                 Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }


    }
}
