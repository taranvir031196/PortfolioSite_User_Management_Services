using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Portfolio_Site_UserManagement_Services.Dtos;
using Portfolio_Site_UserManagement_Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Portfolio_Site_UserManagement_Services.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/v1/authenticate")]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var appRole = new ApplicationRole { Name = request.Role };
            var createRole = await _roleManager.CreateAsync(appRole);

            return Ok(new { message = "role created succesfully" });
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        private async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null)
                {
                    return new RegisterResponse { Message = "User already exists", Success = false };
                }

                userExists = new ApplicationUser
                {
                    fullName = request.fullName,
                    Email = request.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username,
                };
                var createUserResult = await _userManager.CreateAsync(userExists, request.Password);
                if (!createUserResult.Succeeded) return new RegisterResponse { Message = $"Create user failed {createUserResult?.Errors?.First()?.Description}", Success = false };
                var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, "USER");
                if (!addUserToRoleResult.Succeeded) return new RegisterResponse { Message = $"Create user succeeded but could not add user to role {addUserToRoleResult?.Errors?.First()?.Description}", Success = false };
                return new RegisterResponse
                {
                    Success = true,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse { Message = ex.Message, Success = false };
            }
        }

        [EnableCors("MyPolicy")]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await LoginAsync(request);

            return result.Success ? Ok(result) : BadRequest(result.Message);


        }

        private async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null) return new LoginResponse { Message = "Invalid email/password", Success = false };

                //all is well
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(30);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Login Successful",
                    Email = user?.Email,
                    Success = true,
                    UserId = user?.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse { Message = ex.Message, Success = false };

            }
        }

        [EnableCors("MyPolicy")]
        [HttpPut]
        [Route("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateRequest request)
        {
            var result = await UpdateAsync(request);
            
            return result.Success ? Ok(result) : BadRequest();

        }

        private async Task<UpdateResponse> UpdateAsync(UpdateRequest request)
        {
                try
                {
                    var userExists = await _userManager.FindByEmailAsync(request.Email);
                    if (userExists == null)
                    {
                        return new UpdateResponse { Message = "User not found", Success = false };
                    }

                userExists.fullName = request.fullName;
                userExists.UserName = request.Username;
                 

                var updateUserResult = await _userManager.UpdateAsync(userExists);
                if (!updateUserResult.Succeeded) return new UpdateResponse { Message = $"Update user failed {updateUserResult?.Errors?.First()?.Description}", Success = false };

                return new UpdateResponse
                {
                    Success = true,
                    Message = "User updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new UpdateResponse { Message = ex.Message, Success = false };
            }
        }

        [EnableCors("MyPolicy")]
        [HttpDelete]
        [Route("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteRequest request)
        {
            var result = await DeleteUserAsync(request);

            return result.Success ? Ok(result) : BadRequest();

        }

        private async Task<DeleteResponse> DeleteUserAsync(DeleteRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);

                if (userExists == null)
                {
                    return new DeleteResponse { Message = "User not found. Please provide a valid user", Success = false };
                }

                var deleteUserResult = await _userManager.DeleteAsync(userExists);
                if (!deleteUserResult.Succeeded) return new DeleteResponse { Message = $"Delete user failed {deleteUserResult?.Errors?.First()?.Description}", Success = false };
                return new DeleteResponse
                {
                    Success = true,
                    Message = "User deleted successfully :)"
                };

            }
            catch (Exception ex)
            {
                return new DeleteResponse { Message = ex.Message, Success = false };

            }

        }

        //[HttpPost]
        //[Route("logout")]
        //private async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        //{
        //    var result = await LogoutAsync(request)


        //}

        //private Task LogoutAsync(LogoutRequest request)
        //{
        //}
    }
}
