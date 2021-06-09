namespace SquareMinecraftLauncher.Minecraft
{
    using global::SquareMinecraftLauncher.Core;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using windows;

    public sealed class Game
    {
        Process process = new Process();
        SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        ProcessStartInfo start = new ProcessStartInfo();
        Tools tools = new Tools();

        public delegate void ErrorDel(Game.Error error);

        public delegate void LogDel(Game.Log Log);

        [field: CompilerGenerated]
        public event ErrorDel ErrorEvent;

        [field: CompilerGenerated]
        public event LogDel LogEvent;

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        public async Task StartGame(string version, string java, int RAM, string name)
        {
            await StartGame(version, java, RAM, name, "", "");
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        public async Task StartGame(string version, string java, int RAM, string username, string password)
        {
            await StartGame(version, java, RAM, username, password, "", "");
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        public async Task StartGame(string version, string java, int RAM, string name, string JVMparameter, string RearParameter)
        {
            await StartGame(version, java, RAM, name, SLC.uuid(name), SLC.token(), JVMparameter, RearParameter);
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="username">邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        public async Task StartGame(string version, string java, int RAM, string username, string password, string JVMparameter, string RearParameter)
        {
            Getlogin getlogin = null;

            try
            {
                getlogin = tools.MinecraftLogin(username, password);
            }
            catch (SquareMinecraftLauncherException exception)
            {
                throw new SquareMinecraftLauncherException("启动失败，正版登录" + exception.Message);
            }

            await StartGame(version, java, RAM, getlogin.name, getlogin.uuid, getlogin.token, JVMparameter, RearParameter);
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="uuid">uuid</param>
        /// <param name="token">token</param>
        /// <param name="JVMparameter">前置参数</param>
        /// <param name="RearParameter">后置参数</param>
        public async Task StartGame(string version, string java, int RAM, string name, string uuid, string token, string JVMparameter, string RearParameter)
        {
            if (version == "" || version == null || java == "" || java == null || name == null || name == "" || uuid == "" || uuid == null || token == "" || token == null || RAM == 0)
            {
                throw new SquareMinecraftLauncherException("任何一项都不能为空");
            }

            string Game = null;
            var tools = new Tools();

            if (SLC.FileExist(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar") != null)
            {
                throw new SquareMinecraftLauncherException("启动失败,未找到该版本");
            }
            // download[] MLib = tools.GetMissingLibrary(version);
            // if (MLib.Length != 0)
            // {
            //    throw new SquareMinecraftLauncherException("缺少Libraries文件");
            // }
            MCDownload[] natives1 = new MCDownload[0];
            MCDownload[] natives = tools.GetMissingNatives(version);

            if (natives.Length == 0)
            {
                natives1 = tools.GetAllNatives(version);

                if (natives1.Length == 0)
                {
                    throw new SquareMinecraftLauncherException("Natives获取失败，请检查文件是否完整");
                }

                var nativespath = SLC.nativeszip(version);

                if (SLC.FileExist(java) != "")
                {
                    var bx = "-Dminecraft.client.jar=" + @"""" + System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar" + @"""" + " -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=16M -XX:-UseAdaptiveSizePolicy -XX:-OmitStackTraceInFastThrow -Xmn128m -Xmx" + RAM + "m -Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump ";

                    if (JVMparameter == "" || JVMparameter == null)
                    {
                        Game = bx + "-Djava.library.path=\"" + nativespath + "\" -cp ";
                    }
                    else
                    {
                        Game = bx + JVMparameter + " -Djava.library.path=\"" + nativespath + "\" -cp ";
                    }

                    if (tools.GetMissingLibrary(version).Length != 0)
                    {
                        throw new SquareMinecraftLauncherException("Libraries文件不完整");
                    }

                    MCDownload[] Lib = tools.GetAllLibrary(version);
                    var Libname = "\"";

                    foreach (var Libname1 in Lib)
                        Libname += Libname1.path + ";";
                    

                    Game += Libname + System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".jar\"";
                    var jo = SLC.versionjson<main.mainclass>(version);
                    string[] mA = null;

                    if (jo.minecraftArguments != null)
                    {
                        mA = jo.minecraftArguments.Split(' ');
                    }
                    else
                    {
                        StreamReader sr;

                        try
                        {
                            sr = new StreamReader(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", Encoding.Default);
                        }
                        catch (System.IO.DirectoryNotFoundException ex)
                        {
                            throw new SquareMinecraftLauncherException("未找到该版本");
                        }

                        var c = (JObject)JsonConvert.DeserializeObject(sr.ReadToEnd());
                        List<string> jo3 = new List<string>();

                        for (int i = 1; c["arguments"]["game"].ToArray().Length - 1 > 0; i += 2)
                        {
                            try
                            {
                                c["arguments"]["game"][i].ToString();

                                if (c["arguments"]["game"][i - 1].ToString()[0] == '-' || c["arguments"]["game"][i].ToString()[0] == '$')
                                {
                                    jo3.Add(c["arguments"]["game"][i - 1].ToString());
                                    jo3.Add(c["arguments"]["game"][i].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }

                        string[] arg = jo3.ToArray();
                        var a = arg[0] + " " + arg[1];

                        for (int i = 2; i < arg.Length; i += 2)
                            a += " " + arg[i] + " " + arg[i + 1];
                        

                        mA = a.Split(' ');
                    }

                    var jo2 = SLC.versionjson<json4.Root>(version);
                    var main = " --width 854 --height 480";

                    for (int i = 0; mA.Length > i; i += 2)
                    {
                        switch (mA[i])
                        {
                            case "--username":
                                main += " " + mA[i] + " \"" + name + "\"";
                                break;

                            case "--version":
                                main += " " + mA[i] + " \"" + jo2.id + "\"";
                                break;

                            case "--gameDir":
                                main += " " + mA[i] + " \"" + System.Directory.GetCurrentDirectory() + @"\.minecraft" + "\"";
                                break;

                            case "--assetsDir":
                                main += " " + mA[i] + " \"" + System.Directory.GetCurrentDirectory() + @"\.minecraft\assets" + "\"";
                                break;

                            case "--assetIndex":
                                main += " " + mA[i] + " " + jo2.assets;
                                break;

                            case "--uuid":
                                main += " " + mA[i] + " " + uuid;
                                break;

                            case "--accessToken":
                                main += " " + mA[i] + " " + token;
                                break;

                            case "--userType":
                                main += " " + mA[i] + " " + "Legacy";
                                break;

                            case "--versionType":
                                main += " " + mA[i] + " " + "\"SquareMinecraftLauncherCore\"";
                                break;

                            case "--userProperties":
                                main += " " + mA[i] + " " + "{}";
                                break;

                            default:
                                main += " " + mA[i] + " " + mA[i + 1];
                                break;
                        }
                    }

                    if ((RearParameter == "") || (RearParameter == null))
                    {
                        Game = Game + " " + jo.mainClass + main;
                    }
                    else
                    {
                        string[] textArray14 = new string[] { Game, " ", jo.mainClass, main, " ", RearParameter };
                        Game = string.Concat(textArray14);
                    }

                    Console.WriteLine("\n\n\n\n\n\n" + Game);
                    start.FileName = java;
                    start.Arguments = Game;
                    start.CreateNoWindow = true;
                    start.RedirectStandardOutput = true;
                    start.RedirectStandardInput = true;
                    start.UseShellExecute = false;
                    start.RedirectStandardError = true;

                    var thread1 = new Thread(new ThreadStart(monitoring))
                    {
                        IsBackground = true
                    };

                    var thread2 = new Thread(new ThreadStart(errormonitoring))
                    {
                        IsBackground = true
                    };

                    process = Process.Start(start);
                    thread2.Start();
                    thread1.Start();

                    try
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            if (process.WaitForInputIdle()) return;
                        });
                    }
                    catch (Exception ex)
                    {
                        throw new SquareMinecraftLauncherException("等待时发生异常：" + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="java">java</param>
        /// <param name="RAM">虚拟内存</param>
        /// <param name="name">游戏名</param>
        /// <param name="uuid">uuid</param>
        /// <param name="token">token</param>
        /// <param name="yggdrasilURLORID">ID</param>
        /// <param name="JVMparameter">JVM</param>
        /// <param name="RearParameter">后置参数</param>
        /// <param name="authentication">验证方式</param>
        public async Task StartGame(string version, string java, int RAM, string name, string uuid, string token, string yggdrasilURLORID, string JVMparameter, string RearParameter, AuthenticationServerMode authentication)
        {
            var GacDownload = new GacDownload();
            string str3 = null;
            var download = new Download();

            if (authentication == AuthenticationServerMode.yggdrasil)
            {
                if (SLC.FileExist(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\yggdrasilSquareMinecraftLauncher.jar") != null)
                {
                    SLC.SetFile("SquareMinecraftLauncher");
                    GacDownload.Download(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\yggdrasilSquareMinecraftLauncher.jar", "https://bmclapi2.bangbang93.com/mirrors/authlib-injector/artifact/26/authlib-injector-1.1.26-41a7a47.jar");

                    while (true)
                    {
                        if (GacDownload.Complete == 0 && GacDownload.Failure == 1)
                        {
                            throw new SquareMinecraftLauncherException("yggdrasil下载失败");
                        }
                        else if (GacDownload.Complete == 1) break;

                        ForgeInstallCore.Delay(2000);
                    }
                }

                var s = download.getHtml(yggdrasilURLORID);

                if (s == null)
                {
                    throw new SquareMinecraftLauncherException("启动失败，无法获取相关信息");
                }

                byte[] bytes = Encoding.Default.GetBytes(s);
                string[] textArray1 = new string[] { "-javaagent:", System.Directory.GetCurrentDirectory(), @"\SquareMinecraftLauncher\yggdrasilSquareMinecraftLauncher.jar=", yggdrasilURLORID, " -Dauthlibinjector.side=client -Dauthlibinjector.yggdrasil.prefetched=", Convert.ToBase64String(bytes) };
                str3 = string.Concat(textArray1);
            }
            else
            {
                if (SLC.FileExist(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\UnifiedPassSquareMinecraftLauncher.jar") != null)
                {
                    SLC.SetFile("SquareMinecraftLauncher");
                    GacDownload.Download(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\UnifiedPassSquareMinecraftLauncher.jar", "https://login2.nide8.com:233/download/nide8auth.jar");

                    while (true)
                    {
                        if (GacDownload.Complete == 0 && GacDownload.Failure == 1)
                        {
                            throw new SquareMinecraftLauncherException("统一通行证下载失败");
                        }
                        else if (GacDownload.Complete == 1) break;

                        ForgeInstallCore.Delay(2000);
                    }
                }

                str3 = "-javaagent:" + System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\UnifiedPassSquareMinecraftLauncher.jar=" + yggdrasilURLORID;
            }

            if ((JVMparameter != null) && (JVMparameter != ""))
            {
                str3 = str3 + "," + JVMparameter;
            }

            await StartGame(version, java, RAM, name, uuid, token, str3, RearParameter);
        }

        internal void errormonitoring()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            var standardError = process.StandardError;
            var sE = standardError.ReadToEnd();

            if (sE != "")
            {
                var error = new Error(null, sE);

                if (ErrorEvent != null) ErrorEvent(error);
                else standardError.Close();
            }

            try
            {
                process.WaitForExit();
                standardError.Close();
                process.Close();
            }
            catch (Exception)
            {
            }
        }

        internal void monitoring()
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                var standardOutput = process.StandardOutput;

                while (!standardOutput.EndOfStream)
                {
                    var message = standardOutput.ReadLine();

                    if (WinAPI.GetHandle("LWJGL") == null && WinAPI.GetHandle("GLFW30") == null)
                    {
                        standardOutput.Close();
                        process.Close();
                    }

                    if (LogEvent != null) LogEvent(new Log(message));
                    var str2 = SLC.Replace(message, "Exception", " ");

                    if ((str2 != null) && (str2 != message))
                    {
                        if (windows.WinAPI.GetHandle("LWJGL") == (IntPtr)0 && windows.WinAPI.GetHandle("GLFW30") == (IntPtr)0)
                        {
                            if (ErrorEvent != null)
                            {
                                ErrorEvent(new Error(null, standardOutput.ReadToEnd()));
                            }
                        }
                    }

                    if (ErrorEvent != null)
                    {
                        ErrorEvent(new Error(message, null));
                    }
                }

                process.WaitForExit();
                standardOutput.Close();
                process.Close();
            }
            catch (Exception)
            {
            }
        }
        public class Error : EventArgs
        {
            string message;
            string SE;
            SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();

            internal Error(string Message, string SE)
            {
                if ((Message == null) && (SE != null))
                {
                    this.SE = SE;
                }
                else
                {
                    var str = SLC.Replace(Message, "ERROR", " ");

                    if ((str != null) && (str != Message))
                    {
                        message = Message;
                    }
                }
            }

            public string Message => message;

            public string SeriousError => SE;
        }
        public class Log : EventArgs
        {
            string message;

            internal Log(string Message) => message = Message;

            public string Message => message;
        }
    }
} 