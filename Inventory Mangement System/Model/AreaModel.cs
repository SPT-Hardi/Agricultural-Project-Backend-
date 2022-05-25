using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class AreaModel
    {
        public List<MainAreaModel> arealist { get; set; } = new List<MainAreaModel>();
    }
    public class MainAreaModel
    {
        [RegularExpression(@"^[a-zA-Z0-9\s]{1,50}$", ErrorMessage = "Alphanumeric , Minimum 1 & Maximum 50 Character are allowed")]
        public string MainAreaName { get; set; }

        public List<SubAreaModel> subarea { get; set; } = new List<SubAreaModel>();
    }
    public class SubAreaModel
    {
        [RegularExpression(@"^[a-zA-Z0-9\s]{1,50}$",ErrorMessage ="Alphanumeric , Minimum 1 & Maximum 50 Character are allowed")]
        public string? SubAreaName { get; set; } = null;

    }

   
}
