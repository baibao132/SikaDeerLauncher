using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Gac
{
    public class DownloadThread
    {
        long block, downLength;
        FileDownloader downloader;
        string downUrl, saveFilePath;
        bool finish;
        int threadId = -1;
        public DownloadThread(FileDownloader downloader, string downUrl, string saveFile, long block, long downLength, int threadId)
        {
            this.downUrl = downUrl;
            saveFilePath = saveFile;
            this.block = block;
            this.downloader = downloader;
            this.threadId = threadId;
            this.downLength = downLength;
        }

        /// <summary>
        ///  已经下载的内容大小
        /// </summary>
        /// <returns>如果返回值为-1,代表下载失败</returns>
        public long getDownLength() => downLength;

        /// <summary>
        /// 下载是否完成
        /// </summary>
        /// <returns></returns>
        public bool isFinish() => finish;

        public void ThreadRun()
        {
            //task
            Thread td = new Thread(new ThreadStart(() =>
            {
                if (downLength < block)//未下载完成
                {
                    try
                    {
                        int startPos = (int)(block * (threadId - 1) + downLength);//开始位置
                        int endPos = (int)(block * threadId - 1);//结束位置
                        Console.WriteLine("Thread " + threadId + " start download from position " + startPos + "  and endwith " + endPos);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(downUrl);
                        request.Referer = downUrl.ToString();
                        request.Method = "GET";
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.1124)";
                        request.AllowAutoRedirect = false;
                        request.ContentType = "application/octet-stream";
                        request.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                        request.Timeout = 10 * 1000;
                        request.AllowAutoRedirect = true;
                        request.AddRange(startPos, endPos);
                        //Console.WriteLine(request.Headers.ToString()); //输出构建的http 表头
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        WebResponse wb = request.GetResponse();
                        using (Stream _stream = wb.GetResponseStream())
                        {
                            byte[] buffer = new byte[1024 * 50]; //缓冲区大小
                            long offset = -1;
                            using (Stream threadfile = new FileStream(saveFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) //设置文件以共享方式读写,否则会出现当前文件被另一个文件使用.
                            {
                                threadfile.Seek(startPos, SeekOrigin.Begin); //移动文件位置
                                while ((offset = _stream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    //offset 实际下载流大小
                                    downloader.append(offset); //更新已经下载当前总文件大小
                                    threadfile.Write(buffer, 0, (int)offset);
                                    downLength += offset;  //设置当前线程已下载位置
                                    downloader.update(threadId, downLength);
                                }
                                threadfile.Close(); //using 用完后可以自动释放..手动释放一遍.木有问题的(其实是多余的)
                                _stream.Close();
                                Console.WriteLine("Thread " + threadId + " download finish");
                                finish = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        downLength = -1;
                        Console.WriteLine("Thread " + threadId + ":" + e.Message);
                    }
                }
            }));
            td.IsBackground = true;
            td.Start();
        }
    }
}