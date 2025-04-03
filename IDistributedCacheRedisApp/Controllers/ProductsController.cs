using IDistributedCacheRedisApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Controllers
{
    public class ProductsController : Controller
    {
        //Redis'i basit versiyonda yani sadece get set işlemleri yapabileceğimiz şekilde kullanabilmek için IDisributedCache'i kullanıyoruz.
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            //Not: En iyi yöntem complex verilerde json serialize ile veriyi kaydetmek. Çünkü byte formatında kaydederken okuma işlemi zor olabilir.

            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            //cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1); //1dk'lık bir ömre sahip oluyor.

            //await _distributedCache.SetStringAsync("name", "deneme", cacheEntryOptions); //Bu şekilde name key'ine deneme value'sunu atıyoruz.

            Product product = new Product { Id = 2, Name = "Kalem2", Price = 200 };

            string jsonproduct = JsonConvert.SerializeObject(product);

            //await _distributedCache.SetStringAsync("product:1", jsonproduct, cacheEntryOptions); //Serilize işlemi ile json formatında kaydediyoruz.

            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct); //Burada da istersek bu şekilde byte formatında kaydedebiliyoruz.

            _distributedCache.Set("product:2", byteproduct);

            return View();
        }

        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name"); //Bu şekilde name key'ine ait value'yu alıyoruz.
            //ViewBag.name = name;

            //string product1 = _distributedCache.GetString("product:1"); //Bu şekilde name key'ine ait value'yu alıyoruz.       

            Byte[] byteProduct = _distributedCache.Get("product:2");

            string product2 = Encoding.UTF8.GetString(byteProduct); //Byte formatındaki veriyi bu şekilde okuyabiliriz.

            Product p = JsonConvert.DeserializeObject<Product>(product2);


            ViewBag.product1 = p;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name"); //Bu şekilde name key'ine ait value'yu siliyoruz.
            return View();
        }

        //Image gibi büyük verileri byte dizisine çevirerek cache'leyebiliriz.
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path); //ReadAllBytes methodu ile yukardaki path'i byte dizisine çeviriyoruz.

            _distributedCache.Set("resim", imageByte); //Set methodu ile cache'e atıyoruz.

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("resim"); //Get methodu ile cache'den alıyoruz.
            return File(imageByte, "image/jpg"); //File methodu ile byte dizisini image formatında döndürüyoruz.

        }
    }
}
