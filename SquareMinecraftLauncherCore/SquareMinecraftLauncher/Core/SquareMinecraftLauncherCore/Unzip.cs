using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace SquareMinecraftLauncher
{
    internal class Unzip
    {
        /// <summary>
        /// 功能：解压zip格式的文件。
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <param name="err">出错信息</param>
        /// <returns>解压是否成功</returns>
        public bool UnZipFile(string zipFilePath, string unZipDir, out string err)
        {
            err = "";
            if (zipFilePath == string.Empty)
            {
                err = "压缩文件不能为空！";
                return false;
            }
            if (!File.Exists(zipFilePath))
            {
                err = "压缩文件不存在！";
                return false;
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("\\")) unZipDir += "\\";
            if (!System.IO.Directory.Exists(unZipDir))
                System.IO.Directory.CreateDirectory(unZipDir);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        var directoryName = Path.GetDirectoryName(theEntry.Name);
                        var fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            System.IO.Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("\\"))
                            directoryName += "\\";
                        if (fileName != string.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0) streamWriter.Write(data, 0, size);
                                    else break;
                                }
                            }
                        }
                    }//while
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }//解压结束
    }

    internal class Uzip
    {
        /// <summary>
        /// 压缩文件或文件夹   ----带密码
        /// </summary>
        /// <param name="fileToZip">要压缩的路径-文件夹或者文件</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <param name="password">密码</param>
        /// <param name="errorOut">如果失败返回失败信息</param>
        /// <returns>压缩结果</returns>
        public static bool Zip(string fileToZip, string zipedFile, string password, ref string errorOut)
        {
            bool result = false;
            try
            {
                if (System.IO.Directory.Exists(fileToZip))
                    result = ZipDirectory(fileToZip, zipedFile, password);
                else if (File.Exists(fileToZip))
                    result = ZipFile(fileToZip, zipedFile, password);
            }
            catch (Exception ex)
            {
                errorOut = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 压缩文件或文件夹 ----无密码
        /// </summary>
        /// <param name="fileToZip">要压缩的路径-文件夹或者文件</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <param name="errorOut">如果失败返回失败信息</param>
        /// <returns>压缩结果</returns>
        public static bool Zip(string fileToZip, string zipedFile, ref string errorOut)
        {
            bool result = false;
            try
            {
                if (System.IO.Directory.Exists(fileToZip))
                    result = ZipDirectory(fileToZip, zipedFile, null);
                else if (File.Exists(fileToZip))
                    result = ZipFile(fileToZip, zipedFile, null);
            }
            catch (Exception ex)
            {
                errorOut = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 压缩文件夹内部方法
        /// </summary>
        /// <param name="strFile"></param>
        /// <param name="s"></param>
        /// <param name="staticFile"></param>
        /// <returns></returns>
        static bool zip(string strFile, ZipOutputStream s, string staticFile)
        {
            bool result = true;
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
            var crc = new Crc32();
            try
            {
                string[] filenames = System.IO.Directory.GetFileSystemEntries(strFile);
                foreach (string file in filenames)
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        zip(file, s, staticFile);
                    }
                    else // 否则直接压缩文件
                    {
                        //打开压缩文件
                        var fs = File.OpenRead(file);

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        var tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                        var entry = new ZipEntry(tempfile);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;
                        fs.Close();
                        crc.Reset();
                        crc.Update(buffer);
                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="strFile">带压缩的文件夹目录</param>
        /// <param name="strZip">压缩后的文件名</param>
        /// <param name="password">压缩密码</param>
        /// <returns>是否压缩成功</returns>
        static bool ZipDirectory(string strFile, string strZip, string password)
        {
            bool result = false;
            if (!System.IO.Directory.Exists(strFile)) return false;
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            var s = new ZipOutputStream(File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            if (!string.IsNullOrEmpty(password)) s.Password = password;
            try
            {
                result = zip(strFile, s, strFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                s.Finish();
                s.Close();
            }
            return result;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件全名</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <param name="password">密码</param>
        /// <returns>压缩结果</returns>
        static bool ZipFile(string fileToZip, string zipedFile, string password)
        {
            bool result = true;
            ZipOutputStream zipStream = null;
            FileStream fs = null;
            ZipEntry ent = null;

            if (!File.Exists(fileToZip)) return false;

            try
            {
                fs = File.OpenRead(fileToZip);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                fs = File.Create(zipedFile);
                zipStream = new ZipOutputStream(fs);
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                ent = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(ent);
                zipStream.SetLevel(6);

                zipStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (ent != null) ent = null;
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            GC.Collect();
            GC.Collect(1);

            return result;
        }
    }
}