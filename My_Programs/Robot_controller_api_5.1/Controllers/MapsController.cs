using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
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

        /// <summary>
        /// Gets all maps.
        /// </summary>
        /// <returns>An enumerable of all maps</returns>
        [HttpGet()]
        public IEnumerable<Map> GetAllMaps() => MapDataAccess.GetMaps();

        /// <summary>
        /// Gets only square maps.
        /// </summary>
        /// <returns>An enumerable of square maps</returns>
        [HttpGet("square")]
        public IEnumerable<Map> GetSquareMapOnly() => MapDataAccess.GetSquareMaps();

        /// <summary>
        /// Gets a map by ID.
        /// </summary>
        /// <param name="id">The ID of the map</param>
        /// <returns>The map with the specified ID</returns>
        /// <response code="200">Returns the requested map</response>
        /// <response code="404">If the map is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetMapCommand")]
        public IActionResult GetMapById(int id)
        {
            var map = MapDataAccess.GetMapById(id);
            if (map == null)
                return NotFound("The map does not exist!");
            return Ok(map);
        }

        /// <summary>
        /// Adds a new map.
        /// </summary>
        /// <param name="newMap">The new map to add</param>
        /// <returns>The newly created map</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///   POST /api/maps
        ///   {
        ///       "Name": "Map 3",
        ///       "Description": "Added Map!",
        ///       "Columns": 10,
        ///       "Rows": 10
        ///    }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created map</response>
        /// <response code="400">If the map is null</response>
        /// <response code="409">If a map with the same name already exists.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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

        /// <summary>
        /// Updates a map.
        /// </summary>
        /// <param name="id">The ID of the map to update</param>
        /// <param name="updatedMap">The updated map</param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///   PUT /api/maps
        ///   {
        ///       "Name": "Updated Map 3",
        ///       "Description": "Updated Map!",
        ///       "Columns": 5,
        ///       "Rows": 4
        ///    }
        ///
        /// </remarks>
        /// <response code="200">Returns the updated map</response>
        /// <response code="400">If the ID does not match the map ID or the map is null</response>
        /// <response code="404">If the map is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes a map.
        /// </summary>
        /// <param name="id">The ID of the map to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the map was successfully deleted</response>
        /// <response code="404">If the map is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMap(int id)
        {
            var map = MapDataAccess.GetMapById(id);
            if (map == null)
                return NotFound("The map does not exist!");

            MapDataAccess.DeleteMap(id);
            return NoContent();
        }

        /// <summary>
        /// Checks if a coordinate is on a map.
        /// </summary>
        /// <param name="id">The ID of the map</param>
        /// <param name="x">The x-coordinate</param>
        /// <param name="y">The y-coordinate</param>
        /// <returns>True if the coordinate is on the map, otherwise false</returns>
        /// <response code="200">Returns true if the coordinate is on the map, otherwise false</response>
        /// <response code="400">If the coordinates are negative or out of range</response>
        /// <response code="404">If the map is not found</response>
        [HttpGet("{id}/{x}-{y}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
