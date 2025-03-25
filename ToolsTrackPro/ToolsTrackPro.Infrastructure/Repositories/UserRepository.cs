namespace ToolsTrackPro.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.Data.SqlClient;
    using ToolsTrackPro.Infrastructure.Interfaces; 

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves a user by email.
        /// </summary>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT ID, Name, Email, PasswordHash, Role FROM Users WHERE Email = @Email", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PasswordHash = reader.GetString(3),
                                Role = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return null; 
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        public async Task<bool> CreateUserAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Users (Name, Email, PasswordHash, Role) VALUES (@Name, @Email, @PasswordHash, @Role)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }

}
