using Microsoft.AspNetCore.Mvc;
using Pay1193.Models;
using Pay1193.Services;

namespace Pay1193.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPayService _payService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PaymentController(IPayService payService, IWebHostEnvironment webHostEnvironment)
        {
            _payService = payService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
    }
}
