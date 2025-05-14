using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TurboMapReader;
using ZeroElectric.Vinculum;

public class MapLoader
{
    public Map? ReadMapFromFile(string filename)
    {
        // Lataa tiedosto käyttäen TurboMapReaderia   
        TurboMapReader.TiledMap mapMadeInTiled = TurboMapReader.MapReader.LoadMapFromFile(filename);

        // Tarkista onnistuiko lataaminen
        if (mapMadeInTiled != null)
        {
            // Muuta Map olioksi ja palauta
            return ConvertTiledMapToMap(mapMadeInTiled);
        }
        else
        {
            // OH NO!
            return null;
        }
    }
    /// <summary>
    /// muuttaa tiled kartan olio mapiksi
    /// tekee ground, item ja enemy layerin karttaan, jotta voidaan piirtää kaikki nämä oikein
    /// </summary>
    /// <param name="turboMap"></param>
    /// <returns></returns>
    public Map ConvertTiledMapToMap(TiledMap turboMap)
    {
        // Luo tyhjä kenttä
        Map rogueMap = new Map();

        // Muunna tason "ground" tiedot
        TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");
        TurboMapReader.MapLayer enemyLayer = turboMap.GetLayerByName("enemies");
        TurboMapReader.MapLayer itemLayer = turboMap.GetLayerByName("items"); ;

        rogueMap.mapWidth = groundLayer.width;

        int howManyTiles = groundLayer.data.Length;
        int[] groundTiles = groundLayer.data;
        // string name = groundLayer.name;

        int howManyEnemyTiles = enemyLayer.data.Length;
        int[] enemyTiles = enemyLayer.data;
        // string nameEnemy = enemyLayer.name;

        int howManyItemTiles = itemLayer.data.Length;
        int[] itemTiles = itemLayer.data;
        // string nameItem = itemLayer.name;


        // Luo uusi taso tietojen perusteella
        MapLayer myGroundLayer = new MapLayer();
        myGroundLayer.name = "ground";

        MapLayer myEnemyLayer = new MapLayer();
        myEnemyLayer.name = "enemies";

        MapLayer myItemLayer = new MapLayer();
        myItemLayer.name = "items";


        myGroundLayer.mapTiles = groundTiles;
        myEnemyLayer.mapTiles = enemyTiles;
        myItemLayer.mapTiles = itemTiles;


        // Tallenna taso kenttään
        rogueMap.layers[0] = myGroundLayer;
        rogueMap.layers[1] = myItemLayer;
        rogueMap.layers[2] = myEnemyLayer;

        // Lopulta palauta kenttä
        return rogueMap;
    }

    private Texture atlas;

    public void SetTexture(Texture texture)
    {
        atlas = texture;
    }

    public class MapData
    {
        public int mapWidth { get; set; }
        public int[] mapTiles { get; set; }
        public int[] enemyLayer { get; set; }
        public int[] itemLayer { get; set; }
    }

    public class LayerData
    {
        public string name { get; set; }
        public int[] mapTiles { get; set; }
    }

    //load test map as a fallback
    public Map LoadTestMap()
    {
        string jsonFilePath = "Maps/Layers/mapfile_layers.json";

        int imagesPerRow = 12;
        int wallX = 4;
        int wallY = 3;
        int floorX = 0;
        int floorY = 5;

        int wallIndex = wallY * imagesPerRow + wallX;
        int floorIndex = floorY * imagesPerRow + floorX;

        try
        {
            string json = File.ReadAllText(jsonFilePath);
            dynamic jsonData = JsonConvert.DeserializeObject(json);

            dynamic layers = jsonData.layers;

            // Find the correct layers by name
            dynamic groundLayer = null;
            dynamic itemsLayer = null;
            dynamic enemiesLayer = null;

            foreach (dynamic layer in layers)
            {
                if (layer.name == "ground")
                    groundLayer = layer;
                else if (layer.name == "items")
                    itemsLayer = layer;
                else if (layer.name == "enemies")
                    enemiesLayer = layer;
            }

            // Check if all layers were found
            if (groundLayer == null || itemsLayer == null || enemiesLayer == null)
            {
                throw new Exception("Missing layer in JSON data.");
            }

            return new Map(
                (int)jsonData.mapWidth,
                ((JArray)groundLayer.mapTiles).ToObject<int[]>(),
                atlas,
                imagesPerRow,
                wallIndex,
                floorIndex,
                ((JArray)itemsLayer.mapTiles).ToObject<int[]>(),
                ((JArray)enemiesLayer.mapTiles).ToObject<int[]>()
            );
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading map from JSON file: {e.Message}");
            return new Map(8, new int[]
            {
        // Default map data if loading fails
        2, 2, 2, 2, 2, 2, 2, 2,
        2, 1, 1, 2, 1, 1, 1, 2,
        2, 1, 1, 2, 1, 1, 1, 2,
        2, 1, 1, 1, 1, 1, 2, 2,
        2, 2, 2, 2, 1, 1, 1, 2,
        2, 1, 1, 1, 1, 1, 1, 2,
        2, 2, 2, 2, 2, 2, 2, 2
            }, atlas, imagesPerRow, wallIndex, floorIndex, new int[0], new int[0]);
        }
    }
}
