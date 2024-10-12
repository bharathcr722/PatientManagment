using Microsoft.AspNetCore.Mvc;
using PatientManagment.Behaviours.AdminBehaviour;
using PatientManagment.KeyStore;
using PatientManagment.Models;
using PatientManagment.Models.Encryption;
using PatientManagment.Models.QueryModel;
using System.Security.Claims;

namespace PatientManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController:ControllerBase
    {
        private JwtTokenGenerator _tokenGenerator;
        private IAdminBehaviour _adminBehaviour;
        public AuthController(JwtTokenGenerator tokenGenerator, IAdminBehaviour adminBehaviour)
        {
            _tokenGenerator = tokenGenerator;
            _adminBehaviour= adminBehaviour;
        }

        [HttpPost]
        public async Task<ActionResult> AdminLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminBehaviour.isAdminLoginedIn(model.Email, model.Password);
                if (result.Status == Status.Success)
                {
                    var user = await _adminBehaviour.GetAdminData(model.Email);
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.Name),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role,AppKeyStore.Admin)
                    };
                    var token = _tokenGenerator.GenerateToken(claims);
                    return Ok(new { token });
                }
                else
                {
                    return BadRequest(AppKeyStore.InvalidEmailAndPassword);
                }
            }
            return BadRequest(model);
        }
        [HttpPost]
        public async Task<ActionResult> UserLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminBehaviour.isLoginedIn(model.Email, model.Password);
                if (result.Status == Status.Success)
                {
                    var user = await _adminBehaviour.GetUserData(model.Email);
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.Name),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role,AppKeyStore.User)
                    };
                    var token = _tokenGenerator.GenerateToken(claims);

                    return Ok(new { token });
                }
                else
                {
                    return BadRequest(AppKeyStore.InvalidEmailAndPassword);
                }
            }
            return BadRequest(model);
        }
    }
}
