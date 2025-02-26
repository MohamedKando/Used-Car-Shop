
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Security.Claims;
using Used_Car_Shop.DataTransferObjebt;

namespace Used_Car_Shop.Controllers
{
    public class CarsController : Controller
    {
       
        private readonly CarPricePredictor _carPricePredictor;
        private HttpClient _client;
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string imagepath;
        public int predicted;
        public CarsController(CarPricePredictor carPricePredictor, AppDbContext db , IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            _client = new HttpClient();
            _carPricePredictor = carPricePredictor;
            this.db = db;
            imagepath = $"{webHostEnvironment.WebRootPath}/Images";

        }

        public IActionResult Index()
        {
                     
            var car = db.Cars.Include(m=>m.Model).Include(b=>b.Brand).ToList();
            return View(car);
        }

        [Authorize]
        public IActionResult Predict()
        {

            LoadSelectedListData();
            ModelsList();
            BrandsList();
            if (!ModelState.IsValid)
            {
                // Optionally, log and return the same view with validation errors
                Console.WriteLine("Model is not valid.");
                return View();
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> PredictCarPrice(CarDto car)
        {
            try
            {
                LoadSelectedListData();
                ModelsList();
                BrandsList();
                // Check if the model is valid
                if (!ModelState.IsValid)
                {
                    // Optionally, log and return the same view with validation errors
                    Console.WriteLine("Model is not valid.");
                    return RedirectToAction("Predict",car);
                }

                // Call the GetPredictedAsync method and get the predicted price
                var predictedPrice = await _carPricePredictor.GetPredictedAsync(car);
                Console.WriteLine($"Predicted Price: {predictedPrice}");  // Debugging line
           
                // Store the predicted price in ViewBag to display in the view
                TempData["PredictedPrice"] = (int)(predictedPrice- (predictedPrice%1000));
                TempData.Keep("PredictedPrice");

                // Optionally, set a redirect URL if you want to navigate to a different page


                return View(car);
            }
            catch (Exception ex)
            {
                // Handle any errors here (e.g., log the exception or show an error message)
                ViewBag.Error = "An error occurred: " + ex.Message;
                Console.WriteLine($"Error: {ex.Message}");

                TempData["Warning"] = "The Server is not connecting with the Ai model";
                return RedirectToAction("Predict", car);
            }
        }
        public IActionResult Delete(int id)
        {
            var car = db.Cars.Find(id);

            if (car != null)
            {
                db.Cars.Remove(car);
                db.SaveChanges();
                if (car.FilePath != null)
                {
                    var cover = Path.Combine(imagepath, car.FilePath);
                    System.IO.File.Delete(cover);
                }
            }
            TempData["Success"] = "Item Deleted Succesfully";
            return RedirectToAction("Index");

        }

        public IActionResult Add(CarDto car)
        {
            if (car != null)
            {
                string CoverName = "";
                if (car.Cover != null)
                { CoverName = SaveCover(car.Cover); }
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                OrderToSell cars = new()
                {
                    BrandId = car.BrandId,
                    ModelId = car.ModelId,
                    tranmission = car.tranmission,
                    AirConditioner = car.AirConditioner,
                    RemoteControl = car.RemoteControl,
                    PowerSteering = car.PowerSteering,
                    year = car.year,
                    milage = car.milage,
                    Gov = car.Gov,
                    userId = UserId,
                    FilePath = CoverName,

                    price = car.price ?? 0 // Keep nullable for now
                };

                // ✅ Manually validate the `Cars` model
                var validationContext = new ValidationContext(cars);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(cars, validationContext, validationResults, true))
                {
                    foreach (var error in validationResults)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }

                // ✅ Specifically check for `price` validation
                if (car.price == null)
                {
                    ModelState.AddModelError("price", "Price is required Please repeat the predict proccess.");
                }

                if (!ModelState.IsValid)
                {
                    LoadSelectedListData();


                    return View("PredictCarPrice", car); // Return to view with errors
                }
                if (car.Cover != null)
                {
                    using var memoryStream = new MemoryStream();
                    car.Cover.CopyTo(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    var imageService = new ImageValidationService();
                    string result = imageService.PredictCarImage(imageBytes);

                    if (result != "Car")
                    {
                        ModelState.AddModelError("Cover", "The uploaded image is not recognized as a car.");
                        return View("PredictCarPrice", car); // Show error message
                    }
                }

                db.OrdersToSell.Add(cars);
                db.SaveChanges();

                TempData["Success"] = "Congratulations! Your car has been successfully added to the order list and is awaiting review by our team." +
                    " You will be notified via email once it has been approved.";
            }
            return RedirectToAction("Index");
        }
        public IActionResult edit(int id )
        {
            var car = db.Cars.Find(id);
            var car2 = new CarEditDto()
            {
                CurrentCover = car.FilePath,
                price = car.price

            };


            return View(car2);
        }
        [HttpPost]
        public IActionResult edit(CarEditDto car, int id)
        {
            var hasNewCover = car.Cover is not null;
            var car1 = db.Cars.Find(id);
            string oldcover = "";
            car1.price = car.price;

            if (hasNewCover)
            {
                oldcover = car1.FilePath;
                car1.FilePath =  SaveCover(car.Cover!);

            }
            db.Update(car1);
            var changes = db.SaveChanges();
            if (changes > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(imagepath, oldcover);
                    System.IO.File.Delete(cover);
                }
            }
            return RedirectToAction("Index");
        }
       
        public string SaveCover(IFormFile Cover)
        {
            var CoverName=$"{Guid.NewGuid()}{Path.GetExtension(Cover.FileName)}";
            var path = Path.Combine(imagepath, CoverName);
            using var stream = System.IO.File.Create(path);
            Cover.CopyTo(stream);

            return CoverName;
        }
        private SelectList GetSelectListFromJson(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                List<string>? items = JsonSerializer.Deserialize<List<string>>(jsonData);
                return new SelectList(items ?? new List<string>());
            }
            return new SelectList(new List<string>());
        }



        public void ModelsList(int selectid = 1)
        {
            List<Model> models = db.Models.ToList();

            // Create a list of SelectListItem instead of SelectList
            var modelList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Model", Selected = true } // Ensure it's selected
            };

            // Add actual models
            modelList.AddRange(models.Select(m => new SelectListItem
            {
                Value = m.id.ToString(),
                Text = m.name
            }));

            ViewBag.CarModels = modelList; // Store as List<SelectListItem>
        }
        public void BrandsList(int selectid = 1)
        {
            List<Brand> brands = db.Brands.ToList();

            // Create a list of SelectListItem instead of SelectList
            var brandList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Brand", Selected = true } 
            };

            // Add actual brands
            brandList.AddRange(brands.Select(b => new SelectListItem
            {
                Value = b.id.ToString(),
                Text = b.name
            }));

            ViewBag.CarBrands = brandList; // Store as List<SelectListItem>
        }
        private void LoadSelectedListData()
        {
            ViewBag.CarGov = GetSelectListFromJson(FileSettings.GovFilePath);

            ViewBag.CarEngine = GetSelectListFromJson(FileSettings.EngineFilePath);
            ViewBag.CareTranssions = GetSelectListFromJson(FileSettings.TransmissionsFilePath);
        }

        public JsonResult GetModelsByBrand(int brandId)
        {
            var models = db.Models.Where(m => m.brand_id == brandId)
                                  .Select(m => new { m.id, m.name })
                                  .ToList();

            return Json(models);
        }

    }
}
