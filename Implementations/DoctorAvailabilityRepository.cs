using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TrackCure.Interfaces;
using TrackCure.Models;

namespace TrackCure.Implementations
{
    public class DoctorAvailabilityRepository : IDoctorAvailability
    {
        private readonly string _connectionString;
        private readonly IDoctorAvailability _doctorAvailability;
        public DoctorAvailabilityRepository(IConfiguration configuration, IDoctorAvailability doctorAvailability)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _doctorAvailability = doctorAvailability;
        }
        public async Task<bool> AddAvailabilityAsync(DoctorAvailability availability)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [DoctorAvailabilty] (UserId, AvailableDate, StartTime, EndTime) VALUES (@UserId, @AvailableDate, @StartTime, @EndTime)";
            var rows = await connection.ExecuteAsync(sql, availability);
            return rows > 0;
        }

        [HttpGet("getbyid")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<DoctorAvailability>> GetAvailabilityByDoctor()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var availability = await _doctorAvailability.GetAvailabilityByDoctor(userId);

            if (availability == null)
                return NotFound("No availability found for this doctor");

            return Ok(availability);
        }


        public async Task<bool> DeleteAvailability(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [DoctorAvailabilty] WHERE UserId = @UserId";
            var rows = await connection.ExecuteAsync(sql, new { AvailabilityId = id });
            return rows > 0;
        }   

        public async Task<bool> UpdateAvailability(int id, DoctorAvailability doctorAvailability)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE [DoctorAvailabilty] SET AvailableDate = @AvailableDate, StartTime = @StartTime, EndTime = @EndTime WHERE UserId = @UserId";
            var rows = await connection.ExecuteAsync(sql, new
            {
                UserId = id,
                AvailableDate = doctorAvailability.AvailableDate,
                StartTime = doctorAvailability.StartTime,
                EndTime = doctorAvailability.EndTime
            });

            return rows > 0;
                
        }
    }
}
