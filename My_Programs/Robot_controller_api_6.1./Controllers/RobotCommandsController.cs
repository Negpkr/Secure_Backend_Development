using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using Microsoft.AspNetCore.Identity; // Added in 6.1
using Microsoft.AspNetCore.Authorization; // Added in 6.1

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/robot-commands")]
    public class RobotCommandsController : ControllerBase
    {
        private readonly IRobotCommandDataAccess _robotCommandDataAccess;

        public RobotCommandsController(IRobotCommandDataAccess robotCommandDataAccess)
        {
            _robotCommandDataAccess = robotCommandDataAccess;
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<RobotCommand> GetAllRobotCommands() =>
            _robotCommandDataAccess.GetRobotCommands();

        [HttpGet("move")]
        [AllowAnonymous]
        public IEnumerable<RobotCommand> GetMoveCommandsOnly() =>
            _robotCommandDataAccess.GetMoveCommandsOnly();

        [HttpGet("{id}", Name = "GetRobotCommand")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult GetRobotCommandById(int id)
        {
            var command = _robotCommandDataAccess.GetRobotCommandById(id);
            if (command == null)
                return NotFound("The command does not exist!");
            return Ok(command);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AddRobotCommand([FromBody] RobotCommand newCommand)
        {
            if (newCommand == null)
            {
                return BadRequest("New command cannot be null.");
            }

            if (_robotCommandDataAccess.CheckCommandNameExists(newCommand.Name))
            {
                return Conflict("A command with the same name already exists.");
            }

            _robotCommandDataAccess.InsertRobotCommand(newCommand);

            return CreatedAtRoute("GetRobotCommand", new { id = newCommand.Id }, newCommand);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateRobotCommand(int id, [FromBody] RobotCommand updatedCommand)
        {
            var command = _robotCommandDataAccess.GetRobotCommandById(id);
            if (command == null)
                return NotFound("The command does not exist!");

            try
            {
                _robotCommandDataAccess.UpdateRobotCommand(id, updatedCommand);
            }
            catch
            {
                return BadRequest("Modification failed!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteRobotCommand(int id)
        {
            var command = _robotCommandDataAccess.GetRobotCommandById(id);
            if (command == null)
                return NotFound("The command does not exist!");

            _robotCommandDataAccess.DeleteRobotCommand(id);
            return NoContent();
        }
    }
}
