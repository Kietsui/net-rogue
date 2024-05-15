using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
    public MapLayer[] layers;
    private Texture atlas;
    private int imagesPerRow;
    private int wallIndex;
    private int floorIndex;

    public List<Enemy> enemies;
    public List<Item> items;

    public MapLayer GetLayer(string layerName)
    {
        switch (layerName)
        {
            case "ground":
                return layers[0];
            case "items":
                return layers[1];
            case "enemies":
                return layers[2];
            default:
                Console.WriteLine($"Error: No layer with name: {layerName}");
                return null;
        }
    }

    public Map(int width, int[] tiles, Texture atlas, int imagesPerRow, int wallIdx, int floorIdx, int[] itemLayer, int[] enemyLayer)
    {
        this.mapWidth = width;
        this.mapTiles = tiles;
        this.atlas = atlas;
        this.imagesPerRow = imagesPerRow;
        wallIndex = wallIdx;
        floorIndex = floorIdx;

        layers = new MapLayer[3];

        layers[0] = new MapLayer();
        layers[0].mapTiles = tiles;

        layers[1] = new MapLayer();
        layers[1].mapTiles = itemLayer;

        layers[2] = new MapLayer();
        layers[2].mapTiles = enemyLayer;

        LoadEnemiesAndItems(atlas, enemyLayer, itemLayer);
    }

    public void LoadEnemiesAndItems(Texture spriteAtlas, int[] enemyLayer, int[] itemLayer)
    {
        enemies = new List<Enemy>();
        int mapHeight = enemyLayer.Length / mapWidth;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Vector2 position = new Vector2(x, y);
                int index = x + y * mapWidth;
                int enemyTileId = enemyLayer[index];
                switch (enemyTileId)
                {
                    case 0:
                        break;
                    case 1:
                        enemies.Add(new Enemy("Orc", position, spriteAtlas, enemyTileId, x, y));
                        break;
                }
            }
        }

        items = new List<Item>();
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Vector2 position = new Vector2(x, y);
                int index = x + y * mapWidth;
                int itemTileId = itemLayer[index];
                switch (itemTileId)
                {
                    case 0:
                        break;
                    case 1:
                        items.Add(new Item("Health Potion", position, spriteAtlas, itemTileId, x, y));
                        break;
                }
            }
        }
    }

    public void Draw(Texture myImage)
    {
        if (layers == null)
        {
            Console.WriteLine("Error: Map layers are null.");
            return;
        }

        DrawMapTiles(myImage, layers[0]);
        DrawItems(myImage, layers[1]);
        DrawEnemies(myImage, layers[2]);
    }

    public void DrawMapTiles(Texture myImage, MapLayer layer)
    {
        if (layer == null)
        {
            Console.WriteLine("Error: Map layer is null.");
            return;
        }

        int[] mapTiles = layer.mapTiles;
        int mapHeight = mapTiles.Length / mapWidth;

        int wallX = 4, wallY = 3;
        int floorX = 0, floorY = 4;
        int wallIndex = wallY * imagesPerRow + wallX;
        int floorIndex = floorY * imagesPerRow + floorX;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int index = x + y * mapWidth;
                int tileIndex = mapTiles[index];

                Rectangle sourceRect;
                Rectangle destRect = new Rectangle(x * Game.tileSize, y * Game.tileSize, Game.tileSize, Game.tileSize);

                switch ((MapTile)tileIndex)
                {
                    case MapTile.Floor:
                        sourceRect = GetTileRec(imagesPerRow, floorIndex);
                        break;
                    case MapTile.Wall:
                        sourceRect = GetTileRec(imagesPerRow, wallIndex);
                        break;
                    default:
                        continue;
                }

                Raylib.DrawTexturePro(myImage, sourceRect, destRect, Vector2.Zero, 0f, Raylib.WHITE);
            }
        }
    }

    public void DrawItems(Texture myImage, MapLayer layer)
    {
        if (layer == null)
        {
            Console.WriteLine("Error: Items layer is null.");
            return;
        }

        int mapHeight = layer.mapTiles.Length / mapWidth;
        int itemTileIndex = 108; // Hardcoded tile index for items

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int index = x + y * mapWidth;
                int tileIndex = layer.mapTiles[index];

                if (tileIndex == 1)
                {
                    Rectangle sourceRect = GetTileRec(imagesPerRow, itemTileIndex);
                    Rectangle destRect = new Rectangle(x * Game.tileSize, y * Game.tileSize, Game.tileSize, Game.tileSize);
                    Raylib.DrawTexturePro(myImage, sourceRect, destRect, Vector2.Zero, 0f, Raylib.WHITE);
                }
            }
        }
    }

    public void DrawEnemies(Texture myImage, MapLayer layer)
    {
        if (layer == null)
        {
            Console.WriteLine("Error: Enemies layer is null.");
            return;
        }

        int mapHeight = layer.mapTiles.Length / mapWidth;
        int enemyTileIndex = 109; // Hardcoded tile index for enemies

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int index = x + y * mapWidth;
                int tileIndex = layer.mapTiles[index];

                if (tileIndex == 1)
                {
                    Rectangle sourceRect = GetTileRec(imagesPerRow, enemyTileIndex);
                    Rectangle destRect = new Rectangle(x * Game.tileSize, y * Game.tileSize, Game.tileSize, Game.tileSize);
                    Raylib.DrawTexturePro(myImage, sourceRect, destRect, Vector2.Zero, 0f, Raylib.WHITE);
                }
            }
        }
    }

    private Rectangle GetTileRec(int imagesPerRow, int tileIndex)
    {
        int tileWidth = atlas.width / imagesPerRow;
        int tileHeight = atlas.height / imagesPerRow;
        int tileX = (tileIndex % imagesPerRow) * tileWidth;
        int tileY = (tileIndex / imagesPerRow) * tileHeight;
        return new Rectangle(tileX, tileY, tileWidth, tileHeight);
    }
}
