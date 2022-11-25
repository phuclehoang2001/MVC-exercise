﻿using Microsoft.AspNetCore.Mvc;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Services;
using Pay1193.Services.Implement;

namespace Pay1193.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPayService _payService;
        private readonly IEmployee _emloyeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceService _nationalInsuranceService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PaymentController(IPayService payService, IEmployee employeeService, ITaxService taxService,
            INationalInsuranceService nationalInsuranceService, IWebHostEnvironment webHostEnvironment)
        {
            _payService = payService;
            _emloyeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceService = nationalInsuranceService;
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
                PayMonth = pay.MonthPay,
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
            ViewBag.emloyees = _emloyeeService.GetAll();
            ViewBag.taxYears = _payService.GetAllTaxYear();
            var model = new PaymentCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                decimal overtimeHours = _payService.OverTimeHours(model.HoursWorked, model.ContractualHours);
                decimal contractualEarnings = _payService.ContractualEarning(model.ContractualHours, model.HoursWorked, model.HourlyRate);
                decimal overtimeEarnings = _payService.OvertimeEarnings(_payService.OvertimeRate(model.HourlyRate), overtimeHours);
                decimal totalEarnings = _payService.TotalEarnings(overtimeEarnings,contractualEarnings);
                decimal tax = _taxService.TaxAmount(totalEarnings);
                decimal unionFee = _emloyeeService.UnionFee(model.EmloyeeId);
                decimal studentLoan = _emloyeeService.StudentLoanRepaymentAmount(model.EmloyeeId,totalEarnings);
                decimal nationalInsurance = _nationalInsuranceService.NIContribution(totalEarnings);
                decimal totalDeduction = _payService.TotalDeduction(tax, nationalInsurance, studentLoan, unionFee);
                decimal netPayment = _payService.NetPay(totalEarnings, totalDeduction);
                var pay = new PaymentRecord()
                {
                    Id = model.Id,
                    EmployeeId = model.EmloyeeId,            
                    DatePay = model.PayDate,
                    MonthPay = model.PayMonth,
                    TaxYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HourWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours = overtimeHours,
                    ContractualEarnings = contractualEarnings,
                    OvertimeEarnings  = overtimeEarnings,
                    TotalEarnings = totalEarnings,
                    Tax = tax,
                    SLC =  studentLoan,
                    NiC = nationalInsurance,
                    TotalDeduction = totalDeduction,
                    NetPayment = netPayment,
                };
                await _payService.CreateAsync(pay);
                return RedirectToAction("Index");
            }
            ViewBag.emloyees = _emloyeeService.GetAll();
            ViewBag.taxYears = _payService.GetAllTaxYear();
            
            return View(model);
        }
    }
}
