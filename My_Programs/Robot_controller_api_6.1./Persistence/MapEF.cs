
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapEF : IMapDataAccess
    {
        private readonly RobotContext _context;

        public MapEF(RobotContext context)
        {
            _context = context;
        }

        public List<Map> GetMaps()
        {
            return _context.Maps.ToList();
        }

        public Map GetMapById(int id)
        {
            return _context.Maps.FirstOrDefault(m => m.Id == id);
        }

        public Map InsertMap(Map newMap)
        {
            //*added
            newMap.CreatedDate = newMap.ModifiedDate = DateTime.Now;

            _context.Maps.Add(newMap);
            _context.SaveChanges();
            return newMap;
        }

        public Map UpdateMap(int id, Map updatedMap)
        {
            var map = _context.Maps.FirstOrDefault(m => m.Id == id);
            if (map != null)
            {
                map.Name = updatedMap.Name;
                map.Rows = updatedMap.Rows;
                map.Columns = updatedMap.Columns;
                map.Description = updatedMap.Description;
                //*added
                map.ModifiedDate = DateTime.Now;

                _context.SaveChanges();
            }

            return map;
        }

        public void DeleteMap(int id)
        {
            var map = _context.Maps.FirstOrDefault(m => m.Id == id);
            if (map != null)
            {
                _context.Maps.Remove(map);
                _context.SaveChanges();
            }
        }

        public List<Map> GetSquareMaps()
        {
            return _context.Maps.Where(m => m.Rows > 0 && m.Rows == m.Columns).ToList();
            //or use Issquare column!
        }

        public bool CheckMapNameExists(string mapName)
        {
            return _context.Maps.Any(m => m.Name == mapName);
        }
    }
}
