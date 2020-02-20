using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaverLoader : MonoBehaviour
{
    private const string FILENAME   = "savedata_";
    private const string FOLDERNAME = "out";
    private       string PATH       = FOLDERNAME + "\\" + FILENAME;

    // Instruments.
    private BinaryWriter writer;
    private BinaryReader reader;

    // Data for saving.
    GameObject       player            = null;
    List<GameObject> houses            = new List<GameObject>();
    List<GameObject> goblinsManagers   = new List<GameObject>();
    List<GameObject> villagersManagers = new List<GameObject>();

    public void SaveGame(int slotNum)
    {
        string __path = PATH + slotNum.ToString();

        if (!Directory.Exists(FOLDERNAME))
            Directory.CreateDirectory(FOLDERNAME);

        writer = new BinaryWriter(File.Open(__path, FileMode.OpenOrCreate));
        player = GameObject.FindGameObjectWithTag("Player"); 

        houses.AddRange(GameObject.FindGameObjectsWithTag("Village"));
        goblinsManagers.AddRange(GameObject.FindGameObjectsWithTag("Goblins Manager"));
        villagersManagers.AddRange(GameObject.FindGameObjectsWithTag("Villagers Manager"));

        WriteVector3(player.transform.position);
        WriteVector3(player.transform.forward);

        if (goblinsManagers.Count > 0)
        {
            writer.Write(goblinsManagers.Count);
            foreach (GameObject gm in goblinsManagers)
            {
                if (gm.GetComponent<GoblinsManager>().Goblins.Count > 0)
                {
                    writer.Write(gm.GetComponent<GoblinsManager>().Goblins.Count);
                    WriteVector3(gm.GetComponent<GoblinsManager>().BasePoint.transform.position);
                    foreach (GameObject goblin in gm.GetComponent<GoblinsManager>().Goblins)
                    {
                        WriteVector3(goblin.transform.position);
                        WriteVector3(goblin.transform.forward);
                        print(goblin.GetComponent<GoblinBrains>().Type.ToString());
                        writer.Write(goblin.GetComponent<GoblinBrains>().Type.ToString());
                    }
                }
                else
                {
                    writer.Write(0);
                }
            }
        }
        else
        {
            writer.Write(0);
        }

        if (villagersManagers.Count > 0)
        {
            writer.Write(villagersManagers.Count);
            foreach (GameObject vm in villagersManagers)
            {
                if (vm.GetComponent<VillagersManager>().Villagers.Count > 0)
                {
                    writer.Write(vm.GetComponent<VillagersManager>().Villagers.Count);
                    writer.Write(vm.GetComponent<VillagersManager>().SpawnPoints.Length);
                    foreach (GameObject spawnPoint in vm.GetComponent<VillagersManager>().SpawnPoints)
                        WriteVector3(spawnPoint.transform.position);
                    foreach (GameObject vilager in vm.GetComponent<VillagersManager>().Villagers)
                    {
                        WriteVector3(vilager.transform.position);
                        WriteVector3(vilager.transform.forward);
                    }
                }
                else
                {
                    writer.Write(0);
                }
            }
        }
        else
        {
            writer.Write(0);
        }




        writer.Close();
        writer = null;
    }

    public void LoadGame(int slotNum)
    {
        GameSetUper gameSetUper = GetComponent<GameSetUper>();
        string      __path      = PATH + slotNum.ToString();

        if (!Directory.Exists(FOLDERNAME) || !File.Exists(__path))
            return;

        reader = new BinaryReader(File.Open(__path, FileMode.Open, FileAccess.Read));



        reader.Close();
        reader = null;
    }

    private void WriteVector3(Vector3 v)
    {
        writer.Write(v.x);
        writer.Write(v.y);
        writer.Write(v.z);
    }

    private Vector3 ReadVector3()
    {
        return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
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

[System.Serializable]
class SerializedVector3
{
    private float x;
    private float y;
    private float z;

    public SerializedVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
