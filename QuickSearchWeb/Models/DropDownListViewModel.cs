using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.Models
{
    public class DropDownListViewModel
    {
        public int Id { get; set; }
        public string IdString { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
    }
}