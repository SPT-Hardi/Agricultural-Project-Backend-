using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ProductionModel
    {
        public string vegetablenm { get; set; }
        public Model.Common.IntegerNullString mainAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString subAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        //public Model.IntegerNullString vegetableDetails { get; set; } = new Model.IntegerNullString();

    }
}
