    using Dapper;
using Microsoft.Data.SqlClient;
using TrackCure.Interfaces;
using TrackCure.Models;

namespace TrackCure.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;
        public PatientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }


        public async Task<bool> DeletePatient(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [Patient] WHERE PatientId = @PatientId";
            var rows = await connection.ExecuteAsync(sql, new {PatientId = id});
            return rows > 0;
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [Patient]";
            return await  connection.QueryAsync<Patient>(sql);
        }

        public async Task<Patient?> GetPatientById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Patient>(
                   "SELECT * FROM [Patient] WHERE PatientId = @PatientId", new { PatientId = id });
            
        }

        public async Task<bool> RegisterPatient(Patient patient)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [Patient] (Gender, DOB, Address, Phone, AddedBy, AdmissionDate, DischargeDate, BillingStatus, Symptom, Note, FullName) 
                  VALUES (@Gender, @DOB, @Address, @Phone, @AddedBy, @AdmissionDate, @DischargeDate, @BillingStatus, @Symptom, @Note, @FullName)";
            var rows = await connection.ExecuteAsync(sql, patient);
            return rows > 0;
        }

        public async Task<bool>  UpdatePatient(int id, Patient patient)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE [Patient] SET 
            Gender = @Gender, 
            DOB = @DOB,
            Address = @Address,
            Phone = @Phone,
            AddedBy = @AddedBy,
            AdmissionDate = @AdmissionDate,
            DischargeDate = @DischargeDate,
            BillingStatus = @BillingStatus,
            Symptom = @Symptom,
            Note = @Note,
            FullName = @FullName WHERE PatientId = @PatientId";

            var rows = await connection.ExecuteAsync(sql, new
            {
                PatientId = id,
                patient.Gender,
                patient.DOB,
                patient.Address,
                patient.Phone,
                patient.AddedBy,
                patient.AdmissionDate,
                patient.DischargeDate,
                patient.BillingStatus,
                patient.Symptom,
                patient.Note,
                patient.FullName
            });
            return rows > 0;
        }
    }
}
