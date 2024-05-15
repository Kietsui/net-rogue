using Microsoft.VisualBasic;
using System.Numerics;
using ZeroElectric.Vinculum;

public class MapLayer
{
    public string name;
    public int[] mapTiles;
}

public class Enemy
{
    public string name;
    public Vector2 position;
    public int x; // Add x property
    public int y; // Add y property
    private Texture graphics;
    private int drawIndex;

    public Enemy(string name, Vector2 position, Texture graphics, int drawIndex, int x, int y) // Add x and y parameters
    {
        this.name = name;
        this.position = position;
        this.graphics = graphics;
        this.drawIndex = drawIndex;
        this.x = x; // Assign x parameter to the property
        this.y = y; // Assign y parameter to the property
    }

    public void Draw()
    {
        Raylib.DrawTexturePro(graphics, GetSourceRectangle(), GetDestinationRectangle(), Vector2.Zero, 0f, Raylib.WHITE);
    }

    private Rectangle GetSourceRectangle()
    {
        // Calculate source rectangle based on drawIndex
        int tileWidth = graphics.width / Game.imagesPerRow;
        int tileHeight = graphics.height / Game.imagesPerRow;
        int tileX = (drawIndex % Game.imagesPerRow) * tileWidth;
        int tileY = (drawIndex / Game.imagesPerRow) * tileHeight;
        return new Rectangle(tileX, tileY, tileWidth, tileHeight);
    }

    private Rectangle GetDestinationRectangle()
    {
        // Calculate destination rectangle based on position and tile size
        return new Rectangle(position.X * Game.tileSize, position.Y * Game.tileSize, Game.tileSize, Game.tileSize);
    }
}

public class Item
{
    public string name;
    public Vector2 position;
    public int x; // Add x property
    public int y; // Add y property
    private Texture graphics;
    private int drawIndex;

    public Item(string name, Vector2 position, Texture graphics, int drawIndex, int x, int y) // Add x and y parameters
    {
        this.name = name;
        this.position = position;
        this.graphics = graphics;
        this.drawIndex = drawIndex;
        this.x = x; // Assign x parameter to the property
        this.y = y; // Assign y parameter to the property
    }

    public void Draw()
    {
        Raylib.DrawTexturePro(graphics, GetSourceRectangle(), GetDestinationRectangle(), Vector2.Zero, 0f, Raylib.WHITE);
    }

    private Rectangle GetSourceRectangle()
    {
        // Calculate source rectangle based on drawIndex
        int tileWidth = graphics.width / Game.imagesPerRow;
        int tileHeight = graphics.height / Game.imagesPerRow;
        int tileX = (drawIndex % Game.imagesPerRow) * tileWidth;
        int tileY = (drawIndex / Game.imagesPerRow) * tileHeight;
        return new Rectangle(tileX, tileY, tileWidth, tileHeight);
    }

    private Rectangle GetDestinationRectangle()
    {
        // Calculate destination rectangle based on position and tile size
        return new Rectangle(position.X * Game.tileSize, position.Y * Game.tileSize, Game.tileSize, Game.tileSize);
    }
}
