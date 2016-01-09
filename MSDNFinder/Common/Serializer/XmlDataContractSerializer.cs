using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace WinSFA.Common.Serializer
{
    /// <summary>
    /// A utility for serializing and deserializing objects using Generic typed parameters
    /// </summary>
    public static class XmlDataContractSerializer
    {
        /// <summary>
        /// Serializes the object into an XML string
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize</typeparam>
        /// <param name="source">The object to serialize</param>
        /// <returns>The serialized form of the object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Serialize<T>(T source)
        {
            string serializedString = string.Empty;
            var serializer = new DataContractSerializer(source.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NamespaceHandling = NamespaceHandling.Default,
                NewLineOnAttributes = false,
            };

            try
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.WriteObject(xmlWriter, source);
                    }

                    serializedString = stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                var message = "Cannot serialize object of type " + typeof(T);
                Debug.WriteLine(message + "," + ex.Message);
            }

            return serializedString;
        }

        /// <summary>
        /// Transforms a serialized object into a instance of the object
        /// </summary>
        /// <typeparam name="T">The type of the object to create</typeparam>
        /// <param name="rawXml">The serialized form of the object</param>
        /// <returns>An instance of the object</returns>
        public static T Deserialize<T>(string rawXml)
        {
            T deserializedObject = default(T);
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(rawXml)))
                {
                    DataContractSerializer formatter = new DataContractSerializer(typeof(T));
                    deserializedObject = (T)formatter.ReadObject(reader);
                }
            }
            catch (Exception ex)
            {
                var message = "Cannot deserialize xml into object of type " + typeof(T);
                Debug.WriteLine(message + "," + ex.Message);
            }

            return deserializedObject;
        }
    }
}