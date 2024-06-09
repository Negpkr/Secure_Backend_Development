using Npgsql;
using robot_controller_api.Models; //*updated at 4.3

namespace robot_controller_api.Persistence
{
    public class RobotCommandADO : IRobotCommandDataAccess
    {
        private string CONNECTION_STRING = DatabaseConfig.CONNECTION_STRING;

        public List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
            using var dr = cmd.ExecuteReader();

            //read values off the data reader and create a new robotCommand here and then add it to the result list.

            while (dr.Read())
            {
                RobotCommand robotCommand = new RobotCommand();

                robotCommand.Id = dr.GetInt32(0);
                robotCommand.Name = dr.GetString(1);
                robotCommand.Description = dr.IsDBNull(2) ? null : dr.GetString(2);
                robotCommand.IsMoveCommand = dr.GetBoolean(3);
                robotCommand.CreatedDate = dr.GetDateTime(4);
                robotCommand.ModifiedDate = dr.GetDateTime(5);

                robotCommands.Add(robotCommand);
            }

            return robotCommands;
        }

        public RobotCommand GetRobotCommandById(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                RobotCommand robotCommand = new RobotCommand();

                robotCommand.Name = dr["Name"].ToString();
                robotCommand.Description = dr["description"] is DBNull ? null : dr["description"].ToString();
                robotCommand.IsMoveCommand = Convert.ToBoolean(dr["ismovecommand"]);
                robotCommand.CreatedDate = Convert.ToDateTime(dr["createddate"]);
                robotCommand.ModifiedDate = Convert.ToDateTime(dr["modifieddate"]);

                return robotCommand;
            }

            return null;
        }

        public List<RobotCommand> GetMoveCommandsOnly()
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
                RobotCommand robotCommand = new RobotCommand();
                
                robotCommand.Id = Convert.ToInt32(dr["id"]);
                robotCommand.Name = dr["Name"].ToString();
                robotCommand.Description = dr["description"] is DBNull ? null : dr["description"].ToString();
                robotCommand.IsMoveCommand = Convert.ToBoolean(dr["ismovecommand"]);
                robotCommand.CreatedDate = Convert.ToDateTime(dr["createddate"]);
                robotCommand.ModifiedDate = Convert.ToDateTime(dr["modifieddate"]);

                moveCommands.Add(robotCommand);
            }

            return moveCommands;
        }

        public bool CheckCommandNameExists(string commandName)
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

        public RobotCommand InsertRobotCommand(RobotCommand command)
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
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                RobotCommand robotCommand = new RobotCommand();

                robotCommand.Name = dr["Name"].ToString();
                robotCommand.Description = dr["description"] is DBNull ? null : dr["description"].ToString();
                robotCommand.IsMoveCommand = Convert.ToBoolean(dr["ismovecommand"]);
                robotCommand.CreatedDate = Convert.ToDateTime(dr["createddate"]);
                robotCommand.ModifiedDate = Convert.ToDateTime(dr["modifieddate"]);

                return robotCommand;
            }
            return null;
        }

        public RobotCommand UpdateRobotCommand(int id, RobotCommand updatedCommand)
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

            using var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                RobotCommand robotCommand = new RobotCommand();

                robotCommand.Name = dr["Name"].ToString();
                robotCommand.Description = dr["description"] is DBNull ? null : dr["description"].ToString();
                robotCommand.IsMoveCommand = Convert.ToBoolean(dr["ismovecommand"]);
                robotCommand.CreatedDate = Convert.ToDateTime(dr["createddate"]);
                robotCommand.ModifiedDate = Convert.ToDateTime(dr["modifieddate"]);

                return robotCommand;
            }
            return null;
        }

        public void DeleteRobotCommand(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM robotcommand WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
