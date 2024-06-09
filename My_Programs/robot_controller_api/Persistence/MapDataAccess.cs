using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public static class MapDataAccess
    {
        // Connection string is usually set in a config file for ease of change.
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=neg;Database=postgres";

        public static List<Map> GetMaps()
        {
            var maps = new List<Map>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var id = dr.GetInt32(0);
                var columns = dr.GetInt32(1);
                var rows = dr.GetInt32(2);
                var name = dr.GetString(3);
                var description = dr.IsDBNull(4) ? null : dr.GetString(4);
                var createdDate = dr.GetDateTime(5);
                var modifiedDate = dr.GetDateTime(6);

                var map = new Map(id, columns, rows, name, description, createdDate, modifiedDate);
                maps.Add(map);
            }

            return maps;
        }

        public static Map GetMapById(int id)
        {
            Map map = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                var columns = dr.GetInt32(1);
                var rows = dr.GetInt32(2);
                var name = dr.GetString(3);
                var description = dr.IsDBNull(4) ? null : dr.GetString(4);
                var createdDate = dr.GetDateTime(5);
                var modifiedDate = dr.GetDateTime(6);

                map = new Map(id, columns, rows, name, description, createdDate, modifiedDate);
            }

            return map;
        }

        public static bool CheckMapNameExists(string mapName)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM map WHERE name = @MapName", conn);
            cmd.Parameters.AddWithValue("@MapName", mapName);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

         public static List<Map> GetSquareMaps()
        {
            var squareMaps = new List<Map>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE Columns = Rows", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var id = dr.GetInt32(0);
                var columns = dr.GetInt32(1);
                var rows = dr.GetInt32(2);
                var name = dr.GetString(3);
                var description = dr.IsDBNull(4) ? null : dr.GetString(4);
                var createdDate = dr.GetDateTime(5);
                var modifiedDate = dr.GetDateTime(6);

                var map = new Map(id, columns, rows, name, description, createdDate, modifiedDate);
                squareMaps.Add(map);
            }

            return squareMaps;
        }

        public static void InsertMap(Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO map (Columns, Rows, Name, Description, CreatedDate, ModifiedDate) " +
                "VALUES (@Columns, @Rows, @Name, @Description, @CreatedDate, @ModifiedDate)",
                conn
            );
            cmd.Parameters.AddWithValue("@Columns", map.Columns);
            cmd.Parameters.AddWithValue("@Rows", map.Rows);
            cmd.Parameters.AddWithValue("@Name", map.Name);
            cmd.Parameters.AddWithValue("@Description", map.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", map.CreatedDate);
            cmd.Parameters.AddWithValue("@ModifiedDate", map.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public static void UpdateMap(int id, Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE map SET Columns = @Columns, Rows = @Rows, Name = @Name, " +
                "Description = @Description, ModifiedDate = @ModifiedDate WHERE Id = @Id",
                conn
            );
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Columns", map.Columns);
            cmd.Parameters.AddWithValue("@Rows", map.Rows);
            cmd.Parameters.AddWithValue("@Name", map.Name);
            cmd.Parameters.AddWithValue("@Description", map.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedDate", map.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public static void DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM map WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
