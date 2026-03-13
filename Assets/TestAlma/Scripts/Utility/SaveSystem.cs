using System.IO;
using System.Threading.Tasks;
using UnityEngine;


public static class SaveSystem 
{
    
    private const string SAVE_PATH = "Save.data";

    private static string path => Path.Combine(Application.persistentDataPath, SAVE_PATH);
    
    
    public static void SaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);

        using StreamWriter writer = new StreamWriter(path);
        
        writer.Write(json);
    }
    
    public static async Task SaveDataAsync<T>(T data)
    {
        string json = JsonUtility.ToJson(data);
        
        await using StreamWriter writer = new StreamWriter(path);
        await writer.WriteAsync(json);
    }

    public static T LoadData<T>() where T : new()
    {
        if (!IsFileExist()) return new T();
        
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        
        var data = JsonUtility.FromJson<T>(json);
        return data;
    }
    
    public static T LoadDataAsync<T>() where T : new()
    {
        if (!IsFileExist()) return new T();
        
        using StreamReader reader = new StreamReader(path);
        var t = reader.ReadToEndAsync();
        
        string json = t.Result;
        var data = JsonUtility.FromJson<T>(json);
        return data;
    }
    
    public static void DeleteData()
    {
        if (!IsFileExist()) return;
        File.Delete(path);
    }
    
    private static bool IsFileExist()
    {
        return File.Exists(path);
    }



    
    
   
}

