using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaverLoader
{
    private const string FILENAME   = "savedata_";
    private const string FOLDERNAME = "out";

    //private BinaryWriter writer;
    //private BinaryReader reader;

    public void SaveGame(int slotNum)
    {
        if (!Directory.Exists(FOLDERNAME))
            Directory.CreateDirectory(FOLDERNAME);

        BinaryWriter     writer            = new BinaryWriter(File.Open(FOLDERNAME + "\\" + FILENAME, FileMode.CreateNew));
        var              binaryFormatter   = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        string           __filename        = FILENAME + slotNum.ToString();
        GameObject       player            = GameObject.FindGameObjectWithTag("Player"); 
        List<GameObject> houses            = new List<GameObject>();
        List<GameObject> goblinsManagers   = new List<GameObject>();
        List<GameObject> villagersManagers = new List<GameObject>();

        houses.AddRange(GameObject.FindGameObjectsWithTag("Village"));
        goblinsManagers.AddRange(GameObject.FindGameObjectsWithTag("Goblins Manager"));
        villagersManagers.AddRange(GameObject.FindGameObjectsWithTag("Villagers Manager"));

        WriteToBinaryFile<GameObject>("qqq", player);

        player = ReadFromBinaryFile<GameObject>("qqq");
    }











    /// <summary>
    /// Writes the given object instance to a binary file.
    /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
    /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the XML file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    private static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    /// <summary>
    /// Reads an object instance from a binary file.
    /// </summary>
    /// <typeparam name="T">The type of object to read from the XML.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the binary file.</returns>
    private static T ReadFromBinaryFile<T>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
}
