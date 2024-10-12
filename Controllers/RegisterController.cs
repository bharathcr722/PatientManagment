using Microsoft.AspNetCore.Mvc;
using PatientManagment.Behaviours.AdminBehaviour;
using PatientManagment.Data;
using PatientManagment.KeyStore;
using PatientManagment.Models;
using PatientManagment.Models.Encryption;
using PatientManagment.Models.QueryModel;
using System.Security.Claims;

namespace PatientManagment.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegisterController:ControllerBase
    {
        private IAdminBehaviour _adminBehaviour;
        public RegisterController(IAdminBehaviour adminBehaviour)
        {
            _adminBehaviour = adminBehaviour;
        }
        [HttpPost]
        public async Task<ActionResult<Admin>> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                    ModelState.AddModelError("", "Password is not matching;");
                else
                {
                    var user = await _adminBehaviour.RegisterUser(model);
                    if (user == null)
                        return BadRequest(Status.UserAlreadyExists.ToString());
                    else if (user.Id == 0)
                        return BadRequest("Something went wrong!");
                    else
                    {
                        return Ok(new RegisterModel { Id = user.Id, Email = user.Email, Name = user.Name });
                    }
                }

            }
            return BadRequest(ModelState);
        }
    }
}
