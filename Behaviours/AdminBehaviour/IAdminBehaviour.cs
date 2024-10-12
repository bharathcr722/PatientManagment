using PatientManagment.Data;
using PatientManagment.Models;
using PatientManagment.Models.QueryModel;

namespace PatientManagment.Behaviours.AdminBehaviour
{
    public interface IAdminBehaviour
    {
        Task<Admin> RegisterUser(RegisterModel model);
        Task<StatusModel> isAdminLoginedIn(string email, string password);
        Task<StatusModel> isLoginedIn(string email, string password);
        Task<Patient> GetUserData(string email);
        Task<Admin> GetAdminData(string email);
    }
}
