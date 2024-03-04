using Newtonsoft.Json;
using System;

public class Game
{
    private PlayerCharacter player;
    private Map level01;

    public void Run()
    {
        Console.CursorVisible = false;

        // Load the map
        MapLoader loader = new MapLoader();
        level01 = loader.LoadTestMap();

        Console.WindowWidth = 60;
        Console.WindowHeight = 26;

        PlayerCharacter playerCharacter = CreateNewPlayer();
        Console.Clear();

        player = new PlayerCharacter('@', ConsoleColor.Green);

        player.position = new Point2D(level01.mapWidth / 2, level01.mapTiles.Length / level01.mapWidth / 2);

        bool gameRunning = true;
        while (gameRunning)
        {
            level01.Draw();

            Console.SetCursorPosition(player.position.x, player.position.y);
            player.Draw();

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    MovePlayer(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    MovePlayer(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    MovePlayer(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    MovePlayer(1, 0);
                    break;
                case ConsoleKey.Escape:
                    gameRunning = false;
                    break;
                default:
                    break;
            };

            Console.Clear();
        }
    }

    private void MovePlayer(int deltaX, int deltaY)
    {
        int newX = player.position.x + deltaX;
        int newY = player.position.y + deltaY;

        if (newX >= 0 && newX < level01.mapWidth && newY >= 0 && newY < level01.mapTiles.Length / level01.mapWidth)
        {
            int index = newX + newY * level01.mapWidth;
            int tileId = level01.mapTiles[index];

            if (tileId != 2)
            {
                player.Move(deltaX, deltaY);
            }
        }
    }

    private PlayerCharacter CreateNewPlayer()
    {
        string name = AskForPlayerName();
        PlayerCharacter.Race race = AskForPlayerRace();
        PlayerCharacter.Class playerClass = AskForPlayerClass();

        return new PlayerCharacter(name, race, playerClass);
    }

    private string AskForPlayerName()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ---Enter the player character's name--- ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Your name: ");
        Console.ForegroundColor = ConsoleColor.White;

        string input = Console.ReadLine();

        Console.WriteLine();
        Console.ResetColor();

        while (string.IsNullOrWhiteSpace(input) || ContainsNumbers(input))
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("Name cannot be empty. ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Please enter a valid name: ");
            }
            else
            {
                Console.Write("Name cannot contain numbers. ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Please enter a valid name: ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            input = Console.ReadLine();

            Console.ResetColor();
            Console.WriteLine();
        }

        return input;
    }

    private bool ContainsNumbers(string input)
    {
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }
        return false;
    }

    private PlayerCharacter.Race AskForPlayerRace()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ---Choose the player character's race--- ");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("1. Human");
        Console.WriteLine("2. Elf");
        Console.WriteLine("3. Dwarf");
        Console.WriteLine("4. Penguin");

        int choice;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Your Race? ");
        Console.ForegroundColor = ConsoleColor.White;

        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
        {
            Console.Write("Invalid choice. ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Please enter a number between 1 and 4: ");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.ResetColor();

        switch (choice)
        {
            case 1:
                return PlayerCharacter.Race.Human;
            case 2:
                return PlayerCharacter.Race.Elf;
            case 3:
                return PlayerCharacter.Race.Dwarf;
            case 4:
                return PlayerCharacter.Race.Penguin;
            default:
                throw new InvalidOperationException("Invalid race.");
        }
    }

    private PlayerCharacter.Class AskForPlayerClass()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ---Choose the player character's class--- ");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("1. Warrior");
        Console.WriteLine("2. Mage");
        Console.WriteLine("3. Rogue");
        Console.WriteLine("4. Cleric");

        int choice;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Your Class? ");
        Console.ForegroundColor = ConsoleColor.White;

        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
        {
            Console.Write("Invalid choice. ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Please enter a number between 1 and 4: ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.WriteLine();

        switch (choice)
        {
            case 1:
                return PlayerCharacter.Class.Warrior;
            case 2:
                return PlayerCharacter.Class.Mage;
            case 3:
                return PlayerCharacter.Class.Rogue;
            case 4:
                return PlayerCharacter.Class.Cleric;
            default:
                throw new InvalidOperationException("Invalid class.");
        }
    }
}
