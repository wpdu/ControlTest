using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WinSFA.Common.Utility.external
{
    public class DataTableGenerator
    {
        /// <summary>
        /// 根据db.xml生成c#source code
        /// </summary>
        public static async void MakeCSharpSource()
        {
            var dbContent = await Common.Utility.StorageHelper.ReadApplicationFileContent("Resource\\data\\db.xml");

            // LocalDatabaseService source code
            StringBuilder dbManagerContent = new StringBuilder(
            @"using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.IO;
using WinSFA.Common.Logging;
using WinSFA.Model.DataTable;

namespace WinSFA.Service
{
    public class LocalDatabaseService
    {
        public static readonly string MainDb = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, Global.Constant.DbName);

        public static SQLiteConnection GetMainConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), MainDb);
        }

        public static void CreateTalbes()
        {
            using (var conn = GetMainConnection())
            {
                try
                {");

            var xml = XDocument.Parse(dbContent);
            var tables = xml.Descendants("table").ToList();
            foreach (var table in tables)
            {
                var tableName = table.Element("name").Value;
                var className = tableName.ToUpper();

                dbManagerContent.AppendLine("CreateTableBySql(conn, " + className + ".TableName, " + className + ".CreateSql);");

                //DataTable source code
                StringBuilder FileContent = new StringBuilder(
                    @"using SQLite.Net.Attributes;
                    using System;
                    using System.Collections.Generic;
                    using System.Linq;
                    using System.Text;
                    using System.Threading.Tasks;

                    namespace WinSFA.Model.DataTable
                    { [Table(""" + tableName + @""")] public class " + className + " { ");

                var ddl = table.Element("ddl").Value;
                FileContent.AppendLine(@"public static readonly string TableName = """ + tableName + @""";");
                FileContent.AppendLine(@"public static readonly string CreateSql = """ + ddl + @""";");

                var columnList = table.Descendants("column").ToList();
                foreach (var col in columnList)
                {
                    var colName = col.Element("name").Value;
                    var type = col.Element("type").Value;
                    var constraints = col.Element("constraints");
                    var constraintsText = constraints?.Value;
                    string cSharpType = string.Empty;

                    if (type.Equals("INTEGER"))
                    {
                        cSharpType = "int";
                    }
                    else if (type.Equals("TEXT"))
                    {
                        cSharpType = "string";
                    }
                    else if (type.Contains("NUMERIC"))
                    {
                        cSharpType = "double";
                    }
                    else
                    {
                        Debug.WriteLine("Unkonwn type......");
                    }
                    //PRIMARY KEY AUTOINCREMENT NOT NULL

                    if (constraintsText?.Contains("PRIMARY") == true)
                    {
                        FileContent.AppendLine("[PrimaryKey]");
                    }
                    if (constraintsText?.Contains("AUTOINCREMENT") == true)
                    {
                        FileContent.AppendLine("[AutoIncrement]");
                    }
                    if (constraintsText?.Contains("NOT NULL") == true)
                    {
                        FileContent.AppendLine("[NotNull]");
                    }

                    if (colName.Equals("_id") && constraintsText?.Contains("PRIMARY") == false && table.Value.Contains("PRIMARY") == false)
                    {
                        FileContent.AppendLine("[PrimaryKey]");
                    }


                    if (colName.Equals("readonly"))
                    {
                        FileContent.AppendLine(@"[Column(""readonly"")]");
                        colName = "_readonly";
                    }

                    FileContent.AppendLine("public " + cSharpType + " " + colName + "{ get; set; }");
                }
                FileContent.AppendLine("    }}");

                await Common.Utility.StorageHelper.WriteFileContent("class\\" + className + ".cs", FileContent.ToString());

                Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\class\\" + className + ".cs");
            }

            dbManagerContent.AppendLine(@"}
                catch (Exception ex)
            {
                Logger.Log(LogType.Exception, ""create tables error."", ex);
            }

            conn.Close();
        }
    }

    public static void CreateTableBySql(SQLiteConnection conn, string tableName, string sql)
    {
        try
        {
            if (conn.GetTableInfo(tableName).Count == 0)
            {
                var cmd = conn.CreateCommand(sql);
                var ret = cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LogType.Exception, sql, ex);
        }
    }
}
}");
            await Common.Utility.StorageHelper.WriteFileContent("LocalDatabaseService.cs", dbManagerContent.ToString());
            Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "LocalDatabaseService.cs");
        }
    }
}