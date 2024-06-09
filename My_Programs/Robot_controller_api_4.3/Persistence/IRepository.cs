using System.Collections.Generic;
using Npgsql;

namespace robot_controller_api.Persistence
{
    public interface IRepository
    {
        public List<T> ExecuteReader<T>(string sqlCommand, NpgsqlParameter[] dbParams = null)
            where T : class, new()
        {
            var entities = new List<T>();

            //*Note
            //DatabaseConfig.ConnectionString Added instead of CONNECTION_STRING
            using var conn = new NpgsqlConnection(DatabaseConfig.CONNECTION_STRING);
            
            conn.Open();
            using var cmd = new NpgsqlCommand(sqlCommand, conn);
            // Some of our SQL commands might have SQL parameters we will need to pass to DB. MS
            if (dbParams is not null)
            {
                // CommandType is unnecessary for PostgreSQL but can be used in other DB engines like Oracle or SQL Server. MS
                // cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(dbParams.Where(x => x.Value is not null).ToArray());
            }
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var entity = new T();
                dr.MapTo(entity); // AUTOMATIC ORM WILL HAPPEN HERE. MapTo does not exist yet. MS
                entities.Add(entity);
            }
            return entities;
        }
    }
}
