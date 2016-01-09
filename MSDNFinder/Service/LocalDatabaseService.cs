using MSDNFinder.Model.DataJson;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections;
using System.IO;
using Windows.Storage;
using WinSFA.Common.Logging;
using WinSFA.Common.Utility;

namespace Service
{
    public class LocalDatabaseService
    {
        public static readonly string MainDb = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, Global.Config.DbName);
        public static readonly string MSDNDb = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, Global.Config.DbName);

        public async static void CopyAppDbToLocal()
        {
            if (!await StorageHelper.CheckFileExist(Global.Config.DbName))
            {
                StorageFile dbFile = await StorageHelper.GetAppPackageFile(Global.Config.DbName);
                if (dbFile != null)
                {
                    StorageFile localDbFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(Global.Config.DbName);
                    await dbFile.CopyAndReplaceAsync(localDbFile);
                }
                //var buffer = await StorageHelper.ReadApplicationBuffer(Global.Config.DbName);
                //if (buffer != null)
                //{
                //    bool result = await StorageHelper.WriteFileBuffer(Global.Config.DbName, buffer);
                //}
            }
        }

        public static SQLiteConnection GetMainConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), MainDb);
        }

        public static void CreateTalbes()
        {
            using (var conn = GetMainConnection())
            {
                try
                {
                    CreateTableBySql(conn, ServerModel.TableName, ServerModel.CreateSql);
                    CreateTableBySql(conn, ExtendedAttributes.TableName, ExtendedAttributes.CreateSql);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogType.Exception, "create tables error.", ex);
                }

                conn.Close();
            }
        }

        private static void CreateTableBySql(SQLiteConnection conn, object tableName, object createSql)
        {
            throw new NotImplementedException();
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

        public static bool ExcuteSql(SQLiteConnection conn, string sql)
        {
            bool ret = false;
            try
            {
                var cmd = conn.CreateCommand(sql);
                var result = cmd.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Exception, sql, ex);
            }

            return ret;
        }

        public static bool InsertAll(IEnumerable obj, string tableName)
        {
            bool ret = false;
            try
            {
                using (var conn = GetMainConnection())
                {
                    conn.InsertAll(obj);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Exception, "InsertAll db error:" + tableName, ex);
            }

            return ret;
        }

        public static bool InsertOrReplaceAll(IEnumerable obj, string tableName = "")
        {
            bool ret = false;
            try
            {
                using (var conn = GetMainConnection())
                {
                    conn.InsertOrReplaceAll(obj);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Exception, "InsertOrReplaceAll db error:" + tableName, ex);
            }

            return ret;
        }

        public static bool InsertAllOneByOne(IEnumerable obj, string tableName = "")
        {
            bool ret = false;

            using (var conn = GetMainConnection())
            {
                foreach (var one in obj)
                {
                    try
                    {
                        conn.Insert(one);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(LogType.Exception, "InsertAllOneByOne db error:" + tableName, ex);
                    }
                }
            }

            return ret;
        }

        public static bool DeleteAll<T>(string tableName = "")
        {
            bool ret = false;

            try
            {
                using (var conn = GetMainConnection())
                {
                    conn.DeleteAll(typeof(T));
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Exception, "DeleteAll db error:" + tableName, ex);
            }

            return ret;
        }

        public static bool ReplaceAll<T>(IEnumerable obj, string tableName = "")
        {
            bool ret = false;

            try
            {
                using (var conn = GetMainConnection())
                {
                    conn.DeleteAll(typeof(T));
                    conn.InsertAll(obj);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Exception, "ReplaceAll db error:" + tableName, ex);
            }

            return ret;
        }
    }
}