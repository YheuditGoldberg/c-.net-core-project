using User.Models;
using System.Collections.Generic;

namespace Task.Interfaces
{
    public interface IUserService
    {
        List<user> GetAll();
        user Get(int id);
        void Add(user myUser);
        void Delete(int id);
        void Update(user myUser);
        user IsExist(user user);

        int Count {get;}
        int findId(string name, string password);
    }
}