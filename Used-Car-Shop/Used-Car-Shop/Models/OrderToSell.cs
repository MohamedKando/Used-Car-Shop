using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Used_Car_Shop.Models
{
    public class OrderToSell
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Model")]
        public int ModelId { get; set; }
        public Model Model { get; set; }

        [Required]
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        [Required]
        public string tranmission { get; set; } = string.Empty;
        [Required]
        [Range(1900, 2025, ErrorMessage = "Year must be between 1900 and 2025")]
        public int year { get; set; }
        [Required]
        public string AirConditioner { get; set; } = string.Empty;
        [Required]
        public int milage { get; set; }
        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public string Gov { get; set; }
        [Required]

        public string PowerSteering { get; set; }
        [Required]

        public string RemoteControl { get; set; }

        public DateTime Date { get; set; }= DateTime.Now;
        public string? userId { get; set; }
        public IdentityUser? user { get; set; }
        [Required]
        public decimal price { get; set; }
    }
}
