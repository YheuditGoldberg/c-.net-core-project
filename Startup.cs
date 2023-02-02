// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.HttpsPolicy;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using Microsoft.OpenApi.Models;
// using User.Interfaces;
// using User.Services;

// namespace Task
// {
//     public class Startup
//     {
//         public Startup(IConfiguration configuration)
//         {
//             Configuration = configuration;
//         }

//         public IConfiguration Configuration { get; }

//         // This method gets called by the runtime. Use this method to add services to the container.
//         public void ConfigureServices(IServiceCollection services)
//         {

//             services.AddControllers();
//             services.AddSwaggerGen(c =>
//             {
//                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task", Version = "v1" });
//             });
//             services.AddSingleton<Interfaces.ITaskService, Services.TaskService>();
//             services.AddSingleton<User.Interfaces.IUserService, User.Services.UserService>();
            
//         }

//         // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//         public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//         {
//             if (env.IsDevelopment())
//             {
//                 app.UseDeveloperExceptionPage();
//                 app.UseSwagger();
//                 app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task v1"));
//             }

//             app.UseHttpsRedirection();
//             /*js*/
//             app.UseDefaultFiles();
//             app.UseStaticFiles();
//             /*js (remove "launchUrl" from Properties\launchSettings.json*/

//             app.UseRouting();

//             app.UseAuthorization();

//             app.UseEndpoints(endpoints =>
//             {
//                 endpoints.MapControllers();
//             });
//         }
//     }
// }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Task.Interfaces;
using Task.Services;


namespace Task
{
    public class Startup
    {
   // TokenService ToDoTokenService = new TokenService();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             services
                .AddCors()
                .AddAuthentication(option => 
                {
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.TokenValidationParameters = TokenService.GetTokenValidationParameters();
                });
            services.AddHttpContextAccessor();
            //  services.AddAuthorization(cfg =>
            //     {
            //         cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
            //       //  cfg.AddPolicy("Agent", policy => policy.RequireClaim("type", "User"));
            //        cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User"));
            //         cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1", "2"));
            //         cfg.AddPolicy("ClearanceLevel2", policy => policy.RequireClaim("ClearanceLevel", "2"));
            //     });
              services.AddAuthorization(cfg =>
                {
                    cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
                    cfg.AddPolicy("User", policy => policy.RequireClaim("type", "Admin", "User"));
                });
                

            services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task", Version = "v1" });
            //       c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //     {
            //         In = ParameterLocation.Header,
            //         Description = "Please enter JWT with Bearer into field",
            //         Name = "Authorization",
            //         Type = SecuritySchemeType.ApiKey
            //     });
             services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManager", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }
                });
            });
            services.AddSingleton<Interfaces.ITaskService, Services.TaskService>();
            services.AddSingleton<Interfaces.IUserService, Services.UserService>();
            // services.AddSingleton<Interfaces., Services.TaskService>();

            // services.AddSingleton<Interfaces.IUserService, Services.UserService>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task v1"));
            }
             app.UseHttpsRedirection();
               app.UseDefaultFiles();
               app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // app.UseHttpsRedirection();
            // /*js*/
            // app.UseDefaultFiles();
            // app.UseStaticFiles();
            // /*js (remove "launchUrl" from Properties\launchSettings.json*/

            // app.UseRouting();

            // app.UseAuthorization();

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });
        }
    }
}
