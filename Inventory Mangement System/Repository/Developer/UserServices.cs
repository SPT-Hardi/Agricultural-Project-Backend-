using Inventory_Mangement_System.Model.Developer;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository.Developer
{
    public class UserServices
    {
        public Ids GetIds(object LoginID) 
        {
            using (ProductInventoryDataContext c = new ProductInventoryDataContext())
            {

                var Ids = (from x in c.LoginDetails
                           where x.LoginID==(int)LoginID
                           select new Ids
                           {
                               URId = 0,
                               UId = x.User.UserID,
                               RId = x.User.Role.RoleID,
                               OId = 0,
                               LId = x.User.Role.RoleID,
                           }).FirstOrDefault();
                return Ids;
            }
        }
    }
}
