using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSFA.Common.Utility.external
{
    public class ConfignetPropertyGenerator
    {
        /// <summary>
        /// 将Android的confignet.properties转换成resw文件
        /// </summary>
        public static async void MakeResourceFile()
        {
            var configStr = await Common.Utility.StorageHelper.ReadApplicationFileContent("Resource\\data\\confignet.properties");

            var templateStr = await Common.Utility.StorageHelper.ReadApplicationFileContent("Resource\\data\\R.resw");
            templateStr = templateStr.Replace("</root>", "");
            var targetStringBuilder = new StringBuilder(templateStr);

            string tempStrart = @"<data name=""";
            string tempMiddle = @""" xml:space=""preserve"">
                                <value>";
            string tempMiddle2 = @"</value><comment>";
            string tempEnd = @"</comment></data>";

            var configLineArray = configStr.Split('\n', '\r').ToArray().ToList();
            configLineArray.RemoveAll(c => string.IsNullOrWhiteSpace(c));
            string lastLineStr = string.Empty;
            foreach (var line in configLineArray)
            {
                var lrIndex = line.IndexOf('=');
                if(lrIndex != -1)
                {
                    var leftStr = line.Substring(0, lrIndex).Trim();
                    string rightStr = line.Substring(lrIndex + 1).Trim();
                    
                    targetStringBuilder.Append(tempStrart);
                    targetStringBuilder.Append(leftStr);
                    targetStringBuilder.Append(tempMiddle);
                    targetStringBuilder.Append(rightStr.Trim());
                    targetStringBuilder.Append(tempMiddle2);
                    if (lastLineStr.StartsWith("#"))
                    {
                        targetStringBuilder.Append(lastLineStr.Trim('#'));
                    }
                    targetStringBuilder.AppendLine(tempEnd);
                }
                lastLineStr = line;
            }
            targetStringBuilder.Append("</root>");

            await Common.Utility.StorageHelper.WriteFileContent("mapping\\ConfigMapping.resw", targetStringBuilder.ToString());

        }
    }
}
