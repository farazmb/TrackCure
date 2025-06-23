using TrackCure.Models;

namespace TrackCure.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<bool> AddEquipment(Equipment equipment);
        Task<IEnumerable<Equipment>> GetAllAvailableEquipments();
        Task<Equipment?> GetEquipmentById(int id);
        Task<bool> UpdateEquipment(int id, Equipment equipment);
        Task<bool> DeleteEquipment(int id);
    }
}
