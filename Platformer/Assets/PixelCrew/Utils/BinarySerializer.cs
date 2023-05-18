using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PixelCrew.Utils
{
    public static class BinarySerializer
    {
        public static void Serialize(string path, object data)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }

        public static TType Deserialize<TType>(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                TType data = (TType)formatter.Deserialize(stream);
                return data;
            }
        }
    }
}