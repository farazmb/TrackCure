using System.IdentityModel.Tokens.Jwt;
using Microsoft.Data.SqlClient;
using TrackCure.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackCure.Dtos;
using TrackCure.Services;
using System.Security.Claims;
using TrackCure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthWebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly JwtService _jwtService;
        private readonly IReceptionistRepository _receptionistRepository;


        public AdminController(IConfiguration configuration, JwtService jwtService, IReceptionistRepository receptionistRepository  )
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _jwtService = jwtService;
            _receptionistRepository = receptionistRepository;
        }

        [HttpPost("register-receptionist")]
        [Authorize(Roles = "Admin")]  
        public async Task<ActionResult> Register(RegisterDto request)
        {
          
            var hashedPassword = new PasswordHasher<User>();

            var receptionist = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword.HashPassword(null!, request.Password),
                RoleId = request.RoleId,    
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                Gender = request.Gender,
                DOB = request.DOB,
                Designation = request.Designation,
                Specialization = request.Specialization,
                LicenseNo = request.LicenseNo,
                Experience = request.Experience,
                IsActive = true,
                CreatedAt = DateTime.UtcNow

            };

           var success =  await _receptionistRepository.RegisterReceptionist(receptionist);

            if (!success)
            {
                return BadRequest("Registeration Failed");
            }

            return Ok("Receptionist registered successfully");
        }

     

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // GEtting the user here
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM [User] WHERE Username = @Username",
                new { request.Username });

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var hashedPassword = new PasswordHasher<User>();
            var result = hashedPassword.VerifyHashedPassword(null!, user.PasswordHash, request.Password);


            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong password");
            }
            // CHecks Role
            var role = await connection.QueryFirstOrDefaultAsync<Role>(
                "SELECT * FROM [Role] WHERE RoleId = @RoleId",
                new { RoleId = user.RoleId });

            if (role == null)
                return BadRequest("Role not found");

            //  Assigns the role to user
            user.Role = role;

            // Creates the JWT token with role
            var token = _jwtService.CreateToken(user);
            return Ok(token);
        }

        

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllReceptionists()
        {
            var receptionist = await _receptionistRepository.GetAllReceptionists();
            return Ok(receptionist);
        }

        

        [HttpGet("getbyid")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetReceptionistById(int id)
        {
            var receptionist = await _receptionistRepository.GetReceptionistById(id);
            if(receptionist == null)
            {
                return BadRequest("Receptionist not found");
            }

            return Ok(receptionist);
        }


        [HttpPut("update-receptionist")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateReceptionist(int id,  User receptionist)
        {
            var success = await _receptionistRepository.UpdateReceptionist(id, receptionist);
            if (!success)
            {
                return BadRequest("Update Failed");
            }
            return Ok("Update Sucessfully");
        }

        [HttpDelete("delete-receptionist")]
        [Authorize(Roles = "Admin")]
        public async Task <ActionResult> DeleteReceptionist(int id)
        {
            var success = await _receptionistRepository.DeleteReceptionist(id);
            if (!success)
            {
                return BadRequest("Delete Failed");
            }
            return Ok("Deleted Sucessfully");
        }





    }
}
