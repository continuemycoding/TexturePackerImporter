using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace TexturePackerImporter
{
    public class Utility
    {
        public static T DeserializeXml<T>(byte[] bytes)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));

            T value;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                TextReader textReader = new StreamReader(memoryStream);
                value = (T)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            return value;
        }

        public static T DeserializeXml<T>(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StreamReader(path);
            T value = (T)deserializer.Deserialize(textReader);
            textReader.Close();

            return value;
        }

        public static void SerializeXml<T>(string path, object o)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, o);
            textWriter.Close();
        }

        public static T DeserializeBinary<T>(byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}