using Microsoft.VisualBasic;
using PatientManagment.Data;
using PatientManagment.DataAccess.AdminDataAccess;
using PatientManagment.DataAccess.PatientDataAccess;
using PatientManagment.Models.Encryption;
using PatientManagment.Models;
using PatientManagment.Models.QueryModel;

namespace PatientManagment.Behaviours.AdminBehaviour
{
    public class AdminBehaviour: IAdminBehaviour
    {
        private IAdminDataAccess _iAdminDataAccess;
        public AdminBehaviour(IAdminDataAccess iAdminDataAccess)
        {
            _iAdminDataAccess = iAdminDataAccess;
        }
        public async Task<Admin> RegisterUser(RegisterModel model)
        {
            return await _iAdminDataAccess.RegisterUser(model);
        }
        public async Task<StatusModel> isAdminLoginedIn(string email, string password)
        {
            StatusModel status = new StatusModel();
            var data = await _iAdminDataAccess.GetAdminData(email);
            if (data == null)
            {
                status.Status = Status.UserNotFound;
            }
            else
            {
                bool isPasswordMatched = Argon2Utils.VerifyPassword(password, data.PasswordHash, data.Salt);
                status.Status = isPasswordMatched ? Status.Success : Status.Fail;
            }
            return status;
        }
        public async Task<StatusModel> isLoginedIn(string email, string password)
        {
            StatusModel status = new StatusModel();
            var data = await _iAdminDataAccess.GetUserData(email);
            if (data == null)
            {
                status.Status = Status.UserNotFound;
            }
            else
            {
                bool isPasswordMatched = Argon2Utils.VerifyPassword(password, data.PasswordHash, data.Salt);
                status.Status = isPasswordMatched ? Status.Success : Status.Fail;
            }
            return status;
        }
        public async Task<Admin> GetAdminData(string email)
        {
            return await _iAdminDataAccess.GetAdminData(email);

        }
        public async Task<Patient> GetUserData(string email)
        {
            return await _iAdminDataAccess.GetUserData(email);

        }
    }
}
