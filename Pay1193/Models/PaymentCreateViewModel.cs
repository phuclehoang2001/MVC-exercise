﻿using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Pay1193.Models
{
    public class PaymentCreateViewModel
    {
        public int Id { get; set; }
        
        public int EmloyeeId { get; set; }
        public Employee? Emloyee { get; set; }

        [Display(Name = "Name")]
        public string? Fullname { get; set; }

        public string? Nino { get; set; }

        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Pay Month")]
        public string PayMonth { get; set; } = DateTime.Today.Month.ToString();

        [Display(Name = "Tax Year")]
        public int TaxYearId { get; set; }

        public TaxYear? TaxYear { get; set; }

        public string TaxCode { get; set; } = "1250L";

        public decimal HourlyRate { get; set; }

        public decimal HoursWorked { get; set; }

        public decimal ContractualHours { get; set; } = 144m;

        public decimal OvertimeHours { get; set; } 

        public decimal ContractualEarnings { get; set; }

        public decimal OvertimeEarnings { get; set; }

        public decimal Tax { get; set; }

        public decimal NIC { get; set; }

        public decimal? UnionFee { get; set; }

        public decimal? SLC { get; set; }

        public decimal TotalEarnings { get; set; }

        public decimal TotalDeduction { get; set; }

        public decimal NetPayment { get; set; }

    }
}
