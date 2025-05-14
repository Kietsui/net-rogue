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
    public int x;
    public int y;
    private Texture graphics;
    private int drawIndex;

    public Enemy(string name, Vector2 position, Texture graphics, int drawIndex, int x, int y)
    {
        this.name = name;
        this.position = position;
        this.graphics = graphics;
        this.drawIndex = drawIndex;
        this.x = x;
        this.y = y;
    }

    public void Draw()
    {
        Raylib.DrawTexturePro(graphics, GetSourceRectangle(), GetDestinationRectangle(), Vector2.Zero, 0f, Raylib.WHITE);
    }

    private Rectangle GetSourceRectangle()
    {
        int tileX = (drawIndex % Game.imagesPerRow) * Game.tileSize;
        int tileY = (drawIndex / Game.imagesPerRow) * Game.tileSize;
        return new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize);
    }

    private Rectangle GetDestinationRectangle()
    {
        return new Rectangle(position.X * Game.tileSize, position.Y * Game.tileSize, Game.tileSize, Game.tileSize);
    }
}

public class Item
{
    public string name;
    public Vector2 position;
    public int x;
    public int y;
    private Texture graphics;
    private int drawIndex;

    public Item(string name, Vector2 position, Texture graphics, int drawIndex, int x, int y) // Add x and y parameters
    {
        this.name = name;
        this.position = position;
        this.graphics = graphics;
        this.drawIndex = drawIndex;
        this.x = x;
        this.y = y;
    }

    public void Draw()
    {
        Raylib.DrawTexturePro(graphics, GetSourceRectangle(), GetDestinationRectangle(), Vector2.Zero, 0f, Raylib.WHITE);
    }

    private Rectangle GetSourceRectangle()
    {
        int tileX = (drawIndex % Game.imagesPerRow) * Game.tileSize;
        int tileY = (drawIndex / Game.imagesPerRow) * Game.tileSize;
        return new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize);
    }

    private Rectangle GetDestinationRectangle()
    {
        return new Rectangle(position.X * Game.tileSize, position.Y * Game.tileSize, Game.tileSize, Game.tileSize);
    }
}
