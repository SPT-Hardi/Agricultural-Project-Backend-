using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IAccountRepository
    {
        //Add Role
        Result AddRole(RoleModel roleModel);

        //View All User
        Task<IEnumerable> ViewAllUser();
        //User Registration
        Result RegisterUser(UserModel userModel);
        //User Login
        Result LoginUser(LoginModel loginModel);
    }
}