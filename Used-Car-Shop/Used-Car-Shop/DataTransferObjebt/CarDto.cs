using System.ComponentModel.DataAnnotations.Schema;

namespace Used_Car_Shop.DataTransferObjebt
{
    public class CarDto
    {
        public int Id { get; set; }
  
        public int ModelId { get; set; }
      
    
        public int BrandId { get; set; }
       

        [Required]
        public string tranmission { get; set; } = string.Empty;
        [Required]
        [Range(1900, 2025, ErrorMessage = "Year must be between 1900 and 2025")]
        public int year { get; set; }
        public string AirConditioner { get; set; } = string.Empty;
        [Required]
        public int milage { get; set; }
      
        public IFormFile? Cover { get; set; }

        [Required]
       

        public string RemoteControl { get; set; }
        public string? CurrentCover { get; set; }
        public string Gov { get; set; }
        [Required]

        public string PowerSteering { get; set; }
      
        public decimal? price { get; set; }
    }
}
