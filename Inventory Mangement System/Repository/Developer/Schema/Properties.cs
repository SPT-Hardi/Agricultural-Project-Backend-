using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository.Developer.Schema
{
    public class Properties
    {
        public class Property
        {
            public string Value(string ControllerName, string PropertyName)
            {
                using (var c = new ProductInventoryDataContext())
                {
                    //var Ids = Model.Developer.Ids;
                    return (from x in c.DevControllers
                            where x.ControllerName == ControllerName
                            select

                                   PropertyName == "URL" ? x.URL :
                                   PropertyName == "SqlCommand" ? x.SqlCommand :
                                   PropertyName == "SqlCommandKeys" ? x.SqlCommandKeys :
                                   PropertyName == "SqlCommandOrderBy" ? x.SqlCommandOrderBy :
                                   PropertyName == "DisplayText" ? x.DisplayText :
                                   PropertyName == "EditController" ? x.EditController
                                   /*PropertyName==*/
                                   : ""

                            ).FirstOrDefault();
                }
            }
        }
    }
}
