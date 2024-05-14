using System;
using System.Numerics;
using ZeroElectric.Vinculum;

public class Map
{
    public enum MapTile : int
    {
        Floor = 1,
        Wall = 2
    }

    public int mapWidth;
    public int[] mapTiles;
    private Texture atlas;
    private int imagesPerRow;
    private int wallIndex;
    private int floorIndex;

    public Map(int width, int[] tiles, Texture atlas, int imagesPerRow, int wallIdx, int floorIdx)
    {
        this.mapWidth = width;
        this.mapTiles = tiles;
        this.atlas = atlas;
        this.imagesPerRow = imagesPerRow;
        wallIndex = wallIdx;
        floorIndex = floorIdx;
    }

    public void Draw()
    {
        int mapHeight = mapTiles.Length / mapWidth;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int index = x + y * mapWidth;
                MapTile tileType = (MapTile)mapTiles[index];

                int pixelX = x * Game.tileSize;
                int pixelY = y * Game.tileSize;

                Rectangle sourceRect = new Rectangle();
                int textureIndex = 0;

                if (tileType == MapTile.Floor)
                {
                    textureIndex = floorIndex;
                }
                else if (tileType == MapTile.Wall)
                {
                    textureIndex = wallIndex;
                }

                sourceRect = GetSourceRect(textureIndex);

                Rectangle destRect = new Rectangle(pixelX, pixelY, Game.tileSize, Game.tileSize);
                Raylib.DrawTextureRec(atlas, sourceRect, new Vector2(pixelX, pixelY), Raylib.WHITE);
            }
        }
    }


    private Rectangle GetSourceRect(int index)
    {
        int row = index / imagesPerRow;
        int col = index % imagesPerRow;
        return new Rectangle(col * Game.tileSize, row * Game.tileSize, Game.tileSize, Game.tileSize);
    }
}
