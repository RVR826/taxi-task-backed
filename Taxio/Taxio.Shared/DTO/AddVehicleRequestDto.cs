namespace Taxio.Shared.DTO
{
    /// <summary>
    /// Class representing the body of the /vehicles/add API call that can be converted to a Json object
    /// </summary>
    public class AddVehicleRequestDto
    {
        public string LicensePlate { get; set; } = null!;
        public int PassengerCapacity { get; set; }
        public int Range { get; set; }
        public int FuelType { get; set; }
    }
}
