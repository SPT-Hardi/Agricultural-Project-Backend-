using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IAccountRepository
    {
        Result AddRole(RoleModel roleModel);
        Result RegisterUser(UserModel userModel);
        Result LoginUser(LoginModel loginModel);
    }
}