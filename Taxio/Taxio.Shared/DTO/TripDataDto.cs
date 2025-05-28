namespace Taxio.Shared.DTO
{
    /// <summary>
    /// Class representing the data of a trip that can be converted to a Json object
    /// </summary>
    public class TripDataDto
    {
        public int Profit { get; set; }
        public List<VehicleDto> Vehicles { get; set; } = null!;
    }
}
