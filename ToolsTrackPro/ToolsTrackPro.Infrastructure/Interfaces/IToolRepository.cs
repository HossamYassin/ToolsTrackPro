namespace ToolsTrackPro.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IToolRepository
    {
        Task<bool> AddAsync(Tool tool);
        Task<IEnumerable<Tool>> GetAllAsync();
        Task<Tool?> GetByIdAsync(int toolId);
        Task<bool> UpdateAsync(Tool tool);
        Task<bool> DeleteAsync(int toolId);
        Task<List<ToolTransaction>> GetToolsWithLatestTransactionAsync();
    }

}
