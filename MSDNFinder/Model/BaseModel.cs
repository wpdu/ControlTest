using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSFA.Common.Serializer;

namespace MSDNFinder.Model
{
    public class BaseModel : ObservableObject
    {
        public static T Create<T>(string json)
        {
            return JsonNetSerializer.Deserialize<T>(json);
        }

        public static Dictionary<T1, T2> CreateDictionary<T1, T2>(string json)
        {
            return JsonNetSerializer.DeserializeDictionary<T1, T2>(json);
        }

        public static List<T> CreateList<T>(string json)
        {
            return JsonNetSerializer.DeserializeList<T>(json);
        }

        public virtual string ToJson()
        {
            string jsonString = JsonNetSerializer.Serialize(this);

            return jsonString;
        }
    }
}
