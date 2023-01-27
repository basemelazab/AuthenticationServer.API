using AuthenticationServer.API.Model;
using AuthenticationServer.API.Model.Requests;
using AuthenticationServer.API.Model.Response;
using AuthenticationServer.API.Model.Users;
using AuthenticationServer.API.Service.Authenticators;
using AuthenticationServer.API.Service.PasswordHasher;
using AuthenticationServer.API.Service.RefreshTokenRepositories;
using AuthenticationServer.API.Service.TokenGenerators;
using AuthenticationServer.API.Service.TokenValidators;
using AuthenticationServer.API.Service.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationServer.API.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Authenticator _authenticator;
        public AuthenticationController(IUserRepository userRepository, IPasswordHasher passwordHasher, 
            AccessTokenGenerator accessTokenGenerator,RefreshTokenGenerator refreshTokenGenerator,
            RefreshTokenValidator refreshTokenValidator,IRefreshTokenRepository refreshTokenRepository,Authenticator authenticator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
            _authenticator = authenticator;
            _refreshTokenRepository = refreshTokenRepository;
        }
        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Password doesnot match confirm password"));
            }
            User exsistingUserByEmail= await _userRepository.GetByEmail(registerRequest.Email);
            if(exsistingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email is already exsist."));
            }
            User exsistingUserByUserName = await _userRepository.GetByUserName(registerRequest.UserName);
            if (exsistingUserByUserName != null)
            {
                return Conflict(new ErrorResponse("UserName is already exsist."));
            }
            string PasswordHasher=_passwordHasher.HashPassword(registerRequest.Password);
            User registerUser = new User()
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                PasswordHash= PasswordHasher
            };
            await _userRepository.Create(registerUser);
            return Ok();

        }

     

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            User user = await _userRepository.GetByUserName(loginRequest.UserName);
            if(user == null)
            {
                return Unauthorized();
            }
            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!isCorrectPassword)
            {
                return Unauthorized();
            }
            AuthenticatedUserResponse response  = await _authenticator.Authenticated(user);
            return Ok(response);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token."));
            }
            
            RefreshToken refreshTokenDTO=await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if(refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token."));
            }

            await _refreshTokenRepository.Delete(refreshTokenDTO.Id);

            User user = await _userRepository.GetById(refreshTokenDTO.UserId);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found."));
            }
            AuthenticatedUserResponse response = await _authenticator.Authenticated(user);
            return Ok(response);
        }
        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");
            if (!Guid.TryParse(rawUserId,out Guid userId))
            {
                return Unauthorized();
            }
            await _refreshTokenRepository.DeleteAll(userId);
            return NoContent();
        }
    }
}
