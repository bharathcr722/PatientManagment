using PatientManagment.Models;

namespace PatientManagment.DataAccess.ObservationDataAccess
{
    public interface IObservationDataAccess
    {
        Task<Observation> CreateObservation(ObservationModel observation);
        Task<Observation> GetObservation(int id);
        Task<StatusModel> UpdateObservation(ObservationModel observation);
        Task<StatusModel> DeleteObservation(int id);
        Task<List<Observation>> GetObservationByPatientId(int patientId);
        Task<List<Observation>> GetAllObservation();
    }
}
