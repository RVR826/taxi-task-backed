namespace Taxio.Shared.DTO
{
    /// <summary>
    /// Class representing the data of a vehicle that can be converted to a Json object
    /// </summary>
    public class VehicleDto
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = null!;
        public int PassengerCapacity { get; set; }
        public int Range { get; set; }
        public string FuelType { get; set; } = null!;
    }
}
