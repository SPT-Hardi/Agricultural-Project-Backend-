using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ProductModel
    {
        public string  ProductName { get; set; }
        public string Variety { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public Model.Common.IntegerNullString categorytype { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString type { get; set; } = new Model.Common.IntegerNullString();
    }
}
