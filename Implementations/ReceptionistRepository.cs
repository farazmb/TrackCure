using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using TrackCure.Interfaces;
using TrackCure.Models;

namespace TrackCure.Implementations
{
    public class ReceptionistRepository : IReceptionistRepository
    {
        private readonly string _connectionString;
        public ReceptionistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<bool> DeleteReceptionist(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [User] WHERE UserId = @UserId and RoleId = 2";
            var rows = await connection.ExecuteAsync(sql, new { UserId = id });
            return rows > 0;
        }

        public async Task<IEnumerable<User>> GetAllReceptionists()
        {
           using var  connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [User] WHERE RoleId = 2";
            return await  connection.QueryAsync<User>(sql);
        }   

        public async Task<bool> RegisterReceptionist(User receptionist)
        {
           using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [User] (Username, PasswordHash, RoleId, Name, Address, Phone, Email, Gender, DOB, Designation, Specialization, LicenseNo, Experience, IsActive, CreatedAT)
                        VALUES (@Username, @PasswordHash, @RoleId, @Name, @Address, @Phone, @Email, @Gender, @DOB, @Designation, @Specialization, @LicenseNo, @Experience, @IsActive, @CreatedAt)";
            var rows =  await connection.ExecuteAsync(sql, receptionist);
            return rows > 0;
        }

        public async Task<User?> GetReceptionistById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(
             "SELECT * FROM [User] WHERE UserId = @UserId and RoleId = 2", new { UserId = id });

        }

        public async Task<bool> UpdateReceptionist(int id, User receptionist)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE [User] SET 
            Name = @Name,
            Address = @Address,
            Phone = @Phone,
            Email = @Email,
            Gender = @Gender,
            DOB = @DOB,
            Designation = @Designation,
            Experience = @Experience
            WHERE Id = @UserId AND RoleId = 2";
            var rows = await connection.ExecuteAsync(sql, new
            {
                UserId = id,
                receptionist.Name,
                receptionist.Address,
                receptionist.Phone,
                receptionist.Email,
                receptionist.Gender,
                receptionist.DOB,
                receptionist.Designation,
                receptionist.Experience
            });

            return rows > 0;

        }
    }
}
