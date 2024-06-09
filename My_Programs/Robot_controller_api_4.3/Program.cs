using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
