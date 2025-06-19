using Dapper;
using Microsoft.Data.SqlClient;
using TrackCure.Interfaces;
using TrackCure.Models;

namespace TrackCure.Implementations
{
    public class DoctorAvailabilityRepository : IDoctorAvailability
    {
        private readonly string _connectionString;
        public DoctorAvailabilityRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<bool> AddAvailabilityAsync(DoctorAvailability availability)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [DoctorAvailabilty] (UserId, AvailableDate, StartTime, EndTime) VALUES (@UserId, @AvailableDate, @StartTime, @EndTime)";
            var rows = await connection.ExecuteAsync(sql, availability);
            return rows > 0;
        }

        public async Task<IEnumerable<DoctorAvailability>> GetAvailabilityByDoctor(int Id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [DoctorAvailabilty] WHERE UserId = @UserId";
            return await  connection.QueryAsync<DoctorAvailability>(sql, new {UserId = Id});
        }

        public async Task<bool> DeleteAvailability(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [DoctorAvailabilty] WHERE AvailabilityId = AvailabilityId";
            var rows = await connection.ExecuteAsync(sql, new { AvailabilityId = id });
            return rows > 0;
        }   

        public async Task<bool> UpdateAvailability(int id, DoctorAvailability doctorAvailability)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE [DoctorAvailabilty] SET AvailableDate = @AvailableDate, StartTime = @StartTime, EndTime = @EndTime WHERE AvailabilityId = @AvailabilityId";
            var rows = await connection.ExecuteAsync(sql, new
            {
                AvailabilityId = id,
                AvailableDate = doctorAvailability.AvailableDate,
                StartTime = doctorAvailability.StartTime,
                EndTime = doctorAvailability.EndTime
            });

            return rows > 0;
                
        }
    }
}
