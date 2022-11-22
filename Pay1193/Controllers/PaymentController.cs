using Microsoft.AspNetCore.Mvc;
using Pay1193.Models;
using Pay1193.Services;

namespace Pay1193.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPayService _payService;
        private readonly IEmployee _emloyeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PaymentController(IPayService payService, IEmployee employeeService, IWebHostEnvironment webHostEnvironment)
        {
            _payService = payService;
            _emloyeeService = employeeService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var records = _payService.GetAll().Select(pay => new PaymentIndexViewModel
            {
                Id = pay.Id,
                EmloyeeId = pay.EmployeeId,
                Emloyee = _emloyeeService.GetById(pay.EmployeeId),
                FullName = pay.Employee.FullName,
                PayDate = pay.DatePay,
                PayMonth = pay.MonthPay.ToString("MMMM"),
                TaxYearId = pay.TaxYearId,
                Year = _payService.GetTaxYearById(pay.TaxYearId).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeduction = pay.TotalDeduction,
                NetPayment = pay.NetPayment,
                
            }).ToList(); 
            return View(records);
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
    }
}
