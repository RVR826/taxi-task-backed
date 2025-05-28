using Taxio.DataAccess.Models;

namespace Taxio.DataAccess.Services
{
    /// <summary>
    /// Class <c>VehicleService</c> implenets the required methods for hadling vehicle and trip data
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly TaxioDbContext _context;

        public VehicleService(TaxioDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add a new vehicle to the fleet
        /// </summary>
        /// <param name="vehicle">Pre-constructed vehicle data</param>
        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);

            await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Add a new vehicle to the fleet
        /// </summary>
        /// <param name="licensePlate">Lincense plate of the vehicle</param>
        /// <param name="passengerCapacity">Passenger capacity of the vehicle</param>
        /// <param name="range">Maximum range of the vehicle (km)</param>
        /// <param name="fuelType">Fuel type of the vehicle</param>
        public async Task AddVehicleAsync(string licensePlate, int passengerCapacity, int range, FuelType fuelType)
        {
            Vehicle vehicle = new Vehicle
            {
                LicensePlate = licensePlate,
                PassengerCapacity = passengerCapacity,
                Range = range,
                FuelType = fuelType
            };

            await AddVehicleAsync(vehicle);
        }

        /// <summary>
        /// Calculates profit for a given trip with the provided vehicles
        /// </summary>
        /// <param name="passengers">Number of passengers</param>
        /// <param name="distance">Trip distance (km)</param>
        /// <param name="vehicles">Vehicles used on the trip</param>
        /// <returns>The profit for the trip</returns>
        public int CalculateProfitForTrip(int passengers, int distance, List<Vehicle> vehicles)
        {
            // Every customer pays €2 for every kilometer traveled (per vehicle)
            int profit = distance * vehicles.Count * 2;

            // Plus another €2 for every half hour started (per vehicle)
            if (distance < 50)
                profit += (int)Math.Ceiling(distance / 15f) * vehicles.Count;

            else
                profit += (int)Math.Ceiling(distance / 30f) * vehicles.Count;

            // Refill
            foreach (var vehicle in vehicles)
            {
                if (vehicle.FuelType == FuelType.Gasoline)
                    profit -= distance * 2;

                else
                {
                    // For electric drive: 1km = 1€
                    // For hybrid: 2km = 2€
                    profit -= distance;
                }
            }

            return profit;
        }

        /// <summary>
        /// Gets all possible combination of vehicles suitable for the trip
        /// </summary>
        /// <param name="passengers">Number of passengers</param>
        /// <param name="distance">Trip distance (km)</param>
        /// <param name="maxVehicles">The maximum number of vehicles used (default: number of passengers)</param>
        /// <returns>List of all possible vehicle combinations</returns>
        public async Task<List<List<Vehicle>>> GetVehcilesForTripAsync(int passengers, int distance, int? maxVehicles = null)
        {
            return await Task.Run(() =>
            {
                if (maxVehicles is null)
                    maxVehicles = passengers;

                List<List<Vehicle>> combinations = new List<List<Vehicle>>();
                var vehicles = _context.Vehicles
                    .Where(x => x.Range >= distance)
                    .ToList();

                for (int vehicleCount = 1; vehicleCount <= maxVehicles; vehicleCount++)
                {
                    combinations.AddRange(GetCombinations(vehicles, vehicleCount, passengers));
                }

                return combinations;
            });
        }

        /// <summary>
        /// Calculates all possible k-size sets of the provided vehicle without repetition.
        /// </summary>
        /// <param name="vehicles">List of vehicles</param>
        /// <param name="vehicleCount">Requested number of vehicles per set</param>
        /// <param name="passengers">Number of passengers</param>
        /// <returns>List of all possible vehicle combinations</returns>
        private static List<List<Vehicle>> GetCombinations(List<Vehicle> vehicles, int vehicleCount, int passengers)
        {
            List<List<Vehicle>> result = new List<List<Vehicle>>();

            if (vehicleCount > vehicles.Count || vehicleCount <= 0)
                return result;

            // Indices to track current combination
            int[] indices = new int[vehicleCount];

            // Initialize with the first combination
            for (int i = 0; i < vehicleCount; i++)
                indices[i] = i;

            while (true)
            {
                // Build current combination
                List<Vehicle> combo = new List<Vehicle>();
                for (int i = 0; i < vehicleCount; i++)
                    combo.Add(vehicles[indices[i]]);

                result.Add(combo);

                // Find the rightmost index that can be incremented
                int pos = vehicleCount - 1;
                while (pos >= 0 && indices[pos] == vehicles.Count - vehicleCount + pos)
                    pos--;

                // All combinations generated
                if (pos < 0)
                    break;

                // Increment this index and reset all to the right
                indices[pos]++;
                for (int i = pos + 1; i < vehicleCount; i++)
                    indices[i] = indices[i - 1] + 1;
            }

            return result
                .Where(x => x.Sum(x => x.PassengerCapacity) >= passengers)
                .ToList();
        }
    }
}
