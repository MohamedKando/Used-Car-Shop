namespace Used_Car_Shop.Models
{
    public class Brand : BaseModel
    {
        public ICollection<Cars> Cars { get; set; }
        public ICollection<Model> models { get; set; }
    }
}
