// using System.Collections.Generic;
// using System.Linq;
// using Task.Models;
// ///
using Task.Models;
using Task.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace Task.Services
{
    public class TaskService:ITaskService
    {
        List<task> Taskas { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Taskas = JsonSerializer.Deserialize<List<task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Taskas));
        }
        public  List<task> GetAll() => Taskas;
        public  task Get(int id) => Taskas.FirstOrDefault(p => p.Id == id );
        public  void Add(task mytask)
        {
            mytask.Id = Taskas.Count() + 1;
            Taskas.Add(mytask);
            saveToFile();
        }
        public  void Delete(int id)
        {
            var myTask = Get(id);
            if(myTask is null)
                return;

            Taskas.Remove(myTask);
            saveToFile();
        }
        public  void Update(task myTask)
        {
            var index = Taskas.FindIndex(p => p.Id == myTask.Id);
            if(index == -1)
                return;

            Taskas[index] = myTask;
            
        }
    
     public  int Count => Taskas.Count();

    }
}
