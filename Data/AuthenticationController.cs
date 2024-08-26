using CoreApiInNet.Contracts;
using CoreApiInNet.Model;
using CoreApiInNet.Users;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiInNet.Data
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly InterfaceAuthManager authManager;

        public AuthenticationController(InterfaceAuthManager authManager)
        {
            this.authManager = authManager;
        }
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto userDto)
        {
            var errors = await authManager.Register(userDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);

                }
                return BadRequest(ModelState);
            }
            return Ok();
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> login(ApiUserDto userDto)
        {
            
            var authResponse = await authManager.login(userDto);
            if (authResponse == null)
            {
                return Unauthorized("User does not exist");
            }
            else
            {

                return Ok(authResponse);
            }
        }
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Refreshtoken(AuthResponse response)
        {

            var authResponse = await authManager.VerifyToken(response);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            else
            {

                return Ok(authResponse);
            }

        }
    }
}
