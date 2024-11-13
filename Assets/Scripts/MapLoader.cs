using System.IO;
using UnityEngine;

public class MapLoader
{
    public static string LoadPremadeMap(string mapFilePath)
    {
        // Creates instance of stream reader and assigns it map path.
        StreamReader streamReader = new StreamReader(mapFilePath);

        // Read all characters from map and return as single string. 
        string content = streamReader.ReadToEnd();
        Debug.Log(content);

        // Return the string.
        return content;
    }
}