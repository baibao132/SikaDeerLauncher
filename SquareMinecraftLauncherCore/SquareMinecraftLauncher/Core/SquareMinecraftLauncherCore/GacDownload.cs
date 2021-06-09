namespace SquareMinecraftLauncher.Core
{
    using Gac;
    using System;
    using System.IO;

    internal class GacDownload
    {
        internal static int id;
        internal int Complete, Failure;
        internal DownLoadFile dlf = new DownLoadFile();
        bool s;

        internal void Download(string path, string url)
        {
            if (!s)
            {
                dlf.doSendMsg += new DownLoadFile.dlgSendMsg(SendMsgHander);
                s = true;
            }
            dlf.AddDown(url, path.Replace(Path.GetFileName(path), ""), Path.GetFileName(path), id);
            dlf.StartDown(10);
        }

        void SendMsgHander(DownMsg msg)
        {
            DownStatus tag = msg.Tag;
            if (tag != DownStatus.End)
            {
                if (tag != DownStatus.Error) return;
            }
            else
            {
                Console.WriteLine(Complete);
                Complete++;
                return;
            }
            Failure++;
        }
    }
}