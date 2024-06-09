using Npgsql;
using robot_controller_api.Models; //*updated at 4.3

namespace robot_controller_api.Persistence
{
    public class MapADO : IMapDataAccess
    {
        private string CONNECTION_STRING = DatabaseConfig.CONNECTION_STRING;

        public List<Map> GetMaps()
        {
            var maps = new List<Map>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Map mainMap = new Map();

                mainMap.Id = dr.GetInt32(0);
                mainMap.Name = dr.GetString(1);
                mainMap.Description = dr.IsDBNull(2) ? null : dr.GetString(2);
                mainMap.Columns = dr.GetInt32(3);
                mainMap.Rows = dr.GetInt32(4);
                mainMap.CreatedDate = dr.GetDateTime(5);
                mainMap.ModifiedDate = dr.GetDateTime(6);

                maps.Add(mainMap);
            }

            return maps;
        }

        public Map GetMapById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Map mainMap = new Map();

                mainMap.Id = dr.GetInt32(0);
                mainMap.Name = dr.GetString(1);
                mainMap.Description = dr.IsDBNull(2) ? null : dr.GetString(2);
                mainMap.Columns = dr.GetInt32(3);
                mainMap.Rows = dr.GetInt32(4);
                mainMap.CreatedDate = dr.GetDateTime(5);
                mainMap.ModifiedDate = dr.GetDateTime(6);

                return mainMap;
            }

            return null;
        }

        public bool CheckMapNameExists(string mapName)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM map WHERE \"Name\" = @MapName",
                conn
            );
            cmd.Parameters.AddWithValue("@MapName", mapName);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public List<Map> GetSquareMaps()
        {
            var squareMaps = new List<Map>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE Columns = Rows", conn);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Map mainMap = new Map();

                mainMap.Id = dr.GetInt32(0);
                mainMap.Name = dr.GetString(1);
                mainMap.Description = dr.IsDBNull(2) ? null : dr.GetString(2);
                mainMap.Columns = dr.GetInt32(3);
                mainMap.Rows = dr.GetInt32(4);

                squareMaps.Add(mainMap);
            }

            return squareMaps;
        }

        public Map InsertMap(Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO map (\"Name\", Description, Columns, Rows, CreatedDate, ModifiedDate) VALUES (@Name, @Description, @Columns, @Rows, @CreatedDate, @ModifiedDate)",
                conn
            );
            cmd.Parameters.AddWithValue("@Description", map.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Columns", map.Columns);
            cmd.Parameters.AddWithValue("@Rows", map.Rows);
            cmd.Parameters.AddWithValue("@Name", map.Name);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

            using var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Map mainMap = new Map();

                mainMap.Name = dr["Name"].ToString();
                mainMap.Description = dr.IsDBNull(2) ? null : dr.GetString(2);
                mainMap.Columns = dr.GetInt32(3);
                mainMap.Rows = dr.GetInt32(4);
                mainMap.CreatedDate = dr.GetDateTime(5);
                mainMap.ModifiedDate = dr.GetDateTime(6);

                return mainMap;
            }

            return null;
        }

        public Map UpdateMap(int id, Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE map SET Columns = @Columns, Rows = @Rows, \"Name\" = @Name, "
                    + "Description = @Description, ModifiedDate = @ModifiedDate WHERE Id = @Id",
                conn
            );
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Columns", map.Columns);
            cmd.Parameters.AddWithValue("@Rows", map.Rows);
            cmd.Parameters.AddWithValue("@Name", map.Name);
            cmd.Parameters.AddWithValue("@Description", map.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

            using var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Map mainMap = new Map();

                mainMap.Id = dr.GetInt32(0);
                mainMap.Name = dr.GetString(1);
                mainMap.Description = dr.IsDBNull(2) ? null : dr.GetString(2);
                mainMap.Columns = dr.GetInt32(3);
                mainMap.Rows = dr.GetInt32(4);
                mainMap.CreatedDate = dr.GetDateTime(5);
                mainMap.ModifiedDate = dr.GetDateTime(6);

                return mainMap;
            }

            return null;
        }

        public void DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM map WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
