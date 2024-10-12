using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagment.Data;
using PatientManagment.Models;

namespace PatientManagment.DataAccess.ObservationDataAccess
{
    public class ObservationDataAccess: IObservationDataAccess
    {
        private ApplicationDbContext _context;
        public ObservationDataAccess(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Observation> CreateObservation(ObservationModel observation)
        {
            Observation model = new Observation();
            model.PatientId=observation.PatientId;
            model.Note = observation.Note;
            model.Date = observation.Date;
            var patientData = await GetPatientData(observation.PatientId);
            if (patientData == null)
                return null;
            _context.Observations.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
        public async Task<Patient> GetPatientData(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<Observation> GetObservation(int id)
        {
            return await _context.Observations.FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<List<Observation>> GetObservationByPatientId(int patientId)
        {
            return await _context.Observations.Where(o => o.PatientId == patientId).ToListAsync();
        }
        public async Task<List<Observation>> GetAllObservation()
        {
            return await _context.Observations.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<StatusModel> UpdateObservation(ObservationModel observationModel)
        {
            StatusModel status = new StatusModel(Status.Success);
            try
            {
                var observation = await GetObservation(observationModel.Id);
                if (observation != null)
                {
                    observation.Note = observationModel.Note;
                    observation.Date = observationModel.Date;
                    _context.Observations.Update(observation);
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

        [HttpDelete("{id}")]
        public async Task<StatusModel> DeleteObservation(int id)
        {
            StatusModel status = new StatusModel(Status.Success);
            try
            {
                var observation = await GetObservation(id);
                if (observation == null)
                {
                    status.Status = Status.NotFound;
                    return status;
                }
                _context.Observations.Remove(observation);
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
