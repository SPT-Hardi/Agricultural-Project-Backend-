using Inventory_Mangement_System.Model.Developer;
using Microsoft.AspNetCore.Http;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository.Developer.Contact
{
    public class Current
    {
        private static IHttpContextAccessor _HttpContextAccessor;

        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            _HttpContextAccessor = accessor;
        }
        public static HttpContext httpContext
        {
            get
            {
                return _HttpContextAccessor.HttpContext;
            }
        }
        public static Ids Ids
        {
            get
            {
                return (Ids)httpContext.Items["Ids"];
            }
        }
        public static bool IsLoggedIn
        {
            get
            {
                return Ids == null ? false : true;
            }
        }




        public static bool SetCId(int? CId)
        {
            Ids CurrentIds = httpContext.Items["Ids"] as Ids;
            CurrentIds.CId = CId;
            httpContext.Items["Ids"] = CurrentIds;
            return true;
        }
        public static bool IsAuthorised(string Con, string ParentCon = null)
        {
            using (var c = new ProductInventoryDataContext())
            {
                var cc = Ids;
                if (c.IsAuthorised(Con, ParentCon, cc.UId, cc.RId, cc.OId, cc.LId) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



    }
}

