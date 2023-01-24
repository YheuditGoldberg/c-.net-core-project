// using System.Collections.Generic;
// using System.Linq;
// using Task.Models;
// ///
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
            myUser.Id = Users.Count() + 1;
            Users.Add(myUser);
            saveToFile();
        }
        public void Delete(int id)
        {
            var myUser = Get(id);
            if (myUser is null)
                return;

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
    }
   
}
