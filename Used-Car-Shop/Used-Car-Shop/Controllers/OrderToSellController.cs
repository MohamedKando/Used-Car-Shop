using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Used_Car_Shop.Ai_Model;
using Used_Car_Shop.DataTransferObjebt;

namespace Used_Car_Shop.Controllers
{
    public class OrderToSellController : Controller
    {
        private HttpClient _client;
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string imagepath;
        public OrderToSellController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            _client = new HttpClient();
           
            this.db = db;
            imagepath = $"{webHostEnvironment.WebRootPath}/Images";
        }

        public IActionResult Index()
        {
           var orders = db.OrdersToSell.Include(m => m.Model).Include(b => b.Brand).Include(b=>b.user).ToList();
            return View(orders);
        }
        public IActionResult Delete(int id)
        {
            var car = db.OrdersToSell.Find(id);

            if (car.FilePath != null)
            {
                var cover = Path.Combine(imagepath, car.FilePath);
                System.IO.File.Delete(cover);
            }
            db.OrdersToSell.Remove(car);
            db.SaveChanges();
            TempData["Success"] = "Item Deleted Succesfully";
            return RedirectToAction("Index");

        }
        public IActionResult Add(int id)
        {

            var OrderCar = db.OrdersToSell.Find(id);
            if (OrderCar != null)
            {

                Cars cars = new()
                {
                    BrandId = OrderCar.BrandId,
                    ModelId = OrderCar.ModelId,
                    tranmission = OrderCar.tranmission,
                    AirConditioner = OrderCar.AirConditioner,
                    RemoteControl = OrderCar.RemoteControl,
                    PowerSteering = OrderCar.PowerSteering,
                    year = OrderCar.year,
                    milage = OrderCar.milage,
                    Gov = OrderCar.Gov,

                    FilePath = OrderCar.FilePath,

                    price = OrderCar.price
                };



                db.Cars.Add(cars);

                db.SaveChanges();

                
                TempData["Success"] = "Item Added Successfully";
                db.OrdersToSell.Remove(OrderCar);
                db.SaveChanges();
            }
            // return RedirectToAction("Index", "Cars");
            return RedirectToAction("Index");
        }


    }
}
