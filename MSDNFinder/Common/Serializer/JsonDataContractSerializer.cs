using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WinSFA.Common.Serializer
{
    public static class JsonDataContractSerializer
    {
        public static string Serialize<T>(T value, bool throwException = false) where T : class
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(ms, value);
                    var array = ms.ToArray();
                    return Encoding.UTF8.GetString(array, 0, array.Length);
                }
            }
            catch
            {
                if (!throwException)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static string Serialize(object value, bool throwException = false)
        {
            if (value == null)
            {
                return string.Empty;
            }

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(value.GetType());
                    serializer.WriteObject(ms, value);
                    var array = ms.ToArray();
                    return Encoding.UTF8.GetString(array, 0, array.Length);
                }
            }
            catch
            {
                if (!throwException)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static T Deserialize<T>(string json, bool throwException = false) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(json)) return default(T);

                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    T value = (T)serializer.ReadObject(ms);
                    return value;
                }
            }
            catch
            {
                if (!throwException)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}