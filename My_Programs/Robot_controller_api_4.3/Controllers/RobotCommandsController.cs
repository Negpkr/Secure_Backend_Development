using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models; //*updated at 4.3
using robot_controller_api.Persistence; // Add using statement for data access layer

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/robot-commands")]
    public class RobotCommandsController : ControllerBase
    {
        private readonly IRobotCommandDataAccess RobotCommandDataAccess;

        public RobotCommandsController(IRobotCommandDataAccess robotCommandDataAccess)
        {
            RobotCommandDataAccess = robotCommandDataAccess;
        }

        [HttpGet()]
        public IEnumerable<RobotCommand> GetAllRobotCommands() =>
            RobotCommandDataAccess.GetRobotCommands();

        [HttpGet("move")]
        public IEnumerable<RobotCommand> GetMoveCommandsOnly() =>
            RobotCommandDataAccess.GetMoveCommandsOnly();

        [HttpGet("{id}", Name = "GetRobotCommand")]
        public IActionResult GetRobotCommandById(int id)
        {
            var command = RobotCommandDataAccess.GetRobotCommandById(id);
            if (command == null)
                return NotFound("The command does not exist!");
            return Ok(command);
        }

        [HttpPost()]
        public IActionResult AddRobotCommand([FromBody] RobotCommand newCommand)
        {
            if (newCommand == null)
            {
                return BadRequest("New command cannot be null.");
            }

            if (RobotCommandDataAccess.CheckCommandNameExists(newCommand.Name))
            {
                return Conflict("A command with the same name already exists.");
            }

            RobotCommandDataAccess.InsertRobotCommand(newCommand);

            return CreatedAtRoute("GetRobotCommand", new { id = newCommand.Id }, newCommand);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRobotCommand(int id, [FromBody] RobotCommand updatedCommand)
        {
            var command = RobotCommandDataAccess.GetRobotCommandById(id);
            if (command == null)
                return NotFound("The command does not exist!");

            try
            {
                RobotCommandDataAccess.UpdateRobotCommand(id, updatedCommand);
            }
            catch
            {
                return BadRequest("Modification failed!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRobotCommand(int id)
        {
            var command = RobotCommandDataAccess.GetRobotCommandById(id);
            if (command == null)
                return NotFound("The command does not exist!");

            RobotCommandDataAccess.DeleteRobotCommand(id);
            return NoContent();
        }
    }
}
