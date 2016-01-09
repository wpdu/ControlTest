using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSFA.Common.Serializer
{
    public class JsonNetSerializer
    {
        public static T Deserialize<T>(string json)
        {
            T t = JsonConvert.DeserializeObject<T>(json);

            return t;
        }

        public static Dictionary<T1, T2> DeserializeDictionary<T1, T2>(string json)
        {
            Dictionary<T1, T2> dic = JsonConvert.DeserializeObject<Dictionary<T1, T2>>(json);

            return dic;
        }

        public static List<T> DeserializeList<T>(string json)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json);

            return list;
        }

        public static string Serialize(object obj)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string jsonString = JsonConvert.SerializeObject(obj, Formatting.None, jsSettings);

            return jsonString;
        }
    }
}
