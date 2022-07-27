using Business;
using Business.Interfaces;
using Business.Services;
using Data.Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Data.Entities;
using Data.Repositories;
using Business.Models;
using AuthCommon;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Forum");
            services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString));

            //services.AddCors((setup) =>
            //{
            //    setup.AddPolicy("default", (options) =>
            //    {
            //        options.AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowAnyOrigin();
            //    });
            //});

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            }) ;
         
            services.AddSwaggerGen();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(AutomapperProfile));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();

            //services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<ForumDbContext>();

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ForumDbContext>()
                .AddDefaultTokenProviders();

            var authOptionsConfiguration = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptionsConfiguration);

            var authOptions = authOptionsConfiguration.Get<AuthOptions>();
            //services.AddAuthentication();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {                                                               
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum Api ver.1");
                    }
                );
            }

            //app.UseCors("default");
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();            

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var connectionString = Configuration.GetConnectionString("Forum");
            InitRoles(serviceProvider).Wait();
            CreateAdmin(serviceProvider).Wait();
            CreateTestUsers(serviceProvider).Wait();
            FillDbWithTestData(serviceProvider, connectionString).Wait();
        }       

        private async static Task InitRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User", "AdvancedUser", "PowerUser", "Client", "SilverClient", "PlatinumClient"};
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        private static async Task CreateAdmin(IServiceProvider serviceProvider)
        {            
            //var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var admin = new User()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com"
            };
            string userPassw = "Qwerty@123";
            var checkUser = await UserManager.FindByEmailAsync("admin@gmail.com");

            if (checkUser == null)
            {
                var createdUser = await UserManager.CreateAsync(admin, userPassw);
                if (createdUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, "Admin");
                    admin.UserRoles = "Admin";
                }
                    
            }
        }

        private static async Task CreateTestUsers(IServiceProvider serviceProvider)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] userNames = { "serg@gmail.com", "igor@gmail.com", "anna@gmail.com", "alex@gmail.com", "max@gmail.com" };
            string userPassw = "Qwerty@123";
            foreach (var item in userNames)
            {
                var checkUser = await UserManager.FindByNameAsync(item);
                if (checkUser == null)
                {
                    checkUser = new User()
                    {
                        UserName = item,
                        Email = item
                    };
                    var createdUser = await UserManager.CreateAsync(checkUser, userPassw);
                    if (createdUser.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(checkUser, "User");
                        await UserManager.AddToRoleAsync(checkUser, "Client");
                        checkUser.UserRoles = "User, Client";
                    }                        
                }
            }
        }
        static async Task FillDbWithTestData(IServiceProvider serviceProvider, string connectionString)
        {
            ForumDbContext context = new ForumDbContext();

            if (context.Posts.Count() == 0)
            {
                var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
                string[] userNames = { "serg@gmail.com", "igor@gmail.com", "anna@gmail.com", "alex@gmail.com", "max@gmail.com" };
                var serg = await UserManager.FindByNameAsync(userNames[0]);
                var sergId = serg.Id;
                var igor = await UserManager.FindByNameAsync(userNames[1]);
                var igorId = igor.Id;
                var anna = await UserManager.FindByNameAsync(userNames[2]);
                var annaId = anna.Id;
                var alex = await UserManager.FindByNameAsync(userNames[3]);
                var alexId = alex.Id;
                var max = await UserManager.FindByNameAsync(userNames[4]);
                var maxId = max.Id;

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string lorum = "Lorem ipsum dolor sit, amet consectetur adipisicing elit." +
                    "Sapiente numquam eveniet enim exercitationem saepe pariatur cupiditate" +
                    "magni quibusdam repellat at rem, minima rerum omnis dolor veritatis architecto ullam? Accusantium, eaque." +
                    "Lorem ipsum dolor sit, amet consectetur adipisicing elit" +
                    "Sapiente numquam eveniet enim exercitationem saepe pariatur cupiditate " +
                    "magni quibusdam repellat at rem, minima rerum omnis dolor veritatis architecto ullam? Accusantium, eaque.";

                string strCommand = "insert into Posts(Title, Summary, Content, PublishDate, UserId) values " +
                    $"(N'CRV', N'Incredible car', '{lorum}', N'2020-09-7 00:12:00', '{sergId}')," +
                    $"(N'Tucson', N'Corea rules', '{lorum}', N'2022-05-7 00:12:00', '{sergId}')," +
                    $"(N'RAV4', N'What a car!', '{lorum}', N'2022-06-7 00:12:00', '{igorId}')," +
                    $"(N'X5', N'Epxensive one', '{lorum}', N'2022-06-3 00:12:00', '{igorId}')," +
                    $"(N'Forrester', N'Impressive', '{lorum}', N'2022-06-2 00:12:00', '{annaId}')";
                SqlCommand command = new SqlCommand(strCommand, connection);
                command.ExecuteNonQuery();

                strCommand = "insert into Comments(Content, PublishDate, UserId, PostId) values " +
                    $"(N'Agree', N'2021-09-7 00:12:00', '{maxId}', 1)," +
                    $"(N'Disagree', N'2021-09-7 00:12:00', '{alexId}', 1)," +
                    $"(N'Could be', N'2021-09-7 00:12:00', '{annaId}', 1)," +
                    $"(N'Agree2', N'2021-09-7 00:12:00', '{maxId}', 2)," +
                    $"(N'Disagree2', N'2021-09-7 00:12:00', '{alexId}', 2)," +
                    $"(N'Could be2', N'2021-09-7 00:12:00', '{annaId}', 2)," +
                    $"(N'Agree3', N'2021-09-7 00:12:00', '{maxId}', 3)," +
                    $"(N'Disagree3', N'2021-09-7 00:12:00', '{alexId}', 3)," +
                    $"(N'Could be3', N'2021-09-7 00:12:00', '{annaId}', 3)";
                command = new SqlCommand(strCommand, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

    }
}
