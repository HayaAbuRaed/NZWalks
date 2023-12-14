using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository) 
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        // POST: /api/Auth/Register
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerDTO.Password);

            if (identityResult.Succeeded)
            {
                if (registerDTO.Roles is not null && registerDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerDTO.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User is created successfully.");
                    }
                }
            }

            return BadRequest("Something went wring.");
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogIn([FromBody] LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.UserName);

            if (user is not null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

                if (checkPassword)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles is not null) 
                    {
                        var JwtToken = _tokenRepository.CreateJWTTokent(user, roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtToken = JwtToken,
                        };

                        return Ok(response);
                    }

                }
            }

            return BadRequest("username or password is wrong");
        }
    }
}
