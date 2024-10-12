using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagment.Behaviours.PatientBehaviour;
using PatientManagment.KeyStore;
using PatientManagment.Models;
using System.Data;
using System.Security.Claims;

namespace PatientManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PatientsController:ControllerBase
    {
        private IPatientBehaviour _iPatientBehaviour;
        public PatientsController(IPatientBehaviour iPatientBehaviour)
        {
            _iPatientBehaviour = iPatientBehaviour;
        }
        [Authorize(Policy =AppKeyStore.AdminOnly)]
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient([FromBody] PatientDataModel patient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patientData= await _iPatientBehaviour.CreatePatient(patient);
            if(patientData==null)
                return Conflict(Status.UserAlreadyExists.ToString());
            return patientData;
        }
        [Authorize(Policy = AppKeyStore.AdminAndUser)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            if (!isAdmin())
            {
                int loggedUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if(loggedUserId!=id)
                   return Forbid();
            }
            var patient = await _iPatientBehaviour.GetPatientData(id);
            if (patient == null)
                return NotFound();
            else
            {
                patient.PasswordHash = null;
                patient.Salt = null;

            }
            return patient;
        }
        [Authorize(Policy = AppKeyStore.AdminOnly)]
        [HttpGet]
        public async Task<List<Patient>> GetAllPatientData()
        {
            var patient = await _iPatientBehaviour.GetAllPatientData();
            return patient;
        }
        private bool isAdmin()
        {
            string loggedinRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return string.Equals(loggedinRole,AppKeyStore.Admin);
        }
        [Authorize(Policy = AppKeyStore.AdminOnly)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientDataModel patient)
        {
            if (id != patient.Id || !ModelState.IsValid)
                return BadRequest();

            StatusModel status = await _iPatientBehaviour.UpdatePatient(patient);
            if (status.Status!= Status.Success)
            {
                return NotFound(status.Status.ToString());
            }
            return Ok(status.Status.ToString());
        }
        [Authorize(Policy = AppKeyStore.AdminOnly)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var statusModel = await _iPatientBehaviour.DeletePatient(id);
            if (statusModel.Status==Status.UserNotFound)
                return NotFound(Status.UserNotFound.ToString());

            return Ok(statusModel.Status.ToString());
        }
    }
}
