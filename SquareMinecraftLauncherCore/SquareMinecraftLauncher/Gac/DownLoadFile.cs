using System;
using System.Collections.Generic;
using System.Threading;

namespace Gac
{
    public class DownLoadFile
    {
        public int ThreadNum = 3;
        List<Thread> list = new List<Thread>();

        public DownLoadFile () => doSendMsg += Change;

        public delegate void dlgSendMsg(DownMsg msg);

        public event dlgSendMsg doSendMsg;

        public void AddDown(string DownUrl, string Dir, string FileName = "", int Id = 0)
        {
            var tsk = new Thread(() =>
            {
                download(DownUrl, Dir, FileName, Id);
            });

            list.Add(tsk);
        }

        public void StartDown(int StartNum)
        {
            if (StartNum == 0) StartNum = 3;

            for (int i2 = 0; i2 < StartNum; i2++)
            {
                lock (list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ThreadState == System.Threading.ThreadState.Unstarted || list[i].ThreadState == ThreadState.Suspended)
                        {
                            list[i].Start();
                            break;
                        }
                    }
                }
            }
        }

        void Change(DownMsg msg)
        {
            if (msg.Tag == DownStatus.Error || msg.Tag == DownStatus.End)
            {
                StartDown(1);
            }
        }

        // public event doSendMsg;
        // public dlgSendMsg doSendMsg = null;
        void download(string path, string dir, string filename, int id = 0)
        {
            try
            {
                var msg = new DownMsg();
                msg.Id = id;
                msg.Tag = 0;
                doSendMsg(msg);
                var loader = new FileDownloader(path, dir, filename, ThreadNum);
                loader.data.Clear();
                msg.Tag = DownStatus.Start;
                msg.Length = (int)loader.getFileSize();                 doSendMsg(msg);
                var linstenter = new DownloadProgressListener(msg);
                linstenter.doSendMsg = new DownloadProgressListener.dlgSendMsg(doSendMsg);
                loader.download(linstenter);
            }
            catch (Exception ex)
            {
                var msg = new DownMsg();
                msg.Id = id;
                msg.Length = 0;
                msg.Tag = DownStatus.Error;
                msg.ErrMessage = ex.Message;
                doSendMsg(msg);

                Console.WriteLine(ex.Message);
            }
        }
    }
} 