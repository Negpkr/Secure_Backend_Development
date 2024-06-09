using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/robot-commands")]
    public class RobotCommandsController : ControllerBase
    {
        private static readonly List<RobotCommand> _commands = new List<RobotCommand>
        {
            new RobotCommand(1, "LEFT", true, DateTime.Now, DateTime.Now, "Turns the robot left"),
            new RobotCommand(2, "RIGHT", true, DateTime.Now, DateTime.Now, "Turns the robot right"),
            new RobotCommand(
                3,
                "MOVE",
                true,
                DateTime.Now,
                DateTime.Now,
                "Moves the robot forward"
            ),
            new RobotCommand(
                4,
                "PLACE",
                false,
                DateTime.Now,
                DateTime.Now,
                "Places the robot on the grid"
            ),
            new RobotCommand(
                5,
                "REPORT",
                false,
                DateTime.Now,
                DateTime.Now,
                "Reports the current position of the robot"
            )
        };

        [HttpGet()]
        public IEnumerable<RobotCommand> GetAllRobotCommands() => _commands;

        [HttpGet("move")]
        public IEnumerable<RobotCommand> GetMoveCommandsOnly() =>
            _commands.Where(c => c.IsMoveCommand);

        [HttpGet("{id}", Name = "GetRobotCommand")]
        public IActionResult GetRobotCommandById(int id)
        {
            var command = _commands.FirstOrDefault(c => c.Id == id);
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

            if (_commands.Any(c => c.Name == newCommand.Name))
            {
                return Conflict("A command with the same name already exists.");
            }
            // Generate a new unique Id
            int newId = _commands.Count > 0 ? _commands.Max(c => c.Id) + 1 : 1;

            newCommand.CreatedDate = DateTime.Now;
            newCommand.ModifiedDate = DateTime.Now;
            newCommand.Id = newId;
            _commands.Add(newCommand);

            return CreatedAtRoute("GetRobotCommand", new { id = newCommand.Id }, newCommand);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRobotCommand(int id, [FromBody] RobotCommand updatedCommand)
        {
            var command = _commands.FirstOrDefault(c => c.Id == id);
            if (command == null)
                return NotFound("The command does not exist!");

            try
            {
                command.Name = updatedCommand.Name;
                command.Description = updatedCommand.Description;
                command.IsMoveCommand = updatedCommand.IsMoveCommand;
            }
            catch
            {
                return BadRequest("Modification failed!");
            }

            command.ModifiedDate = DateTime.Now;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRobotCommand(int id)
        {
            var command = _commands.FirstOrDefault(c => c.Id == id);
            if (command == null)
                return NotFound("The command does not exist!");

            _commands.Remove(command);
            return NoContent();
        }
    }
}
