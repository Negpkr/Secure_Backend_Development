
using robot_controller_api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IRobotCommandDataAccess,
RobotCommandRepository>();
builder.Services.AddScoped<IMapDataAccess, MapRepository>();

/*
builder.Services.AddControllers();
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
builder.Services.AddScoped<IMapDataAccess, MapADO>();
*/

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
