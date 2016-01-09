using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WinSFA.Common.Utility
{
    public class ZipUtil
    {
        #region GZip

        /// <summary>
        /// GZipStream压缩
        /// </summary>
        /// <param name="str"></param>
        public static byte[] Compress(byte[] inputBytes)
        {
            byte[] zipBytes = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    zipStream.Write(inputBytes, 0, inputBytes.Length);
                }
                zipBytes = outStream.ToArray();
            }

            return zipBytes;
        }

        /// <summary>
        /// GZipStream解压
        /// </summary>
        /// <param name="str"></param>
        public static byte[] Decompress(byte[] inputBytes)
        {
            byte[] unZipBytes = null;
            using (MemoryStream inputStream = new MemoryStream(inputBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(outStream);
                    }
                    unZipBytes = outStream.ToArray();
                }
            }

            return unZipBytes;
        }

        #endregion GZip

        #region ZipArchive

        public static async Task<StorageFolder> UnZipFile(string folderName, byte[] data)
        {
            StorageFolder unZipfolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            var stream = new MemoryStream(data);
            var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
            foreach (var zipArchiveEntry in zipArchive.Entries)
            {
                if (!String.IsNullOrEmpty(zipArchiveEntry.FullName))
                {
                    if (!zipArchiveEntry.FullName.EndsWith("/"))
                    {
                        string fileName = zipArchiveEntry.FullName.Replace("/", "\\");
                        using (Stream fileData = zipArchiveEntry.Open())
                        {
                            StorageFile newFile = await unZipfolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                            using (IRandomAccessStream newFileStream = await newFile.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                using (Stream s = newFileStream.AsStreamForWrite())
                                {
                                    await fileData.CopyToAsync(s);
                                    await s.FlushAsync();
                                    s.Dispose();
                                }
                                newFileStream.Dispose();
                            }
                        }
                    }
                }
            }
            return unZipfolder;
        }

        public static async Task<byte[]> ZipFolder(StorageFolder folder)
        {
            using (var zipMemoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create))
                {
                    await AddZipFolderToEntry(zipArchive, folder, "");
                }
                return zipMemoryStream.ToArray();
            }
        }

        private static async Task<bool> AddZipFolderToEntry(ZipArchive zipArchive, StorageFolder folder, string entryFirst)
        {
            IReadOnlyList<StorageFile> filesToCompress = await folder.GetFilesAsync();

            foreach (StorageFile fileToCompress in filesToCompress)
            {
                byte[] buffer = (await FileIO.ReadBufferAsync(fileToCompress)).ToArray();
                ZipArchiveEntry entry = zipArchive.CreateEntry(entryFirst + fileToCompress.Name);
                using (Stream entryStream = entry.Open())
                {
                    await entryStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            var childrenFolder = await folder.GetFoldersAsync();
            foreach (var storageFolder in childrenFolder)
            {
                await AddZipFolderToEntry(zipArchive, storageFolder, entryFirst + storageFolder.Name + "/");
            }

            return true;
        }

        #endregion ZipArchive

        #region InflaterStream

        public static byte[] Inflate(byte[] bytes)
        {
            byte[] result = null;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                //using (InflaterInputStream inf = new InflaterInputStream(memoryStream))
                //{
                //    using (MemoryStream outStream = new MemoryStream())
                //    {
                //        var buffer = new byte[1024];
                //        int resLen;
                //        while ((resLen = inf.Read(buffer, 0, buffer.Length)) > 0)
                //        {
                //            outStream.Write(buffer, 0, resLen);
                //        }
                //        result = outStream.ToArray();
                //    }
                //}
            }
            return result;
        }

        #endregion InflaterStream
    }
}