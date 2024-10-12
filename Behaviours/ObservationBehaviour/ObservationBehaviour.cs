using PatientManagment.DataAccess.ObservationDataAccess;
using PatientManagment.Models;

namespace PatientManagment.Behaviours.ObservationBehaviour
{
    public class ObservationBehaviour: IObservationBehaviour
    {
        private IObservationDataAccess _iObservationDataAccess;
        public ObservationBehaviour(IObservationDataAccess iObservationDataAccess)
        {
            _iObservationDataAccess = iObservationDataAccess;
        }
        public async Task<Observation> CreateObservation(ObservationModel observation){
            return await _iObservationDataAccess.CreateObservation(observation); 
        }
        public async Task<Observation> GetObservation(int id)
        {
            return await _iObservationDataAccess.GetObservation(id);
        }
        public async Task<StatusModel> UpdateObservation(ObservationModel observation)
        {
            return await _iObservationDataAccess.UpdateObservation(observation);
        }
        public async Task<StatusModel> DeleteObservation(int id)
        {
            return await _iObservationDataAccess.DeleteObservation(id);
        }
        public async Task<List<Observation>> GetObservationByPatientId(int patientId)
        {
            return await _iObservationDataAccess.GetObservationByPatientId(patientId);
        }
        public async Task<List<Observation>> GetAllObservation()
        {
            return await _iObservationDataAccess.GetAllObservation();
        }
    }
}
