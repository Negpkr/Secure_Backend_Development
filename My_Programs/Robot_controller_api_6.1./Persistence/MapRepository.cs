using Npgsql;
using robot_controller_api.Models; //*updated at 4.3

namespace robot_controller_api.Persistence
{
    public class MapRepository : IMapDataAccess, IRepository
    {
        private IRepository _repo => this;

        /* =>
        public MapRepository(IRepository repo)
        {
            _repo = repo;
        }
        */

        public List<Map> GetMaps()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM map");
            return maps;
        }

        public Map GetMapById(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new NpgsqlParameter("id", id) };
            var result = _repo
                .ExecuteReader<Map>("SELECT * FROM map WHERE id=@id", sqlParams)
                .FirstOrDefault();
            return result;
        }

        public Map InsertMap(Map newMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("Name", newMap.Name),
                new NpgsqlParameter("rows", newMap.Rows),
                new NpgsqlParameter("columns", newMap.Columns),
                new NpgsqlParameter("description", newMap.Description ?? (object)DBNull.Value)
            };
            var result = _repo
                .ExecuteReader<Map>(
                    "INSERT INTO map (\"Name\", rows, columns, description, createddate, modifieddate) VALUES (@name, @rows, @columns, @description, current_timestamp, current_timestamp) RETURNING *;",
                    sqlParams
                )
                .FirstOrDefault();
            return result;
        }

        public Map UpdateMap(int id, Map updatedMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", id),
                new NpgsqlParameter("Name", updatedMap.Name),
                new NpgsqlParameter("rows", updatedMap.Rows),
                new NpgsqlParameter("columns", updatedMap.Columns),
                new NpgsqlParameter("description", updatedMap.Description ?? (object)DBNull.Value)
            };
            var result = _repo
                .ExecuteReader<Map>(
                    "UPDATE map SET \"Name\"=@name, description=@description, rows=@rows, columns=@columns, modifieddate=current_timestamp WHERE id=@id RETURNING *;",
                    sqlParams
                )
                .FirstOrDefault();
            return result;
        }

        public void DeleteMap(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new NpgsqlParameter("id", id) };
            _repo.ExecuteReader<Map>("DELETE FROM map WHERE id=@id", sqlParams);
        }

        public List<Map> GetSquareMaps()
        {
            var sql = "SELECT * FROM map WHERE columns = rows";
            var squareMaps = _repo.ExecuteReader<Map>(sql);
            return squareMaps;
        }

        public bool CheckMapNameExists(string mapName)
        {
            var sqlParams = new NpgsqlParameter[] { new NpgsqlParameter("Name", mapName) };

            var result = _repo
                .ExecuteReader<Map>("SELECT * FROM map WHERE \"Name\" = @Name", sqlParams)
                .FirstOrDefault();

            return result != null;
        }
    }
}
