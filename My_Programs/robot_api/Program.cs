using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

var robotCommands = new[]
{
    "LEFT", "RIGHT", "PLACE", "MOVE"
};

var robotMap = new[]
{
    "X", "Y"
};

app.MapGet("/", () => "Hello, Robot!");

app.MapGet("/robot-commands", () => robotCommands);

app.MapGet("/robot-commands/move", () => robotCommands.Where(c => c == "MOVE").ToArray());

app.MapGet("/robot-commands/{id}", (int id) =>
{
    if (id < 0 || id >= robotCommands.Length)
    {
        return Results.NotFound();
    }

    return Results.Ok(robotCommands[id]);
});

app.MapPost("/robot-commands", ([FromBody] string command) =>
{
    robotCommands = robotCommands.Append(command).ToArray();
    return Results.Created($"/robot-commands/{robotCommands.Length - 1}", command);
});

app.MapPut("/robot-commands/{id}", (int id, [FromBody] string command) =>
{
    if (id < 0 || id >= robotCommands.Length)
    {
        return Results.NotFound();
    }

    robotCommands[id] = command;
    return Results.NoContent();
});

app.MapGet("/robot-map", () => robotMap);

app.MapGet("/robot-map/{coordinate}", (string coordinate) =>
{
    var coordinates = coordinate.Split('-');
    if (coordinates.Length != 2 || !int.TryParse(coordinates[0], out int x) || !int.TryParse(coordinates[1], out int y))
    {
        return Results.BadRequest();
    }

    return Results.Ok(int.Parse(robotMap[0]) > x && int.Parse(robotMap[0]) > y && x >= 0 && y >= 0);
});

app.MapPut("/robot-map", ([FromBody] List<string> newMap) =>
{
    if (newMap.Count != 2 || newMap[0]!=newMap[1] || int.Parse(newMap[0])< 2 || int.Parse(newMap[0]) > 100)
    {
        return Results.BadRequest();
    }

    robotMap = newMap.ToArray();
    return Results.NoContent();
});

app.Run();
