using DTOs;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDTO> GetDashboardDataAsync();
        Task<int> CleanupOldOrdersAsync(int daysOld = 7);
    }
}