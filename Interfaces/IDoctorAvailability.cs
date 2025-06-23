using TrackCure.Models;

namespace TrackCure.Interfaces
{
    public interface IDoctorAvailability
    {
        Task<bool> AddAvailabilityAsync(DoctorAvailability availability);
        Task<IEnumerable<DoctorAvailability>> GetAvailabilityByDoctor(int userId);
        Task<bool> UpdateAvailability(int id, DoctorAvailability doctorAvailability);
        Task<bool> DeleteAvailability(int id);
    }
}
