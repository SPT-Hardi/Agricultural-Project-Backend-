using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IAccountRepository
    {
        Result AddRole(RoleModel roleModel);

        //View All User
        Task<IEnumerable> ViewAllUser();

        Result RegisterUser(UserModel userModel);
        //View User By Id
        Task<IEnumerable> ViewUserById(int userID);
        Result LoginUser(LoginModel loginModel);
    }
}