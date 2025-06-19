using Dapper;
using Microsoft.Data.SqlClient;
using TrackCure.Interfaces;
using TrackCure.Models;

namespace TrackCure.Implementations
{
    public class BedRepository : IBedRepository 
    {
        private readonly string _connectionString;
        public BedRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<bool> AddBed(Bed bed)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [Bed] (BedNumber, RoomNumber, Status) VALUES (@BedNumber, @RoomNumber, @Status)";
            var rows = await connection.ExecuteAsync(sql, bed);
            return rows > 0;
        }

        public async Task<bool> DeleteBed(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [Bed] WHERE BedId = @BedId";
            var rows = await connection.ExecuteAsync(sql, new {BedId = id});
            return rows > 0;
        }

        public async Task<IEnumerable<Bed>> GetAllAvailableBeds()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [Bed]";
            return await connection.QueryAsync<Bed>(sql);
            
        }

        public async Task<Bed?> GetBedById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Bed>(
               "SELECT * FROM [Bed] WHERE BedId = @BedId", new {BedId = id});
        }

        public async Task<bool> UpdateBed(int id, Bed bed)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE [Bed] SET 
                BedNumber = @BedNumber,
                RoomNumber = @RoomNumber,
                Status = @Status
                WHERE BedId = @BedId";

            var rows = await connection.ExecuteAsync(sql, new
            {
                BedId = id,
                BedNumber = bed.BedNumber,
                RoomNumber = bed.RoomNumber,
                Status = bed.Status

            });

            return rows > 0;
        }
    }
}
