using Microsoft.Data.SqlClient;
using TrackCure.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackCure.Dtos;
using TrackCure.Services;
using TrackCure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace TrackCure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly JwtService _jwtService;
        private readonly IDoctorRepository _doctorRepository;
        public ReceptionistController(JwtService jwtService, IConfiguration configuration, IDoctorRepository doctorRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _jwtService = jwtService;
            _doctorRepository = doctorRepository;

        }

        [HttpPost("register-doctor")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult> CreateDoctor(RegisterDto request)
        {
            var hashedPassword = new PasswordHasher<User>();
            

            const int docttorRoleId = 3;

            var doctor = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword.HashPassword(null!, request.Password),
                RoleId = docttorRoleId,
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
                CreatedAt = DateTime.UtcNow,
                
            };

            var success = await _doctorRepository.RegisterDoctor(doctor);
            if (!success)
            {
                return BadRequest("Registeration Failed");
            }

            return Ok("Doctor registered successfully");

            
        }

        [HttpPost("login")]

        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            //get user
            var user = await connection.QueryFirstOrDefaultAsync<User>(
               "SELECT * FROM [User] WHERE Username = @Username", new { request.Username });
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var hashedPassword = new PasswordHasher<User>();
            var result = hashedPassword.VerifyHashedPassword(null!, user.PasswordHash, request.Password);

            //check pass
            if(result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong Password");
            }

            //check role
            var role = await connection.QueryFirstOrDefaultAsync<Role>(
                "SELECT * FROM [Role] WHERE RoleId = @RoleId", 
                new { RoleId = user.RoleId });

            if(role == null)
            {
                return BadRequest("Role not found");
            }


            //assign role
            user.Role = role;



            //create jwt token
            var token = _jwtService.CreateToken(user);
            return Ok(token);

        }

        [HttpGet]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllDoctors()
        {
            var doctor = await _doctorRepository.GetAllDoctors();
            return Ok(doctor);
        }



        [HttpGet("getbyid")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<IEnumerable<User>>> GetDoctorById(int id)
        {
            var doctor = await _doctorRepository.GetDoctorById(id);
            if (doctor == null)
            {
                return BadRequest("Doctor not found");
            }

            return Ok(doctor);
        }


        [HttpPut("update-doctor")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult> UpdateDoctor(int id, User doctor)
        {
            var success = await _doctorRepository.UpdateDoctor(id, doctor);
            if (!success)
            {
                return BadRequest("Update Failed");
            }
            return Ok("Update Sucessfully");
        }

        [HttpDelete("delete-doctor")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            var success = await _doctorRepository.DeleteDoctor(id);
            if (!success)
            {
                return BadRequest("Delete Failed");
            }
            return Ok("Deleted Sucessfully");
        }



    }
}
