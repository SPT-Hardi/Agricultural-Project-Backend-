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
        public string mname { get; set; }
        public List<SubAreaModel> subarea { get; set; } = new List<SubAreaModel>();
    }

    public class SubAreaModel
    {
       public string sname { get; set; }
    }

}
