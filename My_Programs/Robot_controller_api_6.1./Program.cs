
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using robot_controller_api.Authentication;
using robot_controller_api.Models;
using robot_controller_api.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

/*
// 4.1P:
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
builder.Services.AddScoped<IMapDataAccess, MapADO>();
*/

/*
// 4.2C:
builder.Services.AddScoped<IRobotCommandDataAccess,
RobotCommandRepository>();
builder.Services.AddScoped<IMapDataAccess, MapRepository>();
*/


// 4.3D:
builder.Services.AddDbContext<RobotContext>();
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();


//6.1
//*not sure
builder.Services.AddSingleton<IUserDataAccess, UserDataAccess>();
builder.Services.AddSingleton<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
//*

builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions,
BasicAuthenticationHandler>
                ("BasicAuthentication", null); //changed to null

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
});    

//The rest       
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
        
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
