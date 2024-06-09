
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class UserDataAccess : IUserDataAccess
    {
        private string CONNECTION_STRING = DatabaseConfig.CONNECTION_STRING;
        public UserModel GetUserByEmail(string email)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM \"user\" WHERE \"Email\" = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new UserModel
                {
                    Id = dr.GetInt32(0),
                    Email = dr.GetString(1),
                    FirstName = dr.GetString(2),
                    LastName = dr.GetString(3),
                    PasswordHash = dr.GetString(4),
                    Role = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Description = dr.IsDBNull(6) ? null : dr.GetString(6),
                    CreatedDate = dr.GetDateTime(7),
                    ModifiedDate = dr.GetDateTime(8)
                };
            }

            return null;
        }

        public void AddUser(UserModel user)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO \"user\" (\"Email\", \"FirstName\", \"LastName\", \"PasswordHash\", \"Role\", \"Description\", \"CreatedDate\", \"ModifiedDate\") VALUES (@Email, @FirstName, @LastName, @PasswordHash, @Role, @Description, @CreatedDate, @ModifiedDate)",
                conn
            );
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Role", user.Role ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", user.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
            cmd.Parameters.AddWithValue("@ModifiedDate", user.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM \"user\"", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                users.Add(new UserModel
                {
                    Id = dr.GetInt32(0),
                    Email = dr.GetString(1),
                    FirstName = dr.GetString(2),
                    LastName = dr.GetString(3),
                    PasswordHash = dr.GetString(4),
                    Role = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Description = dr.IsDBNull(6) ? null : dr.GetString(6),
                    CreatedDate = dr.GetDateTime(7),
                    ModifiedDate = dr.GetDateTime(8)
                });
            }

            return users;
        }

        public IEnumerable<UserModel> GetUsersByRole(string role)
        {
            var users = new List<UserModel>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM \"user\" WHERE \"Role\" = @Role", conn);
            cmd.Parameters.AddWithValue("@Role", role);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                users.Add(new UserModel
                {
                    Id = dr.GetInt32(0),
                    Email = dr.GetString(1),
                    FirstName = dr.GetString(2),
                    LastName = dr.GetString(3),
                    PasswordHash = dr.GetString(4),
                    Role = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Description = dr.IsDBNull(6) ? null : dr.GetString(6),
                    CreatedDate = dr.GetDateTime(7),
                    ModifiedDate = dr.GetDateTime(8)
                });
            }

            return users;
        }

        public UserModel GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM \"user\" WHERE \"Id\" = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new UserModel
                {
                    Id = dr.GetInt32(0),
                    Email = dr.GetString(1),
                    FirstName = dr.GetString(2),
                    LastName = dr.GetString(3),
                    PasswordHash = dr.GetString(4),
                    Role = dr.IsDBNull(5) ? null : dr.GetString(5),
                    Description = dr.IsDBNull(6) ? null : dr.GetString(6),
                    CreatedDate = dr.GetDateTime(7),
                    ModifiedDate = dr.GetDateTime(8)
                };
            }

            return null;
        }

        public void UpdateUser(UserModel user)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE \"user\" SET \"FirstName\" = @FirstName, \"LastName\" = @LastName, \"Role\" = @Role, \"Description\" = @Description, \"ModifiedDate\" = @ModifiedDate WHERE \"Id\" = @Id",
                conn
            );
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Role", user.Role ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", user.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedDate", user.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public void DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM \"user\" WHERE \"Id\" = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }

        public void UpdateUserPassword(int id, string newPasswordHash)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE \"user\" SET \"PasswordHash\" = @PasswordHash, \"ModifiedDate\" = @ModifiedDate WHERE \"Id\" = @Id",
                conn
            );
            cmd.Parameters.AddWithValue("@PasswordHash", newPasswordHash);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
