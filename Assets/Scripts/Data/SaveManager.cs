using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private static readonly string nombreArchivoGuardado = "save.json";
    private static string savePath => Path.Combine(Application.persistentDataPath, nombreArchivoGuardado);

    public static void GuardarJuego(SaveData data)
    {
        //Metadatos.
        data.fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        data.version = Application.version;

        //JSON
        string json = JsonUtility.ToJson(data,true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Juego guardado en: {savePath}");
    }

    public static SaveData CargarJuego()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No hay archivo de guardado.");
            return null;
        }
        else
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"Archivo de guardado cargado con fecha: {data.fecha}");
            return data;
        }
    }

    public static bool SaveExists() => File.Exists(savePath);

    public static void BorrarDatos()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Archivo dde guardado eliminado");
        }
    }
}
