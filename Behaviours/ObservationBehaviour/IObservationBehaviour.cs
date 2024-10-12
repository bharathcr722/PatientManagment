using PatientManagment.Models;

namespace PatientManagment.Behaviours.ObservationBehaviour
{
    public interface IObservationBehaviour
    {
        Task<Observation> CreateObservation(ObservationModel observation);
        Task<Observation> GetObservation(int id);
        Task<StatusModel> UpdateObservation(ObservationModel observation);
        Task<StatusModel> DeleteObservation(int id);
        Task<List<Observation>> GetObservationByPatientId(int patientId);
        Task<List<Observation>> GetAllObservation();
    }
}
