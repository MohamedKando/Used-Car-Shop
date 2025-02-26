using System.ComponentModel.DataAnnotations.Schema;

namespace Used_Car_Shop.Models
{
    public class Model : BaseModel
    {
        [ForeignKey("Brand")]
        public int brand_id { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Cars> Cars { get; set; }
    }
}
