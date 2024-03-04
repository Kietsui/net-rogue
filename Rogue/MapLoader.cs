using System;
using System.IO;
using Newtonsoft.Json;


public class MapLoader
{
    public class MapData
    {
        public int mapWidth { get; set; }
        public int[] mapTiles { get; set; }
    }

    public Map LoadTestMap()
    {
        string jsonFilePath = "Maps/mapfile.json"; // Update this path to your JSON file

        try
        {
            string json = File.ReadAllText(jsonFilePath);
            MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

            return new Map(mapData.mapWidth, mapData.mapTiles);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading map from JSON file: {e.Message}");
            // Return a default map or handle the error as needed
            return new Map(8, new int[]
            {
            1, 2, 2, 2, 2, 2, 2, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 2, 2,
            2, 2, 2, 2, 1, 1, 1, 2,
            2, 1, 1, 1, 1, 1, 1, 2,
            2, 2, 2, 2, 2, 2, 2, 2
            });
        }
    }
}
