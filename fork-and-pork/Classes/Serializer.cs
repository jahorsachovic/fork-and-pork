using System.Text.Json;
using System.Text.Json.Serialization;

namespace fork_and_pork.Classes;

public static class ObjectStore
{
    private static Dictionary<Type, List<object>> _store = new Dictionary<Type, List<object>>();

    public static void Add(object obj) 
    {
        Type objType = obj.GetType();
        
        if (!_store.TryGetValue(objType, out List<object> items))
        {
            items = new List<object>();
            _store[objType] = items;
        }

        items.Add(obj);
    }

    public static void AddAll(params object[] items)
    {
        foreach (var item in items)
        {
           Add(item); 
        }
    }

    public static List<T> GetObjectList<T>() where T : class
    {
        if (!_store.TryGetValue(typeof(T), out var list))
            return new List<T>();
        return list.OfType<T>().ToList();
    }
    
    public static void Clear(){
        _store.Clear();
    }

    public static void Save(string path)
    {
        var serializable = _store.Select(kvp => new SerializableTypeList_Save
        {
            TypeName = kvp.Key.AssemblyQualifiedName!,
            Items = kvp.Value
        }).ToList();

        var json = JsonSerializer.Serialize(serializable, new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        });

        File.WriteAllText(path, json);
    }

    public static void Load(string path)
    {
        var json = File.ReadAllText(path);
        var serializable = JsonSerializer.Deserialize<List<SerializableTypeList_Load>>(json);

        foreach (var entry in serializable)
        {
            var type = Type.GetType(entry.TypeName);
            if (type == null)
                continue;

            var list = new List<object>();
            foreach (var element in entry.Items.EnumerateArray())
            {
                var obj = JsonSerializer.Deserialize(element.GetRawText(), type);
                if (obj != null)
                    list.Add(obj);
            }

            _store[type] = list;
        }
    }

    private class SerializableTypeList_Save
    {
        public string TypeName { get; set; } = "";
        public List<object> Items { get; set; } = new List<object>();
    }

    private class SerializableTypeList_Load
    {
        public string TypeName { get; set; } = "";
        [JsonConverter(typeof(JsonElementConverter))] 
        public JsonElement Items { get; set; }
    }

    private class JsonElementConverter : JsonConverter<JsonElement>
    {
        public override JsonElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            return doc.RootElement.Clone();
        }

        public override void Write(Utf8JsonWriter writer, JsonElement value, JsonSerializerOptions options)
        {
            value.WriteTo(writer);
        }
    }
}