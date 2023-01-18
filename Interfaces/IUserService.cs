using User.Models;
using System.Collections.Generic;

namespace User.Interfaces
{
    public interface IUserService
    {
        List<user> GetAll();
        user Get(int id);
        void Add(user myUser);
        void Delete(int id);
        void Update(user myUser);
        int Count {get;}
    }
}