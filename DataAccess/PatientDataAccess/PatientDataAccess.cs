using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagment.Data;
using PatientManagment.Models;
using PatientManagment.Models.Encryption;
using PatientManagment.Models.QueryModel;

namespace PatientManagment.DataAccess.PatientDataAccess
{
    public class PatientDataAccess: IPatientDataAccess
    {
        private ApplicationDbContext _context;
        public PatientDataAccess(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Patient> CreatePatient(PatientDataModel model)
        {
            var hasPassWord = Argon2Utils.HashPassword(model.Password);
            Patient patient = new Patient
            {
                Name = model.Name,
                Email = model.Email,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                PasswordHash = hasPassWord.hashPassword,
                Salt = hasPassWord.salt
            };
            var isUserPresent = await GetPatientByPhoneNumber(patient.Email);
            if (isUserPresent != null)
                return null;
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync(); 
            if (patient!=null)
            {
                patient.Salt = null;
                patient.PasswordHash = null;
            }
            return patient;
        }
        public async Task<Patient> GetPatientByPhoneNumber(string email)
        {
            return await _context.Patients.FirstOrDefaultAsync(f=> f.Email== email);
        }
        public async Task<Patient> GetPatientData(int id)
        {
            return await _context.Patients.FindAsync(id);
        }
        public async Task<List<Patient>> GetAllPatientData()
        {
            return await _context.Patients.Select(s =>
            new Patient{
                Name=s.Name,
                Id=s.Id,
                DateOfBirth=s.DateOfBirth,
                Email=s.Email
            }).ToListAsync();
        }
        public async Task<StatusModel> UpdatePatient(PatientDataModel model)
        {
            StatusModel status = new StatusModel(Status.Success);
            try
            {
                var patient = await GetPatientData(model.Id);
                if (patient != null)
                {
                    patient.Name = model.Name;
                    patient.DateOfBirth = model.DateOfBirth;
                    patient.Gender = model.Gender;
                    _context.Patients.Update(patient);
                    await _context.SaveChangesAsync();
                }
                else
                    status.Status = Status.NotFound;
            }
            catch (Exception ex)
            {
                status.Status = Status.Fail;
            }
            return status;
        }
        public async Task<StatusModel> DeletePatient(int id)
        {
            StatusModel status = new StatusModel(Status.Success);
            try
            {
                var patient = await GetPatientData(id);
                if (patient == null)
                {
                    status.Status = Status.UserNotFound;
                    return status;
                }
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status.Status = Status.Fail;
            }
            return status;
        }
    }
}
