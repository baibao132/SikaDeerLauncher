using System;

namespace Gac
{
    public enum DownStatus
    {
        Start,
        GetLength,
        DownLoad,
        End,
        Error
    }

    public class DownloadProgressListener : IDownloadProgressListener
    {
        public dlgSendMsg doSendMsg;
        DownMsg downMsg;

        public DownloadProgressListener(DownMsg downmsg)
        {
            downMsg = downmsg;
            //this.id = id;
            //this.Length = Length;
        }

        public delegate void dlgSendMsg(DownMsg msg);
        public void OnDownloadSize(long size)
        {
            if (downMsg == null)
            {
                var downMsg = new DownMsg();
            }

            //下载速度
            if (downMsg.Size == 0)
            {
                downMsg.Speed = size;
            }
            else
            {
                downMsg.Speed = (float)(size - downMsg.Size);
            }
            if (downMsg.Speed == 0)
            {
                downMsg.Surplus = -1;
                downMsg.SurplusInfo = "未知";
            }
            else
            {
                downMsg.Surplus = ((downMsg.Length - downMsg.Size) / downMsg.Speed);
            }
            downMsg.Size = size; //下载总量

            if (size == downMsg.Length)
            {
                //下载完成
                downMsg.Tag = DownStatus.End;
                downMsg.SpeedInfo = "0 K";
                downMsg.SurplusInfo = "已完成";
            }
            else
            {
                //下载中
                downMsg.Tag = DownStatus.DownLoad;
            }

            if (doSendMsg != null) doSendMsg(downMsg);//通知具体调用者下载进度
        }
    }
    public class DownMsg
    {

        int _Length;

        long _Size = 0;

        float _Speed = 0, _Surplus = 0;

                public string ErrMessage { get;  set; }= "";
        public int Id { get;  set; }
        public int Length
        {
            get
            {
                return _Length;
            }

            set
            {
                _Length = value;
                LengthInfo = GetFileSize(value);
            }
        }
                public string LengthInfo { get;  set; }= "";        
        public double Progress { get;  set; }= 0;
        public long Size
        {
            get
            {
                return _Size;
            }

            set
            {
                _Size = value;
                SizeInfo = GetFileSize(value);
                if (Length >= value)
                {
                    Progress = Math.Round((double)value / Length * 100, 2);
                }
                else
                {
                    Progress = -1;
                }
            }
        }
        
        public string SizeInfo { get;  set; }= "";
        public float Speed
        {
            get
            {
                return _Speed;
            }

            set
            {
                _Speed = value;
                SpeedInfo = GetFileSize(value);
            }
        }
        
        public string SpeedInfo { get;  set; }= "";
        public float Surplus
        {
            get
            {
                return _Surplus;
            }

            set
            {
                _Surplus = value;
                if (value > 0)
                {
                    SurplusInfo = GetDateName((int)Math.Round(value, 0));
                }
            }
        }
        
        public string SurplusInfo { get;  set; }= "";        
        public DownStatus Tag { get;  set; }= 0;        string GetDateName(int Second)
        {
            float temp = Second;
            var suf = "秒";
            if (Second > 60)
            {
                suf = "分钟";
                temp = temp / 60;
                if (Second > 60)
                {
                    suf = "小时";
                    temp = temp / 60;
                    if (Second > 24)
                    {
                        suf = "天";
                        temp = temp / 24;
                        if (Second > 30)
                        {
                            suf = "月";
                            temp = temp / 30;
                            if (Second > 12)
                            {
                                suf = "年";
                                temp = temp / 12;
                            }
                        }
                    }
                }
            }

            return string.Format("{0:0} {1}", temp, suf);
        }

        string GetFileSize(float Len)
        {
            float temp = Len;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (temp >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                temp = temp / 1024;
            }
            return string.Format("{0:0.##} {1}", temp, sizes[order]);
        }
    }
}