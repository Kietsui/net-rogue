using System;
using System.IO;
using Newtonsoft.Json;
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
    }

    public Map LoadTestMap()
    {
        string jsonFilePath = "Maps/mapfile.json";

        int imagesPerRow = 12;
        int wallIndex = 1;
        int floorIndex = 0;

        try
        {
            string json = File.ReadAllText(jsonFilePath);
            MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

            return new Map(mapData.mapWidth, mapData.mapTiles, atlas, imagesPerRow, wallIndex, floorIndex);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading map from JSON file: {e.Message}");
            return new Map(8, new int[]
            {
                2, 2, 2, 2, 2, 2, 2, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 2, 2,
                2, 2, 2, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 2, 2, 2, 2, 2, 2, 2
            }, atlas, imagesPerRow, wallIndex, floorIndex);
        }
    }
}
