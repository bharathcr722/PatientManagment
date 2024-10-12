using PatientManagment.Data;
using PatientManagment.Models;
using PatientManagment.Models.QueryModel;

namespace PatientManagment.DataAccess.AdminDataAccess
{
    public interface IAdminDataAccess
    {
        Task<Admin> RegisterUser(RegisterModel model);
        Task<Admin> GetAdminData(string email);
        Task<Patient> GetUserData(string email);
    }
}
