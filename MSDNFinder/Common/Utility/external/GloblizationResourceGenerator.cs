using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinSFA.Common.Utility.external
{
    /// <summary>
    /// 根据安卓平台string资源文件导出到UWP平台资源文件
    /// </summary>
    public class GloblizationResourceGenerator
    {
        public struct AdResource
        {
            public AdResource(string file,string lan)
            {
                FilePath = file;
                ResLanguage = lan;
            }
            public string FilePath { get; set; }
            public string ResLanguage { get; set; }
        }

        public static async void MakeResourceFile()
        {
            List<AdResource> adRes = new List<AdResource>();
            adRes.Add(new AdResource("Resource\\data\\values-en\\strings.xml", "en"));
            adRes.Add(new AdResource("Resource\\data\\values-zh-rCN\\strings.xml", "zh-cn"));

            string tempStrart = @"<data name=""";
            string tempMiddle = @""" xml:space=""preserve"">
                                <value>";
            string tempEnd = @"</value>
                                </data>";

            var templateStr = await Common.Utility.StorageHelper.ReadApplicationFileContent("Resource\\data\\R.resw");
            templateStr = templateStr.Replace("</root>", "");
            var targetStringBuilder = new StringBuilder(templateStr);

            foreach(var res in adRes)
            {
                var resContent = await Common.Utility.StorageHelper.ReadApplicationFileContent(res.FilePath);
                var xdoc = XDocument.Parse(resContent);
                var strNodes = xdoc.Elements().First().Elements();
                foreach (var node in strNodes)
                {
                    var key = node.Attribute("name").Value;
                    var value = node.Value;
                    targetStringBuilder.Append(tempStrart);
                    targetStringBuilder.Append(key);
                    targetStringBuilder.Append(tempMiddle);
                    value = value.Replace("&", "&amp;");
                    targetStringBuilder.Append(value);
                    targetStringBuilder.AppendLine(tempEnd);
                }
                targetStringBuilder.Append("</root>");

                await Common.Utility.StorageHelper.WriteFileContent("strings\\"+res.ResLanguage +"\\Resources.resw", targetStringBuilder.ToString());
            }

        }
    }
}
