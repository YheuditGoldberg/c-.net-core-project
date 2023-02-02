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
         List<task>  Taskas { get; }
        static int fromdelete;
        static int count=0;
        private  void getmaxvalue(){
        int max=0;
        foreach(var i in Taskas){
        if(i.IdTask>max)
        max=i.IdTask;
        }
          count=max+1;
        }
        private IWebHostEnvironment  webHost;
        private string filePath;
        private int userID;
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
        public  List<task> GetAll(int userId ) {
            if(userId ==0)
            return Taskas.ToList();
            else{
           this.userID=userId;
           return Taskas.Where(i=>i.Id==userId).ToList();
            }
        
        } 
        public  task Get(int id) => Taskas.FirstOrDefault(p => p.IdTask == id );
        public  void Add(task mytask)
        {
            mytask.Id = this.userID;
            getmaxvalue();
            mytask.IdTask=count+1;
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
    
         public void DeleteAll(int userId)
        {
            foreach (var item in Taskas.ToList()) {
                if (item.Id == userId)
                    Taskas.Remove(item);
            }
            saveToFile();
        }
         public void Update(task item)
        {
            var index = Taskas.FindIndex(i => i.IdTask == item.IdTask);
            if(index == -1)
                return;

            Taskas[index] = item;
            saveToFile();
        }
      
     public  int Count => Taskas.Count();

    }
}
