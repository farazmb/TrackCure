using Dapper;
using Microsoft.Data.SqlClient;
using TrackCure.Interfaces;
using TrackCure.Models;

namespace TrackCure.Implementations
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly string _connectionString;
        public EquipmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<bool> AddEquipment(Equipment equipment)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [Equipment] (Name, Type, Status) VALUES (@Name, @Type, @Status)";
            var rows = await connection.ExecuteAsync(sql, equipment);
            return rows > 0;
        }


        public async Task<bool> DeleteEquipment(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [Equipment] WHERE EquipmentId = @EquipmentId";
            var rows = await connection.ExecuteAsync(sql, new { EquipmentId = id });
            return rows > 0;
        }


        public async Task<IEnumerable<Equipment>> GetAllAvailableEquipments()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [Equipment] WHERE Status = 'Available'";
            return await connection.QueryAsync<Equipment>(sql);

        }

        public async Task<Equipment?> GetEquipmentById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Equipment>(
               "SELECT * FROM [Equipment] WHERE EquipmentId = @EquipmentId", new { EquipmentId = id });
        }

        public async Task<bool> UpdateEquipment(int id, Equipment equipment)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE [Equipment] SET 
                Name = @Name,
                Type = @Type,
                Status = @Status
                WHERE Equipment Id = @EquipmentId";

            var rows = await connection.ExecuteAsync(sql, new
            {
                EquipmentId = id,
                Name = equipment.Name,
                Type = equipment.Type,
                Status = equipment.Status

            });

            return rows > 0;
        }
    }
}
