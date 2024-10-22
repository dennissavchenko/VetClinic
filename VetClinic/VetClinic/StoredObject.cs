using System.Text.Json;

namespace VetClinic;

public abstract class StoredObject<T> where T : IIdentifiable
{
    
    private static List<T> _extent = new ();
    private static string _path = "../../../Data/" + typeof(T).Name + ".json";
    
    public static List<T> GetExtent()
    {
        LoadExtent();
        return _extent;
    }
    public static void UpdateExtent(List<T> extent)
    {
        _extent = extent;
        SaveExtent();
    }
    public static void AddToExtent(T obj)
    {
        LoadExtent();
        if (obj.Id < 1) {
            obj.Id = _extent.Count + 1;
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
            }
            catch (JsonException)
            {
                _extent = new List<T>();
            }
        }
        else
        {
            _extent = new List<T>();
        }
    }
    public static void PrintExtent()
    {
        LoadExtent();
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(typeof(T).Name + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var obj in _extent)
        {
            Console.WriteLine(obj);
        }
        
    }
    
}