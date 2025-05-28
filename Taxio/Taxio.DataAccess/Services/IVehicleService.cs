using Taxio.DataAccess.Models;

namespace Taxio.DataAccess.Services
{
    /// <summary>
    /// Interface <c>IVehicleService</c> gives the structure of services dealing with vehicle data
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Add a new vehicle to the fleet
        /// </summary>
        /// <param name="licensePlate">Lincense plate of the vehicle</param>
        /// <param name="passengerCapacity">Passenger capacity of the vehicle</param>
        /// <param name="range">Maximum range of the vehicle (km)</param>
        /// <param name="fuelType">Fuel type of the vehicle</param>
        Task AddVehicleAsync(string licensePlate, int passengerCapacity, int range, FuelType fuelType);

        /// <summary>
        /// Add a new vehicle to the fleet
        /// </summary>
        /// <param name="vehicle">Pre-constructed vehicle data</param>
        Task AddVehicleAsync(Vehicle vehicle);

        /// <summary>
        /// Calculates profit for a given trip with the provided vehicles
        /// </summary>
        /// <param name="passengers">Number of passengers</param>
        /// <param name="distance">Trip distance (km)</param>
        /// <param name="vehicles">Vehicles used on the trip</param>
        /// <returns>The profit for the trip</returns>
        int CalculateProfitForTrip(int passengers, int distance, List<Vehicle> vehicles);

        /// <summary>
        /// Gets all possible combination of vehicles suitable for the trip
        /// </summary>
        /// <param name="passengers">Number of passengers</param>
        /// <param name="distance">Trip distance (km)</param>
        /// <param name="maxVehicles">The maximum number of vehicles used (default: number of passengers)</param>
        /// <returns>List of all possible vehicle combinations</returns>
        Task<List<List<Vehicle>>> GetVehcilesForTripAsync(int passengers, int distance, int? maxVehicles = null);
    }
}