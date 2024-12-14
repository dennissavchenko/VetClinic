using System.Text;
using System.Text.Json;

namespace VetClinic;

public abstract class StoredObject<T> where T : IIdentifiable
{
    
    private static List<T> _storageExtent = new ();
    protected static List<T> _extent = new();
    private static string _path = "../../../Data/" + typeof(T).Name + ".json";
    
    protected static List<T> GetExtent()
    {
        LoadExtent();
        return _storageExtent;
    }
    
    public static List<T> GetCurrentExtent()
    {
        return new List<T>(_extent);
    }
    
    protected static void AddToExtent(T obj)
    {
        LoadExtent();
        if (obj.Id < 1)
        {
            obj.Id = _storageExtent.Count + 1;
        }
        _storageExtent.Add(obj);
        SaveExtent();
    }
    protected static void SaveExtent()
    {
        string json = JsonSerializer.Serialize(_storageExtent, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_path, json);
    }
    private static void LoadExtent()
    {
        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            try
            {
                _storageExtent = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (JsonException)
            {
                _storageExtent = new List<T>();
            }
        }
        else
        {
            _storageExtent = new List<T>();
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

    public static List<string> GetExtentAsString()
    {
        List<string> list = new();
        foreach (var obj in GetExtent())
        {
            list.Add(obj.ToString() ?? "");
        }
        return list;
    }

}