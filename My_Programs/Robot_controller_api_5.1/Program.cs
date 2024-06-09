
using robot_controller_api.Persistence;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//* added at 5.1
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Robot Controller API",
        Description = "New backend service that provides resources for the Moon robot simulator.",
        Contact = new OpenApiContact
        {
            Name = "Negin Pakrooh",
            Email = "s222393187@deakin.edu.au"
        },
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,xmlFilename));
});
//*

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

//*
app.UseStaticFiles();
app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-flattop.css"));
//*

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
