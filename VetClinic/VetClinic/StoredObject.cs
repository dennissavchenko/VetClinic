using System.Text.Json;

namespace VetClinic;

public abstract class StoredObject<T>
{
    
    private static List<T> _extent = new List<T>();
    private static string _path = "../../../Data/" + typeof(T).Name + ".json";
    
    public static List<T> GetExtent()
    {
        LoadExtent();
        return _extent;
    }
    
    public static void AddToExtent(T obj)
    {
        LoadExtent();
        
        if (obj is IIdentifiable identifiable)
        {
            identifiable.Id = _extent.Count + 1;
        }
        
        _extent.Add(obj);
        SaveExtent();
    }
    
    public static void SaveExtent()
    {
        string json = JsonSerializer.Serialize(_extent, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_path, json);
    }
    public static void LoadExtent()
    {
        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            try
            {
                _extent = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            } catch (JsonException)
            {
                _extent = new List<T>();
            }
        }
        else
        {
            _extent = new List<T>();
        }
    }
    
}