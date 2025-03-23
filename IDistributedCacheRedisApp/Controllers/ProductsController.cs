using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}
