using System.Collections.Generic;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IUserDataAccess
    {
        UserModel GetUserByEmail(string email);
        void AddUser(UserModel user);
        IEnumerable<UserModel> GetAllUsers();
        IEnumerable<UserModel> GetUsersByRole(string role);
        UserModel GetUserById(int id);
        void UpdateUser(UserModel user);
        void DeleteUser(int id);
        void UpdateUserPassword(int id, string newPasswordHash);
    }
}
