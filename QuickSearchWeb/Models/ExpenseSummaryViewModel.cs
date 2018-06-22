using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.Models
{
    public class ExpenseSummaryViewModel
    {
        public int Id { get; set; }

        public int ExpenseId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public string Description { get; set; }

        [Display(Name = "Miles(mi)")]
        public decimal? Miles { get; set; }

        public decimal? MilesMultiplicationFactor { get; set; }

        [Display(Name = "MilesDriven($)")]
        public decimal? MilesDriven { get; set; }

        [Display(Name = "Transport($)")]
        public decimal? Transport { get; set; }

        [Display(Name = "Lodging($)")]
        public decimal? Lodging { get; set; }

        [Display(Name = "Meals($)")]
        public decimal? Meals { get; set; }

        [Display(Name = "Phone($)")]
        public decimal? Phone { get; set; }

        [Display(Name = "Other($)")]
        public decimal? Other { get; set; }


        [Display(Name = "Total($)")]
        public decimal? Total { get; set; }
    }
}