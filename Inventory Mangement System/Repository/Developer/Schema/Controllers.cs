using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository.Developer.Schema
{
    public class Controllers
    {
        public Model.Developer.Controller One(int id)
        {
            using (var c = new ProductInventoryDataContext())
            {
                var q = (from x in c.DevControllers
                         where x.CId == id
                         select new Model.Developer.Controller()
                         {
                             CId = x.CId,
                             ControllerName = x.ControllerName,
                             ControllerType = new IntegerString() { Id = x.ControllerTypeId, Text = x.DevControllerType.ControllerType},
                             ParentController = new IntegerNullString() { Id = (int)x.ParentControllerId, Text = x.DevController_ParentControllerId.ControllerName },
                             Status = x.Status,
                             IsVisible = x.IsVisible,
                             NeedAuthorisation = x.NeedAuthorisation,
                             NeedParentAuthorisation = x.NeedParentAuthorisation,
                             NeedLogin = x.NeedLogin,
                             LoginType = new IntegerNullString() { Id = (int)x.LoginTypeId, Text = x.Role.RoleName },
                             Ordinal = x.Ordinal,
                             DisplayText = x.DisplayText,
                             URL = x.URL,
                             SqlCommand = x.SqlCommand,
                             SqlCommandKeys = x.SqlCommandKeys,
                             EditController = x.EditController,
                             CreateController = x.CreateController,
                             SqlCommandOrderBy = x.SqlCommandOrderBy,
                             ChildControllers = x.ChildControllers,
                             ControlController = x.ControlController,

                         }).SingleOrDefault();
                return q;
            }
        }

        public Model.Developer.Controller One(string con)
        {
            using (var c = new ProductInventoryDataContext())
            {
                var q = (from x in c.DevControllers
                         where x.ControllerName == con
                         select new Model.Developer.Controller()
                         {
                             CId = x.CId,
                             ControllerName = x.ControllerName,
                             ControllerType = new IntegerString() { Id = x.ControllerTypeId, Text = x.DevControllerType.ControllerType },
                             ParentController = new IntegerNullString() { Id = (int)x.ParentControllerId, Text = x.DevController_ParentControllerId.ControllerName },
                             Status = x.Status,
                             IsVisible = x.IsVisible,
                             NeedAuthorisation = x.NeedAuthorisation,
                             NeedParentAuthorisation = x.NeedParentAuthorisation,
                             NeedLogin = x.NeedLogin,
                             LoginType = new IntegerNullString() { Id = (int)x.LoginTypeId, Text = x.Role.RoleName },
                             Ordinal = x.Ordinal,
                             DisplayText = x.DisplayText,
                             URL = x.URL,
                             SqlCommand = x.SqlCommand,
                             SqlCommandKeys = x.SqlCommandKeys,
                             EditController = x.EditController,
                             CreateController = x.CreateController,
                             SqlCommandOrderBy = x.SqlCommandOrderBy,
                             ChildControllers = x.ChildControllers,
                             ControlController = x.ControlController,

                         }).SingleOrDefault();
                return q;
            }
        }
    }
}
