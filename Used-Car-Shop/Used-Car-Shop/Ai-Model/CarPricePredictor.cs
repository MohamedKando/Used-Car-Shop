using Newtonsoft.Json;
using System.Text;
using Used_Car_Shop.DataTransferObjebt;

namespace Used_Car_Shop.Ai_Model
{
    public class CarPricePredictor
    {
        private  HttpClient _client;
        private readonly AppDbContext db;
        public CarPricePredictor(AppDbContext db)
        {
            _client = new HttpClient();
            this.db = db;
        }

        public async Task<decimal> GetPredictedAsync(CarDto car)
        {
            var url = "http://127.0.0.1:5000/predict"; // Adjust the URL based on where Flask is hosted
            var brand = db.Brands.Find(car.BrandId);
            var model=db.Models.Find(car.ModelId);
            // Prepare the data to be sent to the Flask API (raw data only)
            var carData = new
            {
                Mileage=car.milage,
                Make = brand.name,
                Model =model.name,
                
                Transmission = car.tranmission,
                AirConditioner=car.AirConditioner,
                PowerSteering = car.PowerSteering,
                RemoteControl=car.RemoteControl,
                Year = car.year

            };

            // Serialize data to JSON
            var content = new StringContent(JsonConvert.SerializeObject(carData), Encoding.UTF8, "application/json");

            // Make the API call to Flask
            var response = await _client.PostAsync(url, content);
                Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Body: {await response.Content.ReadAsStringAsync()}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                dynamic prediction = JsonConvert.DeserializeObject(result);
                return prediction.predicted_price;
            }
            else
            {
                throw new Exception("Error making prediction");
            }
        }
    }
}
