using System;
using System.Numerics;
using ZeroElectric.Vinculum;

public class PlayerCharacter
{
    public Point2D position;
    private char characterSymbol;
    private Color color;

    private Texture image;

    private int imagePixelX;
    private int imagePixelY;

    public PlayerCharacter(char characterSymbol, Color color)
    {
        this.characterSymbol = characterSymbol;
        this.color = color;
    }

    public void Move(int x_move, int y_move)
    {
        int newX = position.x + x_move;
        int newY = position.y + y_move;

        newX = Math.Clamp(newX, 0, (Raylib.GetScreenWidth() / Game.tileSize) - 1);
        newY = Math.Clamp(newY, 0, (Raylib.GetScreenHeight() / Game.tileSize) - 1);

        position.x = newX;
        position.y = newY;
    }

    public void Draw()
    {
        int pixelX = position.x * Game.tileSize;
        int pixelY = position.y * Game.tileSize;

        Rectangle sourceRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

        Raylib.DrawTextureRec(image, sourceRect, new Vector2(pixelX, pixelY), Raylib.WHITE);
    }



    public void SetImageAndIndex(Texture atlasImage, int imagesPerRow, int index)
    {
        image = atlasImage;
        imagePixelX = (index % imagesPerRow) * Game.tileSize;
        imagePixelY = (int)(index / imagesPerRow) * Game.tileSize;
    }


    public enum Race
    {
        Human,
        Elf,
        Dwarf,
        Penguin
    }

    public enum Class
    {
        Warrior,
        Mage,
        Rogue,
        Cleric
    }

    private string name;
    private Race race;
    private Class classType;

    public string Nimi
    {
        get { return name; }
        set { name = value; }
    }

    public Race Rotu
    {
        get { return race; }
        set { race = value; }
    }

    public Class Hahmoluokka
    {
        get { return classType; }
        set { classType = value; }
    }

    public PlayerCharacter(string name, Race race, Class classType)
    {
        this.name = name;
        this.race = race;
        this.classType = classType;
    }

    public void PrintDetails()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ---Your Character--- ");
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Race: {race}");
        Console.WriteLine($"Class: {classType}");
    }
}
