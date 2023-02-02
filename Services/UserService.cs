using User.Models;
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
    public class UserService : IUserService
    {
        
        List<user> Users { get; }
        static int count=0;
        private  void getmaxvalue(){
        int max=0;
        foreach(var i in Users){
        if(i.Id>max)
        max=i.Id;
        }
          count=max+1;
        }
        private IWebHostEnvironment webHost;
        private string filePath;
        public UserService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "User.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Users = JsonSerializer.Deserialize<List<user>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Users));
        }
        public List<user> GetAll() => Users;
        public user Get(int id) => Users.FirstOrDefault(p => p.Id == id);
        public void Add(user myUser)
        {
            getmaxvalue();
            myUser.Id = count+1;
            Users.Add(myUser);
            saveToFile();
        }
        public void Delete(int id)
        {
            var myUser = Get(id);
            if (myUser is null)
                return;
            int idusertask=myUser.Id;
            TaskService toDoService = new TaskService(this.webHost);
            toDoService.DeleteAll(id);
            Users.Remove(myUser);
            saveToFile();
        }
        public void Update(user myUser)
        {
            var index = Users.FindIndex(p => p.Id == myUser.Id);
            if (index == -1)
                return;

            Users[index] = myUser;

        }
        public int Count => Users.Count();
        public user IsExist(user user) 
        {
            return(Users.FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password));
        }
     public int findId(string name, string password)
        {
            var idUser = Users.FirstOrDefault(u => u.Name.Equals(name) && u.Password.Equals(password));
            return idUser.Id;
        }
    }
   
}
