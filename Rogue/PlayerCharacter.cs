using System;

public class PlayerCharacter
{
    public Point2D position;

    private char image;
    private ConsoleColor color;

    public PlayerCharacter(char image, ConsoleColor color)
    {
        this.image = image;
        this.color = color;
    }

    public void Move(int x_move, int y_move)
    {
        position.x += x_move;
        position.y += y_move;

        position.x = Math.Clamp(position.x, 0, Console.WindowWidth - 1);
        position.y = Math.Clamp(position.y, 0, Console.WindowHeight - 1);
    }

    public void Draw()
    {
        Console.ForegroundColor = color;
        Console.SetCursorPosition(position.x, position.y);
        Console.Write(image);
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
