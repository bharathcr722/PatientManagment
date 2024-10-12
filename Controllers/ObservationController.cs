using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagment.Behaviours.ObservationBehaviour;
using PatientManagment.Behaviours.PatientBehaviour;
using PatientManagment.KeyStore;
using PatientManagment.Models;
using System.Security.Claims;

namespace PatientManagment.Controllers
{
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class ObservationController: ControllerBase
    {
        private IObservationBehaviour _iObservationBehaviour;
        public ObservationController(IObservationBehaviour iObservationBehaviour)
        {
            _iObservationBehaviour = iObservationBehaviour;
        }
        [Authorize(Policy = AppKeyStore.AdminOnly)]
        [HttpPost]
        public async Task<ActionResult<Observation>> CreateObservation(ObservationModel observation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model= await _iObservationBehaviour.CreateObservation(observation);
            if (model == null)
                return BadRequest(Status.UserNotFound.ToString());
            else
                model.Patient = null;
            return Ok(model);
        }
        [Authorize(Policy = AppKeyStore.AdminAndUser)]
        [HttpGet("{id:int?}")]
        public async Task<ActionResult<List<Observation>>> GetObservation(int? id)
        {
            var role =User.FindFirst(ClaimTypes.Role)?.Value;
            var patientId =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var observations = new List<Observation>();
            if (role== AppKeyStore.Admin)
            {
                if (id != null)
                {
                    var observation = await _iObservationBehaviour.GetObservation(id ?? 0);
                    if (observation != null)
                        observations.Add(observation);
                }
                else
                 observations = await _iObservationBehaviour.GetAllObservation();
            }
            else
            {
                observations = await _iObservationBehaviour.GetObservationByPatientId(Convert.ToInt32(patientId));
                if (id != null)
                    observations = observations.Where(f=> f.Id==(id ?? 0)).ToList();
            }
            if (observations.Count==0)
                return NotFound("No Data Available..");

            return observations;
        }
        [Authorize(Policy = AppKeyStore.AdminOnly)]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Observation>>> GetObservationByPatientId(int id)
        {
            var observations= await _iObservationBehaviour.GetObservationByPatientId(id);
            if (observations.Count == 0)
                return NotFound("No Observation Available");
            return observations;
        }
        [Authorize(Policy = AppKeyStore.AdminOnly)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservation(int id, ObservationModel observation)
        {
            if (id != observation.Id || !ModelState.IsValid)
                return BadRequest();

            StatusModel status = await _iObservationBehaviour.UpdateObservation(observation);
            if (status.Status != Status.Success)
            {
                return NotFound(status.Status.ToString());
            }
            return Ok(status.Status.ToString());
        }
        [Authorize(Policy = AppKeyStore.AdminOnly)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObservation(int id)
        {
            var statusModel = await _iObservationBehaviour.DeleteObservation(id);
            if (statusModel.Status == Status.NotFound)
                return NotFound();

            return Ok(statusModel.Status.ToString());
        }
    }
}
