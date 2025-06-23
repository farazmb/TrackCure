using TrackCure.Dtos;

namespace TrackCure.Interfaces
{
    public interface IAdminRepository
    {
        Task<DashboardDto> GetDashboardStatsAsync();
    }
}
