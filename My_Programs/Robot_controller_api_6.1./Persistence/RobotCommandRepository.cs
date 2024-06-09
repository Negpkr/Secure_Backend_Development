using Npgsql;
using robot_controller_api.Models; //*updated at 4.3

namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
    {
        private IRepository _repo => this;

        /* =>
        public RobotCommandRepository(IRepository repo)
        {
            _repo = repo;
        }
        */

        public List<RobotCommand> GetRobotCommands()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robotcommand");
            return commands;
        }

        public RobotCommand GetRobotCommandById(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new NpgsqlParameter("id", id) };
            var result = _repo
                .ExecuteReader<RobotCommand>("SELECT * FROM robotcommand WHERE id=@id", sqlParams)
                .FirstOrDefault();
            return result;
        }

        public RobotCommand InsertRobotCommand(RobotCommand newCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("Name", newCommand.Name),
                new NpgsqlParameter("description", newCommand.Description ?? (object)DBNull.Value),
                new NpgsqlParameter("isMoveCommand", newCommand.IsMoveCommand)
            };
            var result = _repo
                .ExecuteReader<RobotCommand>(
                    "INSERT INTO robotcommand (\"Name\", description, ismovecommand, createddate, modifieddate) VALUES (@name, @description, @isMoveCommand, current_timestamp, current_timestamp) RETURNING *;",
                    sqlParams
                )
                .FirstOrDefault();
            return result;
        }

        public RobotCommand UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new NpgsqlParameter("id", id),
                new NpgsqlParameter("Name", updatedCommand.Name),
                new NpgsqlParameter(
                    "description",
                    updatedCommand.Description ?? (object)DBNull.Value
                ),
                new NpgsqlParameter("isMoveCommand", updatedCommand.IsMoveCommand)
            };
            var result = _repo
                .ExecuteReader<RobotCommand>(
                    "UPDATE robotcommand SET \"Name\"=@name, description=@description, ismovecommand=@isMoveCommand, modifieddate=current_timestamp WHERE id=@id RETURNING *;",
                    sqlParams
                )
                .FirstOrDefault();
            return result;
        }

        public void DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[] { new NpgsqlParameter("id", id) };
            _repo.ExecuteReader<RobotCommand>("DELETE FROM robotcommand WHERE id=@id", sqlParams);
        }

        List<RobotCommand> IRobotCommandDataAccess.GetMoveCommandsOnly()
        {
            var sql = "SELECT * FROM robotcommand WHERE ismovecommand = true";
            var moveCommands = _repo.ExecuteReader<RobotCommand>(sql);
            return moveCommands;
        }

        bool IRobotCommandDataAccess.CheckCommandNameExists(string commandName)
        {
            var sqlParams = new NpgsqlParameter[] { new NpgsqlParameter("Name", commandName) };

            var result = _repo
                .ExecuteReader<RobotCommand>(
                    "SELECT * FROM robotcommand WHERE \"Name\" = @Name",
                    sqlParams
                )
                .FirstOrDefault();

            return result != null;
        }
    }
}
