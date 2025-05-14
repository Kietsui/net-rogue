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
    public int[] mapTiles { get { return layers[0].mapTiles; }}
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

    public Map()
    {
        layers = new MapLayer[3];
    }

    /// <summary>
    /// Alustaa kartan ja siihen liittyvät kerrokset sekä lataa viholliset ja esineet.
    /// </summary>
    /// <param name="width">Kartan leveys ruuduissa.</param>
    /// <param name="tiles">ground layer ruutujen indeksit.</param>
    /// <param name="atlas">Käytettävä tekstuurikartta.</param>
    /// <param name="imagesPerRow">Kuvien määrä rivillä tekstuurissa.</param>
    /// <param name="wallIdx">Seinäruudun indeksitekstuurissa.</param>
    /// <param name="floorIdx">Lattiaruudun indeksitekstuurissa.</param>
    /// <param name="itemLayer">Item layerin ruutujen indeksit.</param>
    /// <param name="enemyLayer">Enemy layerin ruutujen indeksit.</param>
    public Map(int width, int[] tiles, Texture atlas, int imagesPerRow, int wallIdx, int floorIdx, int[] itemLayer, int[] enemyLayer)
    {
        this.mapWidth = width;
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
                if ( enemyTileId != 0)
                {
                    enemies.Add(new Enemy("Orc", position, spriteAtlas, enemyTileId -1, x, y));
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
                
                if ( itemTileId != 0) 
                { 
                    items.Add(new Item("Health Potion", position, spriteAtlas, itemTileId -1, x, y));
                }
            }
        }
    }


    /// <summary>
    /// Piirtää koko kartan kaikki tasot: maaston, esineet ja viholliset.
    /// </summary>
    /// <param name="myImage">Tekstuurikuva (sprite atlas), jota käytetään piirtoon.</param>
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


    /// <summary>
    /// Piirtää maalaatat (esim. lattia ja seinä) annetun tason mukaan.
    /// </summary>
    /// <param name="myImage">Tekstuurikuva (sprite atlas), josta laatat leikataan.</param>
    /// <param name="layer">Karttataso, joka sisältää maastolaattojen tiedot.</param>
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

                sourceRect = GetTileRec(imagesPerRow, tileIndex -1);

                Raylib.DrawTexturePro(myImage, sourceRect, destRect, Vector2.Zero, 0f, Raylib.WHITE);
            }
        }
    }

    /// <summary>
    /// Piirtää kartalla olevat esineet kutsumalla jokaisen esineen Draw-metodia.
    /// </summary>
    /// <param name="myImage">Tekstuurikuva (ei käytetä suoraan tässä metodissa).</param>
    /// <param name="layer">map layer, joka sisältää esinetiedot.</param>
    public void DrawItems(Texture myImage, MapLayer layer)
    {
        if (layer == null)
        {
            Console.WriteLine("Error: Items layer is null.");
            return;
        }

        for (int x = 0; x < items.Count; x++)
        {
            items[x].Draw();
        }

    }

    public void DrawEnemies(Texture myImage, MapLayer layer)
    {
        if (layer == null)
        {
            Console.WriteLine("Error: Items layer is null.");
            return;
        }

        for (int x  = 0; x < enemies.Count; x++)
        {
            enemies[x].Draw();
        }
    }


    private Rectangle GetTileRec(int imagesPerRow, int tileIndex)
    {
        int tileX = (tileIndex % Game.imagesPerRow) * Game.tileSize;
        int tileY = (tileIndex / Game.imagesPerRow) * Game.tileSize;
        return new Rectangle(tileX, tileY, Game.tileSize, Game.tileSize);
    }
}
