namespace ToolsTrackPro.Infrastructure.Interfaces
{
    using System.Threading.Tasks;
    using ToolsTrackPro.Domain.Entities;

    public interface ITransactionRepository
    {
        Task<bool> BorrowToolAsync(int userId, int toolId, DateTime borrowDate, DateTime dueDate);
        Task<bool> ReturnToolAsync(int userId, int toolId, DateTime returnDate);
        Task<List<TransactionView>> GetTransactionsByToolIdAsync(int toolId);
    }

}
