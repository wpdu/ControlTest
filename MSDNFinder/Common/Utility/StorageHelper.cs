using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace WinSFA.Common.Utility
{
    public class StorageHelper
    {
        #region Settings Get & Set

        public static object GetSetting(string key)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
            return settings.ContainsKey(key) ? settings[key] : null;
        }

        public static bool CheckKey(string key)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
            return settings.ContainsKey(key) ? true : false;
        }

        public static T GetSetting<T>(string key, T defaultvalue)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
            return settings.ContainsKey(key) ? ((T)settings[key]) == null ? defaultvalue : (T)settings[key] : defaultvalue;
        }

        public static void SaveSetting(string key, object value)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
            if (settings.ContainsKey(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
        }

        public static void RemoveSetting(string key)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
            settings.Remove(key);
        }

        #endregion Settings Get & Set

        #region Local Storage Read & Write

        public static async Task<ulong> GetFolderSize(StorageFolder folder)
        {
            ulong size = 0;

            try
            {
                foreach (StorageFolder thisFolder in await folder.GetFoldersAsync())
                {
                    size += await GetFolderSize(thisFolder);
                }

                foreach (StorageFile thisFile in await folder.GetFilesAsync())
                {
                    BasicProperties props = await thisFile.GetBasicPropertiesAsync();
                    size += props.Size;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return size;
        }

        public static async Task<string> ReadFileContent(string path)
        {
            try
            {
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile file = await local.GetFileAsync(path);

                if (file != null)
                {
                    var content = await FileIO.ReadTextAsync(file);

                    return content;
                }
            }
            catch (Exception ex)
            {
                //TODO
            }

            return null;
        }

        public static async Task<string> ReadFileContent(string folderPath, string fileName)
        {
            var fullPath = String.Concat(folderPath, "\\", fileName);
            var content = await ReadFileContent(fullPath);

            return content;
        }

        public static async Task<bool> WriteFileContent(string fullPath, string content)
        {
            bool result = false;
            try
            {
                var local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await local.CreateFileAsync(fullPath, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, content);
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WriteFileContent Exception:" + ex.Message);
            }

            return result;
        }

        public static async Task<bool> WriteFileContent(string folderPath, string fileName, string content)
        {
            var fullPath = String.Concat(folderPath, "\\", fileName);
            return await WriteFileContent(fullPath, content);
        }

        public static async Task<bool> AppendFileContent(string fullPath, string content)
        {
            bool result = false;
            try
            {
                var local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await local.CreateFileAsync(fullPath, CreationCollisionOption.OpenIfExists);

                await FileIO.AppendTextAsync(file, content);
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AppendFileContent Exception:" + ex.Message);
                result = false;
            }

            return result;
        }

        public static async Task<bool> WriteFileBuffer(string fullPath, IBuffer buffer)
        {
            bool result = false;
            try
            {
                var local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await local.CreateFileAsync(fullPath, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBufferAsync(file, buffer);
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WriteFileContent Exception:" + ex.Message);
            }
            return result;
        }

        public static async Task<bool> DeleteFile(string fullPath)
        {
            bool result = false;
            try
            {
                var local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await local.GetFileAsync(fullPath);
                if (file != null)
                {
                    await file.DeleteAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DeleteFile Exception:" + ex.Message);
            }

            return result;
        }

        public static async Task<bool> DeleteFolder(string fullPath)
        {
            bool result = false;
            try
            {
                var local = Windows.Storage.ApplicationData.Current.LocalFolder;
                var folder = await local.GetFolderAsync(fullPath);
                await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DeleteFolder Exception:" + ex.Message);
            }
            return result;
        }

        public static async Task<bool> CheckFileExist(string fullPath)
        {
            bool result = false;
            try
            {
                var local = Windows.Storage.ApplicationData.Current.LocalFolder;
                await local.GetFileAsync(fullPath);
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FileNotExist:" + fullPath);
            }
            return result;
        }

        #endregion Local Storage Read & Write

        #region Package File Read

        public static async Task<string> ReadApplicationFileContent(string fullPath)
        {
            try
            {
                //Windows.ApplicationModel.Package.Current.InstalledLocation also works.
                Uri uri = new Uri("ms-appx:///" + fullPath, UriKind.RelativeOrAbsolute);
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                if (file != null)
                {
                    var content = await FileIO.ReadTextAsync(file);
                    return content;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ReadApplicationFileContent Exception:" + fullPath + ex.Message);
            }

            return null;
        }

        public static async Task<IBuffer> ReadApplicationBuffer(string fullPath)
        {
            try
            {
                //Windows.ApplicationModel.Package.Current.InstalledLocation also works.
                Uri uri = new Uri("ms-appx:///" + fullPath, UriKind.RelativeOrAbsolute);
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                if (file != null)
                {
                    var content = await FileIO.ReadBufferAsync(file);
                    
                    return content;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ReadApplicationFileContent Exception:" + fullPath + ex.Message);
            }
            return null;
        }

        public static async Task<StorageFile> GetAppPackageFile(string fullPath)
        {
            try
            {
                Uri uri = new Uri("ms-appx:///" + fullPath, UriKind.RelativeOrAbsolute);
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                return file;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("GetAppPackageFile Err" + fullPath + ex.Message);
            }
            return null;
        }


        #endregion Package File Read
    }

    /// <summary>
    /// Useless class for universal8.1
    /// </summary>
    public static class StorageFolderExtenssion
    {
        public async static Task<StorageFile> GetFileByPath(this StorageFolder _folder, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            try
            {
                var folderArray = path.Split(new char[] { '/', '\\' });
                StorageFolder _InnerFolder = _folder;
                for (var i = 0; i < folderArray.Length - 1; i++)
                {
                    if (String.IsNullOrEmpty(folderArray[i]))
                        continue;
                    _InnerFolder = await _InnerFolder.GetSubFolder(folderArray[i]);

                    if (_InnerFolder == null)
                        return null;
                }

                return await _InnerFolder.GetFileAsync(folderArray[folderArray.Length - 1]);
            }
            catch
            {
                return null;
            }
        }

        public async static Task<StorageFolder> GetFolderByPath(this StorageFolder _folder, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            try
            {
                var folderArray = path.Split(new char[] { '/', '\\' });
                StorageFolder _InnerFolder = _folder;
                for (var i = 0; i < folderArray.Length; i++)
                {
                    if (String.IsNullOrEmpty(folderArray[i]))
                        continue;

                    _InnerFolder = await _InnerFolder.GetSubFolder(folderArray[i]);

                    if (_InnerFolder == null)
                        return null;
                }

                return _InnerFolder;
            }
            catch
            {
                return null;
            }
        }

        public async static Task<StorageFolder> GetSubFolder(this StorageFolder _folder, string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
                return null;
            try
            {
                var folders = await _folder.GetFoldersAsync();
                foreach (var f in folders)
                {
                    if (f.Name == folderName)
                    {
                        return f;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}