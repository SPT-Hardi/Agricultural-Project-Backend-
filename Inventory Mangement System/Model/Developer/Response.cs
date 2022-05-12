using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model.Developer
{
    public class Response
    {
        public enum ModeTypes
        {
            None,
            Simple,
            Advanced,
            Procedure
        }
        public class Data
        {
            public List<Dictionary<string, object>> Records { get; set; } = new List<Dictionary<string, object>>();
            public int Total { get; set; } = 0;
            public int CurrentTotal { get; set; } = 0;
            public string Filter { get; set; } = "";
            public string Sort { get; set; } = "";
            public int PageSize { get; set; } = 0;
            public int PageNumber { get; set; } = 0;
        }
        public class Structure
        {
            public string Con { get; set; } = "";
            public ModeTypes Mode { get; set; } = ModeTypes.Simple;
            public Schema Schema { get; set; } = new Schema();
            public RelatedControllers Options { get; set; } = new RelatedControllers();
        }
        public class RelatedControllers
        {
            public string CreateCon { get; set; } = "";
            public string DeleteCon { get; set; } = "";
            public string EditCon { get; set; } = "";
            public string ViewCon { get; set; } = "";
            public string PrintCon { get; set; } = "";
        }
    }
}
