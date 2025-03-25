namespace ToolsTrackPro.Infrastructure.Repositories
{
    using Microsoft.Data.SqlClient;
    using System;
    using System.Threading.Tasks;
    using ToolsTrackPro.Domain.Entities;
    using ToolsTrackPro.Infrastructure.Interfaces;

    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;

        public TransactionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Borrow a tool.
        /// </summary>
        public async Task<bool> BorrowToolAsync(int userId, int toolId, DateTime borrowDate, DateTime dueDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Check if tool is available
                        using (SqlCommand checkToolCmd = new SqlCommand("SELECT StatusID FROM Tools WHERE ID = @ID", conn, transaction))
                        {
                            checkToolCmd.Parameters.AddWithValue("@ID", toolId);
                            object? result = await checkToolCmd.ExecuteScalarAsync();

                            if (result == null || (byte)result != 1) // '1' means Available
                            {
                                throw new InvalidOperationException("Tool is not available for borrowing.");
                            }
                        }

                        // Insert into Transactions
                        using (SqlCommand cmd = new SqlCommand(
                            @"INSERT INTO Transactions (UserID, ToolID, BorrowDate, DueDate, StatusID, CreatedAt, UpdatedAt)
                          VALUES (@UserID, @ToolID, @BorrowDate, @DueDate, 1, GETDATE(), GETDATE())", conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@ToolID", toolId);
                            cmd.Parameters.AddWithValue("@BorrowDate", borrowDate);
                            cmd.Parameters.AddWithValue("@DueDate", dueDate);

                            int rowsAffected = await cmd.ExecuteNonQueryAsync();

                            if (rowsAffected == 0)
                            {
                                throw new Exception("Failed to insert transaction.");
                            }
                        }

                        // Update tool status to Borrowed
                        using (SqlCommand updateToolCmd = new SqlCommand("UPDATE Tools SET StatusID = 2 WHERE ID = @ID", conn, transaction))
                        {
                            updateToolCmd.Parameters.AddWithValue("@ID", toolId);
                            await updateToolCmd.ExecuteNonQueryAsync();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all transactions for a specific tool ID in descending order.
        /// </summary>
        public async Task<List<TransactionView>> GetTransactionsByToolIdAsync(int toolId)
        {
            var transactions = new List<TransactionView>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(@"
                SELECT 
                    t.ToolID,
                    tool.Name AS ToolName,
                    u.Name AS UserName,
                    t.CreatedAt AS TransactionDate,
                    t.BorrowDate,
                    t.ReturnDate,
                    t.DueDate,
                    ts.Name AS TransactionStatus
                FROM Transactions t
                INNER JOIN Tools tool ON t.ToolID = tool.ID
                INNER JOIN Users u ON t.UserID = u.ID
                INNER JOIN TransactionStatus ts ON t.StatusID = ts.ID
                WHERE t.ToolID = @ToolID
                ORDER BY t.CreatedAt DESC;", conn))
                {
                    cmd.Parameters.AddWithValue("@ToolID", toolId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            transactions.Add(new TransactionView
                            {
                                ToolId = reader.GetInt32(0),
                                ToolName = reader.GetString(1),
                                UserName = reader.GetString(2),
                                TransactionDate = reader.GetDateTime(3),
                                BorrowDate = reader.GetDateTime(4),
                                ReturnDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                DueDate = reader.GetDateTime(6),
                                TransactionStatus = reader.GetString(7)
                            });
                        }
                    }
                }
            }
            return transactions;
        }

        /// <summary>
        /// Return a tool.
        /// </summary>
        public async Task<bool> ReturnToolAsync(int userId, int toolId, DateTime returnDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Check if user has borrowed this tool
                        using (SqlCommand checkCmd = new SqlCommand(
                            "SELECT ID FROM Transactions WHERE UserID = @UserID AND ToolID = @ToolID AND StatusID = 1", conn, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@UserID", userId);
                            checkCmd.Parameters.AddWithValue("@ToolID", toolId);

                            object? result = await checkCmd.ExecuteScalarAsync();
                            if (result == null)
                            {
                                throw new InvalidOperationException("No active transaction found for this tool.");
                            }
                        }

                        // Update transaction as returned
                        using (SqlCommand updateCmd = new SqlCommand(
                            "UPDATE Transactions SET ReturnDate = @ReturnDate, StatusID = 2, UpdatedAt = GETDATE() WHERE UserID = @UserID AND ToolID = @ToolID AND StatusID = 1", conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@UserID", userId);
                            updateCmd.Parameters.AddWithValue("@ToolID", toolId);
                            updateCmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                            int affectedRows = await updateCmd.ExecuteNonQueryAsync();

                            if (affectedRows == 0)
                            {
                                throw new InvalidOperationException("Failed to update transaction.");
                            }
                        }

                        // Update tool status to Available
                        using (SqlCommand updateToolCmd = new SqlCommand("UPDATE Tools SET StatusID = 1 WHERE ID = @ID", conn, transaction))
                        {
                            updateToolCmd.Parameters.AddWithValue("@ID", toolId);
                            await updateToolCmd.ExecuteNonQueryAsync();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }

}
