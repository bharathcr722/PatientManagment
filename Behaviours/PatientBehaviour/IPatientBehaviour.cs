using Microsoft.AspNetCore.Mvc;
using PatientManagment.Models;

namespace PatientManagment.Behaviours.PatientBehaviour
{
    public interface IPatientBehaviour
    {
        Task<Patient> CreatePatient(PatientDataModel patient);
        Task<Patient> GetPatientData(int id);
        Task<List<Patient>> GetAllPatientData();
        Task<StatusModel> UpdatePatient(PatientDataModel patient);
        Task<StatusModel> DeletePatient(int id);
    }
}
