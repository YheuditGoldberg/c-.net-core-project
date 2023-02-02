using Task.Models;
using System.Collections.Generic;

namespace Task.Interfaces
{
    public interface ITaskService
    {
        List<task> GetAll(int id);
        task Get(int id);
        void Add(task myTask);
        void Delete(int id);
        void Update(task myTask);
        int Count {get;}
        
    }
}