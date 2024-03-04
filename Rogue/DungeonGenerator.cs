using System;

public class DungeonGenerator
{
    private int width;
    private int height;
    private int[,] map;

    public DungeonGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
        map = new int[width, height];
    }

    public int[,] GenerateDungeon()
    {
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            DoCellularAutomataIteration();
        }

        return map;
    }

    private void RandomFillMap()
    {
        Random rand = new Random();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (rand.Next(0, 100) < 55) ? 1 : 0; // 45% chance of being a wall
            }
        }
    }

    private void DoCellularAutomataIteration()
    {
        int[,] newMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int surroundingWallTiles = CountSurroundingWallTiles(x, y);

                if (surroundingWallTiles > 4)
                {
                    newMap[x, y] = 1;
                }
                else if (surroundingWallTiles < 4)
                {
                    newMap[x, y] = 0;
                }
            }
        }

        map = newMap;
    }

    private int CountSurroundingWallTiles(int x, int y)
    {
        int wallCount = 0;
        for (int neighborX = x - 1; neighborX <= x + 1; neighborX++)
        {
            for (int neighborY = y - 1; neighborY <= y + 1; neighborY++)
            {
                if (IsInMapRange(neighborX, neighborY))
                {
                    if (neighborX != x || neighborY != y)
                    {
                        wallCount += map[neighborX, neighborY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    private bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
