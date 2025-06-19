using Microsoft.Data.SqlClient;
using TrackCure.Dtos;
using Dapper;
using TrackCure.Interfaces;

namespace TrackCure.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString;
        public AdminRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<DashboardDto> GetDashboardStatsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var dto = new DashboardDto
            {
                TotalPatients = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [Patient]"),
                TotalDoctors = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [User] WHERE RoleId = 3"),
                TotalReceptionists = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [User] WHERE RoleId = 2"),
                AppointmentsToday = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Appointment")
            };

            return dto;
        }
    }
}
