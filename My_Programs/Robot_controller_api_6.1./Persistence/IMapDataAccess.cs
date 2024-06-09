using robot_controller_api.Models; //*updated at 4.3

namespace robot_controller_api.Persistence
{
    public interface IMapDataAccess
    {
        List<Map> GetMaps();
        Map GetMapById(int id);
        Map UpdateMap(int id, Map map);
        Map InsertMap(Map map);
        void DeleteMap(int id);
        List<Map> GetSquareMaps();
        bool CheckMapNameExists(string mapName);
    }
}
