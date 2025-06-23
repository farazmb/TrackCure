using TrackCure.Models;

namespace TrackCure.Interfaces
{
    public interface IBedRepository
    {
        Task<bool> AddBed(Bed bed);
        Task<IEnumerable<Bed>> GetAllAvailableBeds();
        Task<Bed?> GetBedById(int id);
        Task<bool> UpdateBed(int id, Bed bed);
        Task<bool> DeleteBed(int id);
    }
}
