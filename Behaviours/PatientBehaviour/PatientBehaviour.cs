using PatientManagment.DataAccess.PatientDataAccess;
using PatientManagment.Models;

namespace PatientManagment.Behaviours.PatientBehaviour
{
    public class PatientBehaviour: IPatientBehaviour
    {
        private IPatientDataAccess _iPatientDataAccess;
        public PatientBehaviour(IPatientDataAccess iPatientDataAccess)
        {
            _iPatientDataAccess = iPatientDataAccess;
        }
        public async Task<Patient> CreatePatient(PatientDataModel patient)
        {
            return await _iPatientDataAccess.CreatePatient(patient);
        }   
        public async Task<Patient> GetPatientData(int id)
        {
            return await _iPatientDataAccess.GetPatientData(id);
        }
        public async Task<List<Patient>> GetAllPatientData()
        {
            return await _iPatientDataAccess.GetAllPatientData();
        }
        public async Task<StatusModel> UpdatePatient(PatientDataModel patient)
        {
            return await _iPatientDataAccess.UpdatePatient(patient);
        }
        public async Task<StatusModel> DeletePatient(int id)
        {
            return await _iPatientDataAccess.DeletePatient(id);
        }
    }
}
