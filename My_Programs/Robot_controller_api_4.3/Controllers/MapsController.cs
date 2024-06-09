using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models; //*Updated at 4.3
using robot_controller_api.Persistence; // Add using statement for data access layer

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("api/maps")]
    public class MapsController : ControllerBase
    {
        private readonly IMapDataAccess MapDataAccess;

        public MapsController(IMapDataAccess mapDataAccess)
        {
            MapDataAccess = mapDataAccess;
        }

        [HttpGet()]
        public IEnumerable<Map> GetAllMaps() => MapDataAccess.GetMaps();

        [HttpGet("square")]
        public IEnumerable<Map> GetSquareMapOnly() => MapDataAccess.GetSquareMaps();

        [HttpGet("{id}", Name = "GetMapCommand")]
        public IActionResult GetMapById(int id)
        {
            var map = MapDataAccess.GetMapById(id);
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

            if (MapDataAccess.CheckMapNameExists(newMap.Name))
            {
                return Conflict("A map with the same name already exists.");
            }

            MapDataAccess.InsertMap(newMap);

            return CreatedAtRoute("GetMapCommand", new { id = newMap.Id }, newMap);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMap(int id, [FromBody] Map updatedMap)
        {
            var map = MapDataAccess.GetMapById(id);
            if (map == null)
                return NotFound("The map does not exist!");

            try
            {
                MapDataAccess.UpdateMap(id, updatedMap);
            }
            catch
            {
                return BadRequest("Modification failed!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMap(int id)
        {
            var map = MapDataAccess.GetMapById(id);
            if (map == null)
                return NotFound("The map does not exist!");

            MapDataAccess.DeleteMap(id);
            return NoContent();
        }

        [HttpGet("{id}/{x}-{y}")]
        public IActionResult CheckCoordinate(int id, int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return BadRequest("Coordinates cannot be negative.");
            }

            var map = MapDataAccess.GetMapById(id);
            if (map == null)
            {
                return NotFound("Map not found.");
            }

            bool isOnMap = x < map.Columns && y < map.Rows;
            return Ok(isOnMap);
        }
    }
}
