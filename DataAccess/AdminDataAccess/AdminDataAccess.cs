using Microsoft.EntityFrameworkCore;
using PatientManagment.Data;
using PatientManagment.KeyStore;
using PatientManagment.Models;
using PatientManagment.Models.Encryption;
using PatientManagment.Models.QueryModel;

namespace PatientManagment.DataAccess.AdminDataAccess
{
    public class AdminDataAccess: IAdminDataAccess
    {
        private ApplicationDbContext _appDbContext;
        public AdminDataAccess(ApplicationDbContext context)
        {
            _appDbContext = context;
        }
        public async Task<Admin> RegisterUser(RegisterModel model)
        {
            try
            {
                var adminData = await GetUserData(model.Email);
                if (adminData!=null)
                {
                    return null;
                }

                var hasPassWord = Argon2Utils.HashPassword(model.Password);
                Admin admindata = new Admin()
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = hasPassWord.hashPassword,
                    Salt = hasPassWord.salt
                };
                await _appDbContext.Admin.AddAsync(admindata);
                await _appDbContext.SaveChangesAsync();
                return admindata;
            }
            catch (Exception ex)
            {
            }
            return new Admin();
        }
        public async Task<Patient> GetUserData(string email)
        {
            return await _appDbContext.Patients.SingleOrDefaultAsync(a => a.Email == email);

        }
        public async Task<Admin> GetAdminData(string email)
        {
            return await _appDbContext.Admin.SingleOrDefaultAsync(a => a.Email == email);

        }
    }
}
