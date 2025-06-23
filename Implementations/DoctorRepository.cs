using Dapper;
using Microsoft.Data.SqlClient;
using TrackCure.Interfaces;
using TrackCure.Models;

namespace TrackCure.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;
        public DoctorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<bool> RegisterDoctor(User doctor)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [User] (Username, PasswordHash, RoleId, Name, Address, Phone, Email, Gender, DOB, Designation, Specialization, LicenseNo, Experience, IsActive, CreatedAT)
                        VALUES (@Username, @PasswordHash, @RoleId, @Name, @Address, @Phone, @Email, @Gender, @DOB, @Designation, @Specialization, @LicenseNo, @Experience, @IsActive, @CreatedAt)";
            var rows = await connection.ExecuteAsync(sql, doctor);
            return rows > 0;
        }

       public async Task<bool> DeleteDoctor(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [User] WHERE UserId = @UserId AND RoleId = 3";
            var rows = await connection.ExecuteAsync(sql, new { UserId = id });
            return rows > 0;
        }

        public async Task<IEnumerable<User>> GetAllDoctors()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [User] WHERE RoleId = 3";
            return await connection.QueryAsync<User>(sql);
        }
                
        public async Task<User?> GetDoctorById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(
             "SELECT * FROM [User] WHERE UserId = @UserId and RoleId = 3", new {UserId = id});
            
        }

       public  async Task<bool> UpdateDoctor(int id, User doctor)
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
            Specialization = @Specialization,
            LicenseNo = @LicenseNo,
            Experience = @Experience
            WHERE UserId = @UserId AND RoleId = 3";
            //return await connection.ExecuteAsync(sql, doctor);

            var rows = await connection.ExecuteAsync(sql, new
            {
                UserId = id,
                doctor.Name,
                doctor.Address,
                doctor.Phone,
                doctor.Email,
                doctor.Gender,
                doctor.DOB,
                doctor.Designation,
                doctor.Specialization,
                doctor.LicenseNo,
                doctor.Experience
            });

            return rows > 0;

        }
    }
}
