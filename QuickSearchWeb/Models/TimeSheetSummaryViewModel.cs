using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickSearchWeb.Models
{
    public class TimeSheetSummaryViewModel
    {
        public int ID { get; set; }
        public int TimeSheetID { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

       
        [Display(Name = "Description")]
        public string Description { get; set; }


      
        
        public Decimal? Day1Hours { get; set; }
       
      
        public Decimal? Day2Hours { get; set; }
       
       
        public Decimal? Day3Hours { get; set; }
      
       
        public Decimal? Day4Hours { get; set; }

       
        public Decimal? Day5Hours { get; set; }
        
        public Decimal? Day6Hours { get; set; }
       
       
        public Decimal? Day7Hours { get; set; }

    }
}