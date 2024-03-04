using System;

public class Map
{
    public int mapWidth;
    public int[] mapTiles;

    public Map(int width, int[] tiles)
    {
        mapWidth = width;
        mapTiles = tiles;
    }

    public void Draw()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        int mapHeight = mapTiles.Length / mapWidth;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int index = x + y * mapWidth;
                int tileId = mapTiles[index];

                Console.SetCursorPosition(x, y);
                switch (tileId)
                {
                    case 1:
                        Console.Write(".");
                        break;
                    case 2:
                        Console.Write("#");
                        break;
                    default:
                        Console.Write(" ");
                        break;
                }
            }
        }
    }
}
