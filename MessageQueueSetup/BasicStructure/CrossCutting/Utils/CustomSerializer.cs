using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CrossCutting.Utils
{
    public static class CustomSerializer
    {
        static void ThrowIfNotSerializable(Type objType)
        {
            var isSerializable = objType.GetCustomAttributes(typeof(SerializableAttribute), true).Any();
            if (!isSerializable)
                throw new InvalidOperationException("The object must to be a serializable for this operation (eg. be decorated with 'Serializable' attribute).");
        }

        public static byte[] ToByteArray<T>(this T obj)
        {
            ThrowIfNotSerializable(obj.GetType());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T ToObject<T>(this byte[] obj)
        {
            ThrowIfNotSerializable(typeof(T));

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(obj))
                return (T)bf.Deserialize(ms);
        }
    }
}
