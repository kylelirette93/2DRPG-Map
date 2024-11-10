using System.IO;
using UnityEngine;

public class MapLoader
{
    public static string LoadPremadeMap(string mapFilePath)
    {
        StreamReader streamReader = new StreamReader(mapFilePath);
        string content = streamReader.ReadToEnd();
        Debug.Log(content);
        return content;
    }
}