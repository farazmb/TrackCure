using TrackCure.Dtos;
using TrackCure.Models;

namespace TrackCure.Interfaces
{
    public interface IDoctorRepository
    {
        Task<bool> RegisterDoctor(User doctor);
        Task<IEnumerable<User>> GetAllDoctors();
        Task<User?> GetDoctorById(int id);
        Task<bool> UpdateDoctor(int id, User doctor);
        Task<bool> DeleteDoctor(int id);
       
    }
}
