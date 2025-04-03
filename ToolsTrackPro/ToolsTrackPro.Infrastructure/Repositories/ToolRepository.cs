namespace ToolsTrackPro.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.Data.SqlClient;
    using ToolsTrackPro.Infrastructure.Interfaces;

    public class ToolRepository : IToolRepository
    {
        private readonly string _connectionString;

        public ToolRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new tool (Admin).
        /// </summary>
        public async Task<bool> AddAsync(Tool tool)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Tools (Name, Description, StatusID, CreatedAt) VALUES (@Name, @Description, @StatusID, GETDATE())", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", tool.Name);
                    cmd.Parameters.AddWithValue("@Description", tool.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@StatusID", tool.StatusId);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        /// <summary>
        /// Gets all tools.
        /// </summary>
        public async Task<IEnumerable<Tool>> GetAllAsync()
        {
            List<Tool> tools = new List<Tool>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT ID, Name, Description, StatusID, CreatedAt FROM Tools", conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tools.Add(new Tool
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                StatusId = reader.GetByte(3),
                                CreatedAt = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            return tools;
        }

        /// <summary>
        /// Gets a tool by ID.
        /// </summary>
        public async Task<Tool?> GetByIdAsync(int toolId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT ID, Name, Description, StatusID, CreatedAt FROM Tools WHERE ID = @ToolID", conn))
                {
                    cmd.Parameters.AddWithValue("@ToolID", toolId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Tool
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                StatusId = reader.GetByte(3),
                                CreatedAt = reader.GetDateTime(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Updates tool details.
        /// </summary>
        public async Task<bool> UpdateAsync(Tool tool)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("UPDATE Tools SET Name = @Name, Description = @Description, StatusID = @StatusID WHERE ID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", tool.Id);
                    cmd.Parameters.AddWithValue("@Name", tool.Name);
                    cmd.Parameters.AddWithValue("@Description", tool.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@StatusID", tool.StatusId);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        /// <summary>
        /// Deletes a tool by ID.
        /// </summary>
        public async Task<bool> DeleteAsync(int toolId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Tools WHERE ID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", toolId);
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        /// <summary>
        /// Get tools with Latest Transaction.
        /// </summary>
        public async Task<IEnumerable<ToolTransaction>> GetToolsWithLatestTransactionAsync()
        {
            var tools = new List<ToolTransaction>();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(@"
                    WITH LatestTransaction AS (
                        SELECT 
                            t.ToolID, 
                            t.UserID, 
                            u.Name AS UserName, 
                            t.BorrowDate, 
                            t.DueDate, 
                            t.ReturnDate, 
                            t.UpdatedAt,
                            ROW_NUMBER() OVER (PARTITION BY t.ToolID ORDER BY t.UpdatedAt DESC) AS rn
                        FROM Transactions t
                        LEFT JOIN Users u ON t.UserID = u.ID
                    )
                    SELECT 
                        tl.ID AS ToolId, 
                        tl.Name AS ToolName, 
                        ts.Name AS StatusName, 
                        lt.UserID, 
                        lt.UserName, 
                        lt.BorrowDate, 
                        lt.DueDate, 
                        lt.ReturnDate,
                        lt.UpdatedAt
                    FROM Tools tl
                    JOIN ToolStatus ts ON tl.StatusID = ts.ID
                    LEFT JOIN LatestTransaction lt ON tl.ID = lt.ToolID AND lt.rn = 1
                    ORDER BY lt.UpdatedAt DESC;", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tools.Add(new ToolTransaction
                            {
                                ToolId = reader.GetInt32(0),
                                ToolName = reader.GetString(1),
                                StatusName = reader.GetString(2),
                                UserId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                UserName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                BorrowDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                DueDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                ReturnDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                                UpdatedAt = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8)
                            });
                        }
                    }
                }
            }
            return tools;
        }

    }

}
