
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class RobotCommandEF : IRobotCommandDataAccess
    {
        private readonly RobotContext _context;

        public RobotCommandEF(RobotContext context)
        {
            _context = context;
        }

        public List<RobotCommand> GetRobotCommands()
        {
            return _context.Robotcommands.ToList();
        }

        public RobotCommand GetRobotCommandById(int id)
        {
            return _context.Robotcommands.FirstOrDefault(r => r.Id == id);
        }

        public RobotCommand InsertRobotCommand(RobotCommand newCommand)
        {
            //*************************************************** added
            newCommand.CreatedDate = newCommand.ModifiedDate = DateTime.Now;

            _context.Robotcommands.Add(newCommand);
            _context.SaveChanges();
            return newCommand;
        }

        public RobotCommand UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var command = _context.Robotcommands.FirstOrDefault(r => r.Id == id);
            if (command != null)
            {
                command.Name = updatedCommand.Name;
                command.Description = updatedCommand.Description;
                command.IsMoveCommand = updatedCommand.IsMoveCommand;
                //*added
                command.ModifiedDate = DateTime.Now;

                _context.SaveChanges();
            }

            return command;
        }

        public void DeleteRobotCommand(int id)
        {
            var command = _context.Robotcommands.FirstOrDefault(r => r.Id == id);
            if (command != null)
            {
                _context.Robotcommands.Remove(command);
                _context.SaveChanges();
            }
        }

        public List<RobotCommand> GetMoveCommandsOnly()
        {
            return _context.Robotcommands.Where(r => r.IsMoveCommand).ToList();
        }

        public bool CheckCommandNameExists(string commandName)
        {
            return _context.Robotcommands.Any(r => r.Name == commandName);
        }
    }
}
