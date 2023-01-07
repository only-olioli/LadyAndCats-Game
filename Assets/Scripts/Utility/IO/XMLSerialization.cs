using System.IO;
using System.Xml.Serialization;

namespace Utility.IO
{
    public static class XMLSerialization
    {
        public static void Write<T>(T obj, string path)
        {
            using var stream = new FileStream(path, FileMode.Create);
            var XML = new XmlSerializer(typeof(T));
            XML.Serialize(stream, obj);
        }

        public static T Read<T>(string path)
        {
            if (File.Exists(path))
            {
                using var stream = new FileStream(path, FileMode.Open);
                var XML = new XmlSerializer(typeof(T));
                return (T)XML.Deserialize(stream);
            }
            return default;
        }
    }
}
