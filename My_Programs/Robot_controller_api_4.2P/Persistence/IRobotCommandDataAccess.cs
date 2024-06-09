using robot_controller_api.Models;

namespace robot_controller_api.Persistence;
public interface IRobotCommandDataAccess
{
    List<RobotCommand> GetRobotCommands();
    RobotCommand GetRobotCommandById(int id);
    RobotCommand InsertRobotCommand(RobotCommand command);
    RobotCommand UpdateRobotCommand(int id, RobotCommand updatedCommand);
    void DeleteRobotCommand(int id);
    List<RobotCommand> GetMoveCommandsOnly();
    bool CheckCommandNameExists(string commandName);
}
