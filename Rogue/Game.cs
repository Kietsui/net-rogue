using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Numerics;
using ZeroElectric.Vinculum;
using static MapLoader;

public class Game
{
    private PlayerCharacter player;
    private Map level01;

    private bool gameRunning = false;
    private Texture myImage;

    public static readonly int imagesPerRow = 12;
    public static readonly int tileSize = 16;

    private int screen_width = 1280;
    private int screen_height = 720;
    private int game_width = 480;
    private int game_height = 270;
    private RenderTexture game_screen;

    public void Run()
    {
        Console.CursorVisible = false;
        Console.Clear();

        Init();
        GameLoop();
        Raylib.CloseWindow();
        Raylib.UnloadRenderTexture(game_screen);
    }

    private void Init()
    {
        Console.WindowWidth = 55;

        PlayerCharacter playerCharacter = CreateNewPlayer();
        player = new PlayerCharacter('@', Raylib.GREEN);

        Raylib.InitWindow(screen_width, screen_height, "Rogue");
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.SetTargetFPS(30);

        Raylib.SetWindowMinSize(game_width, game_height);

        game_screen = Raylib.LoadRenderTexture(game_width, game_height);
        Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);

        myImage = Raylib.LoadTexture("Images/tilemap_packed.png");
        if (myImage.id == 0)
        {
            Console.WriteLine("Failed to load texture atlas");
            return;
        }
        else
        {
            Console.WriteLine("Loaded texture atlas");
        }

        MapLoader loader = new MapLoader();
        loader.SetTexture(myImage);
        level01 = loader.LoadTestMap();

        if (level01 != null && level01.layers != null)
        {
            Console.WriteLine($"Layers count: {level01.layers.Length}");

            if (level01.layers.Length > 0 && level01.layers[0] != null)
            {
                Console.WriteLine($"Ground layer tile count: {level01.layers[0].mapTiles.Length}");
            }
            if (level01.layers.Length > 1 && level01.layers[1] != null)
            {
                Console.WriteLine($"Items layer tile count: {level01.layers[1].mapTiles.Length}");
            }
            if (level01.layers.Length > 2 && level01.layers[2] != null)
            {
                Console.WriteLine($"Enemies layer tile count: {level01.layers[2].mapTiles.Length}");
            }
        }
        else
        {
            Console.WriteLine("level01 or level01.layers is null.");
        }

        level01.LoadEnemiesAndItems(myImage, level01.layers[0].mapTiles, level01.layers[1].mapTiles);
        player.position = new Point2D(level01.mapWidth / 2, level01.mapTiles.Length / level01.mapWidth / 2);

        player.SetImageAndIndex(myImage, imagesPerRow, 97);

        int wallX = 4, wallY = 3;
        int floorX = 0, floorY = 4;
        int itemX = 7, itemY = 9;
        int enemyX = 1, enemyY = 9;

        int wallIndex = wallY * imagesPerRow + wallX;
        int floorIndex = floorY * imagesPerRow + floorX;
        int itemIndex = itemY * imagesPerRow + itemX;
        int enemyIndex = enemyY * imagesPerRow + enemyX;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Wall Texture Index: {wallIndex}");
        Console.WriteLine($"Floor Texture Index: {floorIndex}");
        Console.WriteLine($"Item Textuer Index: {itemIndex}");
        Console.WriteLine($"Enemy Texture Index: {enemyIndex}");
        Console.ResetColor();
    }




    private void DrawGameToTexture()
    {
        Raylib.BeginTextureMode(game_screen);
        Raylib.ClearBackground(Raylib.WHITE);

        level01.DrawMapTiles(myImage, level01.layers[0]);
        level01.DrawItems(myImage, level01.layers[1]);
        //level01.DrawEnemies(myImage, level01.layers[2]);
        player.Draw();

        Raylib.EndTextureMode();
    }



    private void DrawGameScaled()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.DARKGRAY);

        int draw_width = Raylib.GetScreenWidth();
        int draw_height = Raylib.GetScreenHeight();
        float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);

        Rectangle source = new Rectangle(0.0f, 0.0f, game_screen.texture.width, game_screen.texture.height * -1.0f);
        Rectangle destination = new Rectangle(
            (draw_width - (float)game_width * scale) * 0.5f,
            (draw_height - (float)game_height * scale) * 0.5f,
            game_width * scale,
            game_height * scale
        );

        Raylib.DrawTexturePro(game_screen.texture, source, destination, new Vector2(0, 0), 0.0f, Raylib.WHITE);

        Raylib.EndDrawing();
    }

    private void UpdateGame()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) { MovePlayer(0, -1); }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)) { MovePlayer(0, 1); }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT)) { MovePlayer(-1, 0); }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT)) { MovePlayer(1, 0); }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE)) { gameRunning = false; }
    }

    private void GameLoop()
    {
        while (!Raylib.WindowShouldClose())
        {
            UpdateGame();
            DrawGameToTexture();
            DrawGameScaled();
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
