
using System;

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public class Robot
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction Facing { get; private set; }
    private bool IsPlaced { get; set; }

    public Robot()
    {
        IsPlaced = false;
    }

    public void Place(int x, int y, Direction facing, int mapSize)
    {
        if (x < 0 || y < 0 || x >= mapSize || y >= mapSize)
        {
            Console.WriteLine("Robot must be placed within the map boundaries.");
            return;
        }

        X = x;
        Y = y;
        Facing = facing;
        IsPlaced = true;
    }

    public void Move(int mapSize)
    {
        if (!IsPlaced)
        {
            Console.WriteLine("Robot must be placed on the map FIRST.");
            return;
        }

        int newX = X;
        int newY = Y;

        switch (Facing)
        {
            case Direction.NORTH:
                newY++;
                break;
            case Direction.EAST:
                newX++;
                break;
            case Direction.SOUTH:
                newY--;
                break;
            case Direction.WEST:
                newX--;
                break;
        }

        if (newX < 0 || newY < 0 || newX >= mapSize || newY >= mapSize)
        {
            Console.WriteLine("Robot cannot move off the map.");
            return;
        }

        X = newX;
        Y = newY;
    }

    public void TurnLeft()
    {
        if (!IsPlaced)
        {
            Console.WriteLine("Robot must be placed on the map FIRST!");
            return;
        }

        Facing = (Direction)(((int)Facing + 3) % 4);
    }

    public void TurnRight()
    {
        if (!IsPlaced)
        {
            Console.WriteLine("Robot must be placed on the map FIRST!");
            return;
        }

        Facing = (Direction)(((int)Facing + 1) % 4);
    }

    public void Report()
    {
        if (!IsPlaced)
        {
            Console.WriteLine("Robot must be placed on the map FIRST!");
            return;
        }

        Console.WriteLine($"Robot is at ({X},{Y}), facing {Facing}");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Enter map size(x):");
        string mapInput = Console.ReadLine();

        if (!int.TryParse(mapInput, out _))
        {
            Console.WriteLine("Invalid map size. Size must be an integer.");
            return;
        }

        int mapSize = int.Parse(mapInput);

        if (mapSize < 2 || mapSize > 100)
        {
            Console.WriteLine("Invalid map size. Size must be between 2 and 100.");
            return;
        }
        Console.WriteLine($"Map Size is {mapSize}x{mapSize}.");

        Robot robot = new Robot();

        while (true)
        {
            Console.WriteLine("Enter command (PLACE X Y D | LEFT | RIGHT | MOVE | REPORT | EXIT):");
            string input = Console.ReadLine().Trim().ToUpper();

            if (input.StartsWith("PLACE"))
            {
                //* CHECKING
                string[] parts = input.Split(' ');
                if (
                    parts.Length != 4
                    || int.TryParse(parts[1], out int x) == false
                    || int.TryParse(parts[2], out int y) == false
                    || Enum.TryParse(parts[3], out Direction facing) == false
                )
                {
                    Console.WriteLine(
                        "Invalid PLACE command. Use format PLACE X Y D which X and Y must be integer and D must be a direction (NORTH, SOUTH, EAST, WEST)!"
                    );
                    continue;
                }

                robot.Place(x, y, facing, mapSize);
            }
            else if (input == "MOVE")
            {
                robot.Move(mapSize);
            }
            else if (input == "LEFT")
            {
                robot.TurnLeft();
            }
            else if (input == "RIGHT")
            {
                robot.TurnRight();
            }
            else if (input == "REPORT")
            {
                robot.Report();
            }
            else if (input == "EXIT")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid command.");
            }
        }
    }
}
