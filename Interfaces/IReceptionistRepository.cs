using TrackCure.Models;

namespace TrackCure.Interfaces
{
    public interface IReceptionistRepository
    {
        Task<bool> RegisterReceptionist(User receptionist); 
        Task<IEnumerable<User>> GetAllReceptionists();
        Task<User?> GetReceptionistById(int id);
        Task<bool> UpdateReceptionist(int id, User doctor);
        Task<bool> DeleteReceptionist(int id);
        



    }
}
