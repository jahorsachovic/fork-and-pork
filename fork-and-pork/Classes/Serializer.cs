using Newtonsoft.Json;
using System.Net.Mail;

namespace fork_and_pork.Classes;

public static class ObjectStore
{
    private static Dictionary<Type, List<object>> _store = new Dictionary<Type, List<object>>();

    private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,

        PreserveReferencesHandling = PreserveReferencesHandling.Objects,

        TypeNameHandling = TypeNameHandling.Auto,

        Converters = { new MailAddressConverter() }
    };

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

    public static void Clear()
    {
        _store.Clear();
    }

    public static void Save(string path)
    {
        var serializable = _store.Select(kvp => new SerializableTypeList
        {
            TypeName = kvp.Key.AssemblyQualifiedName!,
            Items = kvp.Value
        }).ToList();

        var json = JsonConvert.SerializeObject(serializable, _jsonSettings);
        File.WriteAllText(path, json);
    }

    public static void Load(string path)
    {
        var json = File.ReadAllText(path);

        var serializable = JsonConvert.DeserializeObject<List<SerializableTypeList>>(json, _jsonSettings);

        Clear();
        if (serializable == null) return;

        foreach (var entry in serializable)
        {
            if (entry.Items.Any())
            {
                Type type = entry.Items[0].GetType();
                _store[type] = entry.Items;
            }
        }
    }

    private class SerializableTypeList
    {
        public string TypeName { get; set; } = "";

        public List<object> Items { get; set; } = new List<object>();
    }
}

public class MailAddressConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(MailAddress);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var mailAddress = (MailAddress)value;
        writer.WriteValue(mailAddress.Address);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var address = (string)reader.Value;
        return string.IsNullOrEmpty(address) ? null : new MailAddress(address);
    }
}