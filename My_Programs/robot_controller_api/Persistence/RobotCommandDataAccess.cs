using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public static class RobotCommandDataAccess
    {
        // Connection string is usually set in a config file for ease of change.
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=neg;Database=postgres";

        public static List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
            using var dr = cmd.ExecuteReader();

            //read values off the data reader and create a new robotCommand here and then add it to the result list.

            while (dr.Read())
            {
                var id = dr.GetInt32(0);
                var name = dr.GetString(1);
                var description = dr.IsDBNull(2) ? null : dr.GetString(2);
                var isMoveCommand = dr.GetBoolean(3);
                var createdDate = dr.GetDateTime(4);
                var modifiedDate = dr.GetDateTime(5);

                var robotCommand = new RobotCommand(
                    id,
                    name,
                    isMoveCommand,
                    createdDate,
                    modifiedDate,
                    description
                );
                robotCommands.Add(robotCommand);
            }

            return robotCommands;
        }

        public static RobotCommand GetRobotCommandById(int id)
        {
            RobotCommand robotCommand = null;

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                var name = dr["Name"].ToString();
                var description = dr["description"] is DBNull ? null : dr["description"].ToString();
                var isMoveCommand = Convert.ToBoolean(dr["ismovecommand"]);
                var createdDate = Convert.ToDateTime(dr["createddate"]);
                var modifiedDate = Convert.ToDateTime(dr["modifieddate"]);

                robotCommand = new RobotCommand(
                    id,
                    name,
                    isMoveCommand,
                    createdDate,
                    modifiedDate,
                    description
                );
            }

            return robotCommand;
        }

        public static List<RobotCommand> GetMoveCommandsOnly()
        {
            var moveCommands = new List<RobotCommand>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT * FROM robotcommand WHERE ismovecommand = true",
                conn
            );

            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var id = Convert.ToInt32(dr["id"]);
                var name = dr["Name"].ToString();
                var description = dr["description"] is DBNull ? null : dr["description"].ToString();
                var isMoveCommand = Convert.ToBoolean(dr["ismovecommand"]);
                var createdDate = Convert.ToDateTime(dr["createddate"]);
                var modifiedDate = Convert.ToDateTime(dr["modifieddate"]);

                var robotCommand = new RobotCommand(
                    id,
                    name,
                    isMoveCommand,
                    createdDate,
                    modifiedDate,
                    description
                );
                moveCommands.Add(robotCommand);
            }

            return moveCommands;
        }

        public static bool CheckCommandNameExists(string commandName)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT COUNT(*) FROM robotcommand WHERE \"Name\" = @CommandName",
                conn
            );
            cmd.Parameters.AddWithValue("@CommandName", commandName);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public static void InsertRobotCommand(RobotCommand command)
        {
            //*Q: How change ID
            // Generate a new unique Id
            //int newId = RobotCommandDataAccess.GetRobotCommands().Count > 0 ? RobotCommandDataAccess.GetRobotCommands().Max(c => c.Id) + 1 : 1;
            //It must create the unique ID


            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO robotcommand (\"Name\", Description, IsMoveCommand, CreatedDate, ModifiedDate) VALUES (@Name, @Description, @IsMoveCommand, @CreatedDate, @ModifiedDate)",
                conn
            );
            cmd.Parameters.AddWithValue("@Name", command.Name);
            cmd.Parameters.AddWithValue(
                "@Description",
                command.Description ?? (object)DBNull.Value
            );
            cmd.Parameters.AddWithValue("@IsMoveCommand", command.IsMoveCommand);
            cmd.Parameters.AddWithValue("@CreatedDate", command.CreatedDate);
            cmd.Parameters.AddWithValue("@ModifiedDate", command.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public static void UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE robotcommand SET \"Name\" = @Name, Description = @Description, IsMoveCommand = @IsMoveCommand, ModifiedDate = @ModifiedDate WHERE Id = @Id",
                conn
            );
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", updatedCommand.Name);
            cmd.Parameters.AddWithValue(
                "@Description",
                updatedCommand.Description ?? (object)DBNull.Value
            );
            cmd.Parameters.AddWithValue("@IsMoveCommand", updatedCommand.IsMoveCommand);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public static void DeleteRobotCommand(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM robotcommand WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
