using PatientManagment.Models;

namespace PatientManagment.DataAccess.PatientDataAccess
{
    public interface IPatientDataAccess
    {
        Task<Patient> CreatePatient(PatientDataModel patient);
        Task<Patient> GetPatientData(int id);
        Task<List<Patient>> GetAllPatientData();
        Task<StatusModel> UpdatePatient(PatientDataModel patient);
        Task<StatusModel> DeletePatient(int id);
    }
}
