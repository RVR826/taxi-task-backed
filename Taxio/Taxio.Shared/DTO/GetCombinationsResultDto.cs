namespace Taxio.Shared.DTO
{
    /// <summary>
    /// Class representing the result of the /vehicles/get-combinations API call that can be converted to a Json object
    /// </summary>
    public class GetCombinationsResultDto
    {
        public List<TripDataDto> Options { get; set; } = null!;
    }
}
