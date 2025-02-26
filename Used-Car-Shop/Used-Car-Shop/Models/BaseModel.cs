namespace Used_Car_Shop.Models
{
    public class BaseModel
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
    }
}
