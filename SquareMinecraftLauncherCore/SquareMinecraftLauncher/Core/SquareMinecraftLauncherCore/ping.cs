namespace SquareMinecraftLauncher.Core
{
    using System;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;

    public sealed class ping
    {
        const int INTERNET_CONNECTION_LAN = 2, INTERNET_CONNECTION_MODEM = 1;

        public static bool CheckServeStatus()
        {
            if (!LocalConnectionStatus())
            {
                Console.WriteLine("网络异常~无连接");
                return false;
            }
            Console.WriteLine("网络正常");
            return true;
        }

        public static bool MyPing(string[] urls, out int errorCount)
        {
            bool flag = true;
            var ping = new Ping();
            errorCount = 0;
            try
            {
                for (int i = 0; i < urls.Length; i++)
                {
                    var reply = ping.Send(urls[i]);
                    if (reply.Status != IPStatus.Success)
                    {
                        flag = false;
                        errorCount++;
                    }
                    Console.WriteLine("Ping " + urls[i] + "    " + reply.Status.ToString());
                }
            }
            catch
            {
                flag = false;
                errorCount = urls.Length;
            }
            return flag;
        }

        [DllImport("winInet.dll")]
        static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);

        static bool LocalConnectionStatus()
        {
            int dwFlag = 0;
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                Console.WriteLine("LocalConnectionStatus--未连网!");
                return false;
            }
            if ((dwFlag & 1) != 0)
            {
                Console.WriteLine("LocalConnectionStatus--采用调制解调器上网。");
                return true;
            }
            if ((dwFlag & 2) != 0)
            {
                Console.WriteLine("LocalConnectionStatus--采用网卡上网。");
                return true;
            }
            return false;
        }
    }
}