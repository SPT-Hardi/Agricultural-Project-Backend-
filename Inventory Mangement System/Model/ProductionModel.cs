using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ProductionModel
    {
        public string vegetablenm { get; set; }
        public Model.IntegerNullString mainAreaDetails { get; set; } = new Model.IntegerNullString();
        public Model.IntegerNullString subAreaDetails { get; set; } = new Model.IntegerNullString();
    }
}
