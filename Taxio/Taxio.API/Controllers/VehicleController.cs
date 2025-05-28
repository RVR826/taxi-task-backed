using Microsoft.AspNetCore.Mvc;
using Taxio.DataAccess.Models;
using Taxio.DataAccess.Services;
using Taxio.Shared.DTO;

namespace Taxio.API.Controllers
{
    /// <summary>
    /// Hadles API requests about trip inquiry and adding new vehicles to the fleet
    /// </summary>
    [ApiController]
    [Route("/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Add a new vehicle to the fleet
        /// </summary>
        /// <param name="requestDto">The specification of the new vehicle</param>
        /// <response code="200">Vehicle added successfully</response>
        /// <response code="400">Error in the data or structure of the request</response>
        [HttpPost]
        [Route("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleRequestDto requestDto)
        {
            Vehicle vehicle = new Vehicle
            {
                LicensePlate = requestDto.LicensePlate,
                PassengerCapacity = requestDto.PassengerCapacity,
                Range = requestDto.Range
            };

            if(Enum.IsDefined(typeof(FuelType), requestDto.FuelType))
            {
                vehicle.FuelType = (FuelType)requestDto.FuelType;
                await _vehicleService.AddVehicleAsync(vehicle);

                return StatusCode(201);
            }

            return BadRequest("Not a valid fuel type!");
        }

        /// <summary>
        /// Gets all possible combinations of vehicles that can be used for the specified trip
        /// </summary>
        /// <param name="passengers">The number of passengers</param>
        /// <param name="distance">The distance of the trip</param>
        /// <response code="200">All the possible vehicle combinations for the trip</response>
        /// <response code="400">Error in the data or structure of the request</response>
        [HttpGet]
        [Route("get-combinations")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GetCombinationsResultDto))]
        public async Task<IActionResult> GetVehcileCombinations([FromQuery] int passengers, [FromQuery] int distance)
        {
            if (passengers < 1 || distance < 1)
                return BadRequest("Both passengers and distance need to be greater than 0!");

            var combinations = await _vehicleService.GetVehcilesForTripAsync(passengers, distance);
            GetCombinationsResultDto resultDto = new GetCombinationsResultDto
            {
                Options = new()
            };

            foreach (var combination in combinations)
            {
                var profit = _vehicleService.CalculateProfitForTrip(passengers, distance, combination);
                
                TripDataDto tripDataDto = new TripDataDto
                {
                    Profit = profit,
                    Vehicles = new()
                };

                foreach (var vehicle in combination)
                {
                    tripDataDto.Vehicles.Add(new VehicleDto
                    {
                        Id = vehicle.Id,
                        LicensePlate = vehicle.LicensePlate,
                        PassengerCapacity = vehicle.PassengerCapacity,
                        Range = vehicle.Range,
                        FuelType = vehicle.FuelType.ToString()
                    });
                }

                resultDto.Options.Add(tripDataDto);
            }

            return Ok(resultDto);
        }
    }
}
