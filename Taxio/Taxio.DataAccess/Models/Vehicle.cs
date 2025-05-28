using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Taxio.DataAccess.Models
{
    /// <summary>
    /// Class <c>Vehilce</c> models a vehicle entity in the database.
    /// </summary>
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LicensePlate { get; set; } = null!;

        [Required]
        [IntegerValidator(MinValue = 1)]
        public int PassengerCapacity { get; set; }

        [Required]
        [IntegerValidator(MinValue = 1)]
        public int Range { get; set; }

        [Required]
        public FuelType FuelType { get; set; }
    }
}
