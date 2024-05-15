using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZeroElectric.Vinculum;

public class MapLoader
{
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
