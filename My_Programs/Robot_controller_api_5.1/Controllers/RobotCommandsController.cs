using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
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

        /// <summary>
        /// Gets a list of all robot commands.
        /// </summary>
        /// <returns>A list of all robot commands</returns>
        [HttpGet()]
        public IEnumerable<RobotCommand> GetAllRobotCommands() =>
            RobotCommandDataAccess.GetRobotCommands();

        /// <summary>
        /// Gets a list of all move robot commands.
        /// </summary>
        /// <returns>A list of all move robot commands</returns>
        [HttpGet("move")]
        public IEnumerable<RobotCommand> GetMoveCommandsOnly() =>
            RobotCommandDataAccess.GetMoveCommandsOnly();

        /// <summary>
        /// Gets a specific robot command by ID.
        /// </summary>
        /// <param name="id">The ID of the robot command</param>
        /// <returns>The robot command with the specified ID</returns>
        /// <response code="200">Returns the requested robot command</response>
        /// <response code="404">If the robot command is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetRobotCommand")]
        public IActionResult GetRobotCommandById(int id)
        {
            var command = RobotCommandDataAccess.GetRobotCommandById(id);
            if (command == null)
                return NotFound("The command does not exist!");
            return Ok(command);
        }

        /// <summary>
        /// Creates a robot command.
        /// </summary>
        /// <param name="newCommand">A new robot command from the HTTP request.</param>
        /// <returns>A newly created robot command</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/robot-commands
        ///     {
        ///        "name": "DANCE",
        ///        "isMoveCommand": true,
        ///        "description": "Salsa on the Moon"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created robot command</response>
        /// <response code="400">If the robot command is null</response>
        /// <response code="409">If a robot command with the same name already exists.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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

        /// <summary>
        /// Updates an existing robot command.
        /// </summary>
        /// <param name="id">The ID of the robot command to update</param>
        /// <param name="updatedCommand">The updated robot command</param>
        /// <returns>An updated robot command</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/robot-commands
        ///     {
        ///        "Name": "RUN",
        ///        "Description": "Updated command!",
        ///        "IsMoveCommand": true
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the updated robot command</response>
        /// <response code="400">If the ID does not match the command ID or the command is null</response>
        /// <response code="404">If the robot command is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes a robot command.
        /// </summary>
        /// <param name="id">The ID of the robot command to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the robot command was successfully deleted</response>
        /// <response code="404">If the robot command is not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
