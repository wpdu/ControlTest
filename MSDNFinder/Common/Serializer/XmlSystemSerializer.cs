using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSFA.Common.Utility.Serializer
{
    public class XmlSystemSerializer
    {
        public static string Serialize(object value)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, value);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
