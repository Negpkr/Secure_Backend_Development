using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using System;
using System.Collections.Generic;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/maps")]
    public class MapsController : ControllerBase
    {
        private static readonly List<Map> _maps = new List<Map>
        {
            new Map(1, 10, 5, "Map 1", "First Map", DateTime.Now, DateTime.Now),
            new Map(2, 8, 8, "Map 2", "Second Map", DateTime.Now, DateTime.Now),
            new Map(3, 10, 10, "Map 3", "Third Map", DateTime.Now, DateTime.Now)
        };

        [HttpGet()]
        public IEnumerable<Map> GetAllMaps() => _maps;

        [HttpGet("square")]
        public IEnumerable<Map> GetSquareMapOnly() => _maps.Where(c => c.Rows == c.Columns);

        [HttpGet("{id}", Name = "GetMapCommand")]
        public IActionResult GetMapById(int id)
        {
            var map = _maps.FirstOrDefault(m => m.Id == id);
            if (map == null)
                return NotFound("The map does not exist!");
            return Ok(map);
        }

        [HttpPost()]
        public IActionResult AddMapCommand([FromBody] Map newMap)
        {
            if (newMap == null)
            {
                return BadRequest("New map cannot be null.");
            }

            if (_maps.Any(c => c.Name == newMap.Name))
            {
                return Conflict("A map with the same name already exists.");
            }
            // Generate a new unique Id
            int newId = _maps.Count > 0 ? _maps.Max(c => c.Id) + 1 : 1;

            newMap.CreatedDate = DateTime.Now;
            newMap.ModifiedDate = DateTime.Now;
            newMap.Id = newId;
            _maps.Add(newMap);

            return CreatedAtRoute("GetMapCommand", new { id = newMap.Id }, newMap);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMap(int id, [FromBody] Map updatedMap)
        {
            var map = _maps.FirstOrDefault(m => m.Id == id);
            if (map == null)
                return NotFound("The map does not exist!");

            try
            {
                map.Name = updatedMap.Name;
                map.Description = updatedMap.Description;
                map.Columns = updatedMap.Columns;
                map.Rows = updatedMap.Rows;
            }
            catch
            {
                return BadRequest("Modification failed!");
            }

            map.ModifiedDate = DateTime.Now;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMap(int id)
        {
            var map = _maps.FirstOrDefault(m => m.Id == id);
            if (map == null)
                return NotFound("The map does not exist!");

            _maps.Remove(map);
            return NoContent();
        }

        [HttpGet("{id}/{x}-{y}")]
        public IActionResult CheckCoordinate(int id, int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return BadRequest("Coordinates cannot be negative.");
            }

            var map = _maps.FirstOrDefault(m => m.Id == id);
            if (map == null)
            {
                return NotFound("Map not found.");
            }

            bool isOnMap = x < map.Columns && y < map.Rows;
            return Ok(isOnMap);
        }
    }
}
