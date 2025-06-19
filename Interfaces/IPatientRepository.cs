using TrackCure.Models;

namespace TrackCure.Interfaces
{
    public interface IPatientRepository
    {
        Task<bool> RegisterPatient(Patient patient);
        Task<IEnumerable<Patient>> GetAllPatients();
        Task<Patient?> GetPatientById(int id);
        Task<bool> UpdatePatient(int id, Patient patient);
        Task<bool> DeletePatient(int id);
    }
}
