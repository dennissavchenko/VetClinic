using System.Text;
using System.Text.Json;

namespace VetClinic;

public abstract class StoredObject<T> where T : IIdentifiable
{
    
    private static List<T> _extent = new ();
    private static string _path = "../../../Data/" + typeof(T).Name + ".json";
    
    protected static List<T> GetExtent()
    {
        LoadExtent();
        return _extent;
    }
    protected static void AddToExtent(T obj)
    {
        LoadExtent();
        if (obj.Id < 1)
        {
            obj.Id = _extent.Count + 1;
        }
        _extent.Add(obj);
        SaveExtent();
    }
    private static void SaveExtent()
    {
        string json = JsonSerializer.Serialize(_extent, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_path, json);
    }
    private static void LoadExtent()
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
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine(typeof(T).Name + " extent:");
        Console.WriteLine("------------------------------------------------");
        foreach (var obj in GetExtent())
        {
            Console.WriteLine(obj);
        }
    }

    public new static List<string> GetExtentAsString()
    {
        List<string> list = new();
        foreach (var obj in GetExtent())
        {
            list.Add(obj.ToString() ?? "");
        }
        return list;
    }
    
}