namespace Used_Car_Shop.DataTransferObjebt
{
    public class CarEditDto
    {
        public string? CurrentCover { get; set; }
        public IFormFile? Cover { get; set; }
        public decimal price { get; set; }
    }
}
