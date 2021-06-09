using AI;
using BlessingSkinJson;
using global::SquareMinecraftLauncher.Core;
using json4;
using mcbbs;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SquareMinecraftLauncher.Core.fabricmc;
using SquareMinecraftLauncher.Core.Forge;
using SquareMinecraftLauncher.SquareMinecraftLauncher.Core.Java;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using windows;
using File = System.IO.File;

namespace SquareMinecraftLauncher.Minecraft
{
    public sealed class Tools
    {
        internal static string DSI = "https://bmclapi2.bangbang93.com/libraries/";
        internal static List<mc> mcV = new List<mc>();
        SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        bool vp;
        Download web = new Download();
        YESAPI YESAPI = new YESAPI();
        /// <summary>
        /// 取Java路径(新)
        /// </summary>
        /// <returns>列表(Path=路径,Version=Java版本)</returns>
        public static List<JavaVersion> GetJavaInstallationPath()
        {
            List<JavaVersion> vs = new List<JavaVersion>();

            try
            {
                var environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");

                if (!string.IsNullOrEmpty(environmentPath) && File.Exists(environmentPath + @"\bin\javaw.exe"))
                {
                    vs.Add(new JavaVersion() { Path = environmentPath, Version = GetProductVersion(environmentPath + @"\bin\javaw.exe") });
                }

                using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\Java Runtime Environment\"))
                {
                    var currentVersion = rk.GetValue("CurrentVersion").ToString();

                    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                    {
                        var path = key.GetValue("JavaHome").ToString();

                        if (File.Exists(path + @"\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { Path = path, Version = GetProductVersion(path + @"\bin\javaw.exe") });
                        }
                    }
                }

                using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\JDK\"))
                {
                    var currentVersion = rk.GetValue("CurrentVersion").ToString();

                    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                    {
                        var path = key.GetValue("JavaHome").ToString();

                        if (File.Exists(path + @"\bin\javaw.exe"))
                        {
                            vs.Add(new JavaVersion() { Path = path, Version = GetProductVersion(path + @"\bin\javaw.exe") });
                        }
                    }
                }

                return vs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        /// <summary>
        /// 修改游戏标题
        /// </summary>
        /// <param name="Text">标题</param>
        /// <returns></returns>
        public bool ChangeTheTitle(string Text)
        {
            WinAPI.SetWindowText(WinAPI.GetHandle("LWJGL"), Text + " SquareMinecraftLauncher");
            return true;
        }

        /// <summary>
        /// 下载源初始化
        /// </summary>
        /// <param name="downloadSource">下载源</param>
        public void DownloadSourceInitialization(DownloadSource downloadSource)
        {
            YESAPI.Tts();

            if (downloadSource == DownloadSource.MinecraftSource)
            {
                DSI = "Minecraft";
            }
            else if (downloadSource == DownloadSource.bmclapiSource)
            {
                DSI = null;
            }
            else
            {
                DSI = "https://download.mcbbs.net/libraries/";
            }
        }

        /// <summary>
        /// 是否存在Fabric
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool FabricExist(string version)
        {
            var libraries = JsonConvert.DeserializeObject<ForgeY.Root>(SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json"));

            foreach (var i in libraries.libraries)
            {
                if (i.name.Split(':')[0] == "net.fabricmc")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// forge是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool ForgeExist(string version) => (GetLocalForgeVersion(version) != null);

        public bool ForgeExist(string version, ref string ForgeVersion)
        {
            ForgeVersion = GetLocalForgeVersion(version);

            if (ForgeVersion != null) return true;
            else return false;
        }

        /// <summary>
        /// forge安装
        /// </summary>
        /// <param name="ForgePath">forge路径</param>
        /// <param name="version">版本</param>
        /// <param name="java">java路径</param>
        /// <returns></returns>
        public async Task<bool> ForgeInstallation(string ForgePath, string version, string java)
        {
            string str;

            if ((SLC.FileExist(ForgePath) == null) && new Unzip().UnZipFile(ForgePath, @"SquareMinecraftLauncher\Forge\", out str))
            {
                string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".jar" };

                if (SLC.FileExist(string.Concat(textArray1)) == null)
                {
                    var core = new ForgeCore();
                    SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", core.ForgeJson(version, @"SquareMinecraftLauncher\Forge\version.json"));

                    if (SLC.FileExist(@"SquareMinecraftLauncher\Forge\install_profile.json") == null)
                    {
                        await new ForgeInstallCore().ForgeInstall(@"SquareMinecraftLauncher\Forge\install_profile.json", version, java);
                        char[] separator = new char[] { '\\' };
                        string[] strArray = ((JObject)JsonConvert.DeserializeObject(SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\Forge\install_profile.json")))["path"].ToString().Replace(':', '\\').Split(separator);
                        string[] textArray3 = new string[] { strArray[0].Replace('.', '\\'), @"\", strArray[1], @"\", strArray[2] };
                        var str2 = string.Concat(textArray3);
                        var path = System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2;
                        SLC.path(path);

                        foreach (string str4 in System.IO.Directory.GetFiles(@"SquareMinecraftLauncher\Forge\maven\" + str2))
                        {
                            try
                            {
                                File.Copy(str4, path + @"\" + Path.GetFileName(str4));
                            }
                            catch
                            {
                            }
                        }
                    }

                    SLC.DelPathOrFile(@"SquareMinecraftLauncher\Forge\");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取Forge版本列表
        /// </summary>
        /// <returns></returns>
        public string[] ForgeVersionList()
        {
            var str = web.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft");

            if (str == null)
            {
                throw new SquareMinecraftLauncherException("请求失败");
            }

            return JsonConvert.DeserializeObject<List<string>>(str).ToArray();
        }

        /// <summary>
        /// 取所有依赖库包括（Natives）
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public MCDownload[] GetAllFile(string version)
        {
            MCDownload[] downloadArray;
            var download = new MinecraftDownload();
            YESAPI.Tts();

            try
            {
                var root = SLC.versionjson<json4.Root>(version);
                List<MCDownload> list = new List<MCDownload>();
                var dSI = DSI;

                if (DSI == "Minecraft")
                {
                    dSI = "https://libraries.minecraft.net/";
                }
                else if (DSI == null)
                {
                    dSI = "https://bmclapi2.bangbang93.com/libraries/";
                }

                foreach (LibrariesItem item in root.libraries)
                {
                    string str2 = null;

                    if (item.natives != null)
                    {
                        if (item.natives.windows == null) continue;
                        str2 = SLC.libAnalysis(item.name, false, item.natives.windows);
                    }
                    else
                    {
                        str2 = SLC.libAnalysis(item.name, false, "");
                    }

                    var download2 = new MCDownload
                    {
                        name = item.name,
                        mainClass = root.mainClass
                    };

                    string[] strArray = item.name.Split(':');
                    if (strArray[1].IndexOf("lwjgl") >= 0 && strArray[2] == "3.2.1") continue;
                    download2.Url = dSI + str2.Replace('\\', Convert.ToChar("/"));
                    download2.path = System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2;

                    if ((item.downloads != null) && (item.downloads.artifact != null))
                    {
                        if (item.downloads.artifact.url.IndexOf("libraries.minecraft.net") < 0 && item.downloads.artifact.url.IndexOf("files.minecraftforge.net") < 0)
                        {
                            if (item.downloads.artifact.url != "" && item.downloads.artifact.url != null && item.downloads.artifact.url.IndexOf(" ") < 0)
                            {
                                download2.Url = item.downloads.artifact.url + str2.Replace('\\', Convert.ToChar("/"));
                            }
                        }

                        if ((item.downloads.artifact.url.IndexOf("files.minecraftforge.net") != -1))
                        {
                            char[] chArray2 = new char[] { ':' };
                            string[] strArray2 = item.name.Split(chArray2);
                            var str3 = strArray2[2];

                            if (strArray2[2].IndexOf('-') != -1)
                            {
                                string[] urlArray = strArray2[2].Split('-');

                                if (urlArray.Length == 3)
                                {
                                    str3 = urlArray[0] + "-" + urlArray[1];
                                }
                            }

                            string[] textArray1 = new string[] { strArray2[0].Replace('.', Convert.ToChar(@"\")), @"\", strArray2[1], @"\", str3, @"\", strArray2[1], "-", strArray2[2], ".jar" };
                            str2 = string.Concat(textArray1);
                            download2.Url = "https://files.minecraftforge.net/maven/" + str2.Replace('\\', Convert.ToChar("/"));
                        }

                        if (strArray[1] == "OptiFine")
                        {
                            download2.Url = item.downloads.artifact.url;
                        }
                        else if (strArray[1] == "liteloader")
                        {
                            download2.Url = item.downloads.artifact.url;
                        }
                    }

                    if (strArray[1] == "forge")
                    {
                        char[] chArray4 = new char[] { '-' };
                        string[] strArray3 = strArray[2].Split(chArray4);
                        download2.Url = download.ForgeCoreDownload(strArray3[0], strArray3[1]).Url;
                        list.Add(download2);
                        download2.name = "";
                    }

                    list.Add(download2);
                }

                downloadArray = SLC.screening(list.ToArray());
            }
            catch (Exception)
            {
                throw new SquareMinecraftLauncherException("版本有问题，请重新下载");
            }

            return downloadArray;
        }

        /// <summary>
        /// 取所有依赖库
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetAllLibrary(string version)
        {
            List<MCDownload> list = new List<MCDownload>();
            MCDownload[] allFile = GetAllFile(version);
            MCDownload[] allNatives = GetAllNatives(version);

            for (int i = 0; i < allFile.Length; i++)
            {
                int index = 0;

                while (index < allNatives.Length)
                {
                    if (allFile[i].path == allNatives[index].path)
                    {
                        break;
                    }

                    index++;
                }

                if (index == allNatives.Length)
                {
                    list.Add(allFile[i]);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取所有Natives
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetAllNatives(string version)
        {
            MCDownload[] downloadArray;
            YESAPI.Tts();

            try
            {
                List<MCDownload> list = new List<MCDownload>();
                var dSI = DSI;

                if (DSI == "Minecraft")
                {
                    dSI = "https://libraries.minecraft.net/";
                }
                else if (DSI == null)
                {
                    dSI = "https://bmclapi2.bangbang93.com/libraries/";
                }

                foreach (JToken token in SLC.versionjson(version)["libraries"])
                {
                    if ((token["natives"] != null) && (token["natives"]["windows"] != null))
                    {
                        var str2 = SLC.libAnalysis(token["name"].ToString(), false, token["natives"]["windows"].ToString());

                        var item = new MCDownload
                        {
                            Url = dSI + str2.Replace('\\', '/'),
                            path = System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + str2
                        };

                        list.Add(item);
                    }
                }

                downloadArray = list.ToArray();
            }
            catch (Exception)
            {
                throw new SquareMinecraftLauncherException("版本有问题，请重新下载");
            }

            return downloadArray;
        }

        /// <summary>
        /// 取所有Asset
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetAllTheAsset(string version)
        {
            try
            {
                SLC.SetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\assets");
                SLC.SetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\assets\indexes");
                var jo = SLC.versionjson<json4.Root>(version);
                var json = web.getHtml(jo.AssetIndex.url);
                var FileName = jo.AssetIndex.url.Split('/');
                SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\assets\indexes\" + FileName[FileName.Length - 1], json);
                string[] str = new string[0];
                List<MCDownload> str2 = new List<MCDownload>();
                var j = JObject.Parse(json).Value<JObject>("objects");
                var json1 = (JObject)JsonConvert.DeserializeObject(json);
                string jstr;
                var dsi = "http://resources.download.minecraft.net";

                foreach (var o in j.Properties())
                {
                    jstr = o.Name;
                    var assets = new MCDownload();
                    var hash = json1["objects"][o.Name]["hash"].ToString();
                    assets.path = System.Directory.GetCurrentDirectory() + @"\.minecraft\assets\objects\" + hash[0] + hash[1] + "\\" + hash;
                    assets.Url = dsi + @"/" + hash[0] + hash[1] + "/" + hash;
                    str2.Add(assets);
                }

                return str2.ToArray();
            }
            catch (Exception ex)
            {
                throw new SquareMinecraftLauncherException("无法连接网络");
            }
        }

        /// <summary>
        /// 取.minecraft所有存在的版本
        /// </summary>
        /// <returns></returns>
        public AllTheExistingVersion[] GetAllTheExistingVersion()
        {
            YESAPI.Tts();
            new mcbbsnews();
            List<AllTheExistingVersion> list = new List<AllTheExistingVersion>();

            if (!System.IO.Directory.Exists(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions"))
            {
                throw new SquareMinecraftLauncherException("没有找到任何版本");
            }

            foreach (string str in System.IO.Directory.GetDirectories(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions"))
            {
                var str2 = SLC.app(str, Convert.ToChar(@"\"), "versions");

                if (File.Exists(str + @"\" + str2 + ".jar") && File.Exists(str + @"\" + str2 + ".json"))
                {
                    var root = new ForgeY.Root();

                    try
                    {
                        root = JsonConvert.DeserializeObject<ForgeY.Root>(SLC.GetFile(str + @"\" + str2 + ".json"));
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    var item = new AllTheExistingVersion
                    {
                        path = str,
                        version = str2
                    };

                    if (!vp)
                    {
                        new Thread(new ThreadStart(SLC.MCVersion)) { IsBackground = true }.Start();
                        vp = true;
                    }

                    if (mcV.ToArray().Length == 0)
                    {
                        ForgeInstallCore.Delay(2000);
                        char[] separator = new char[] { '|' };
                        string[] strArray2 = SLC.GetFile(@".minecraft\version.Sika").Split(separator);

                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            char[] chArray2 = new char[] { '&' };
                            string[] strArray3 = strArray2[i].Split(chArray2);

                            var mc = new mc
                            {
                                version = strArray3[0],
                                url = strArray3[1]
                            };

                            mcV.Add(mc);
                        }
                    }

                    try
                    {
                        foreach (mc mc2 in mcV)
                        {
                            if (mc2.url == root.downloads.client.url)
                            {
                                item.IdVersion = mc2.version;
                                break;
                            }
                        }

                        if (item.IdVersion != null) list.Add(item);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Authlib_Injector验证
        /// </summary>
        /// <param name="yggdrasilURL"></param>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Skin GetAuthlib_Injector(string yggdrasilURL, string username, string password)
        {
            var str = web.Post(yggdrasilURL + "/authserver/authenticate", "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}");
            var root = new BlessingSkin.Root();

            try
            {
                root = JsonConvert.DeserializeObject<BlessingSkin.Root>(str);
            }
            catch (Exception)
            {
                throw new SquareMinecraftLauncherException("yggdrasil网址有误");
            }

            if (root == null)
            {
                throw new SquareMinecraftLauncherException("网络有问题");
            }

            if (root.accessToken == null)
            {
                throw new SquareMinecraftLauncherException(Regex.Unescape(JsonConvert.DeserializeObject<BlessingSkinError>(str).errorMessage));
            }

            var skin = new Skin
            {
                accessToken = root.accessToken
            };

            List<SkinName> list = new List<SkinName>();

            foreach (BlessingSkin.AvailableProfilesItem item in root.availableProfiles)
            {
                var name = new SkinName
                {
                    Name = item.name,
                    uuid = item.id
                };

                list.Add(name);
            }

            skin.NameItem = list.ToArray();
            return skin;
        }

        /// <summary>
        /// Forge是否需要更新
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public async Task<bool> GetCompareForgeVersions(string version)
        {
            var localForgeVersion = GetLocalForgeVersion(version);

            if (localForgeVersion == null)
            {
                throw new SquareMinecraftLauncherException("没有装Forge");
            }

            char[] separator = new char[] { '.' };
            var MF = await GetMaxForge(version);
            string[] strArray = MF.ForgeVersion.Split(separator);
            char[] chArray2 = new char[] { '-' };
            char[] chArray3 = new char[] { '.' };
            string[] strArray2 = localForgeVersion.Split(chArray2)[1].Split(chArray3);

            if (strArray2.Length == 3)
            {
                if (((Convert.ToInt32(strArray2[0]) <= Convert.ToInt32(strArray[0])) && (Convert.ToInt32(strArray2[1]) <= Convert.ToInt32(strArray[1]))) && ((Convert.ToInt32(strArray2[2]) <= Convert.ToInt32(strArray[2])) && (strArray.Length == 4)))
                {
                    return true;
                }
            }
            else if (((Convert.ToInt32(strArray2[0]) <= Convert.ToInt32(strArray[0])) && (Convert.ToInt32(strArray2[1]) <= Convert.ToInt32(strArray[1]))) && ((Convert.ToInt32(strArray2[2]) <= Convert.ToInt32(strArray[2])) && (Convert.ToInt32(strArray2[3]) <= Convert.ToInt32(strArray[3]))))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取对应版本所有Forge列表
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public async Task<ForgeList[]> GetForgeList(string version)
        {
            var localForgeVersion = GetLocalForgeVersion(version);

            if (localForgeVersion != null)
            {
                char[] separator = new char[] { '-' };
                version = localForgeVersion.Split(separator)[0];
            }
            else
            {
                foreach (AllTheExistingVersion version2 in GetAllTheExistingVersion())
                {
                    if (version2.version == version)
                    {
                        version = version2.IdVersion;
                    }
                }
            }

            string str2 = null;

            await Task.Factory.StartNew(() =>
            {
                str2 = web.getHtml("https://bmclapi2.bangbang93.com/forge/minecraft/" + version);
            });

            if ((str2 != "[]") && (str2 != null))
            {
                List<ForgeList> list = new List<ForgeList>();

                foreach (JToken token in (JArray)JsonConvert.DeserializeObject(str2))
                {
                    var item = new ForgeList
                    {
                        version = token["mcversion"].ToString(),
                        ForgeVersion = token["version"].ToString(),
                        ForgeTime = token["modified"].ToString()
                    };

                    list.Add(item);
                }

                return list.ToArray();
            }

            if (str2 == null)
            {
                throw new SquareMinecraftLauncherException("访问失败");
            }

            throw new SquareMinecraftLauncherException("版本有误或目前没有该版本");
        }

        /// <summary>
        /// 取java路径
        /// </summary>
        /// <returns></returns>
        public string GetJavaPath()
        {
            RegistryKey key;
            var str = "";

            if (Environment.Is64BitOperatingSystem)
            {
                key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            }
            else
            {
                key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            }

            try
            {
                str = key.OpenSubKey(@"SOFTWARE\JavaSoft\Java Runtime Environment\1.8", false).GetValue("JavaHome", true).ToString() + @"\bin\javaw.exe";
            }
            catch
            {
            }

            key.Close();

            if (str == "")
            {
                int length = Environment.SystemDirectory.Length;
                var str2 = Environment.SystemDirectory.Remove(3, length - 3);
                string[] directories = new string[0];

                if (GetOSBit() == 0x40)
                {
                    try
                    {
                        directories = System.IO.Directory.GetDirectories(str2 + @"Program Files\Java\");
                    }
                    catch
                    {
                    }

                    if (directories.Length != 0)
                    {
                        if (SLC.FileExist(directories[0] + @"\bin\javaw.exe") == null)
                        {
                            str = directories[0] + @"\bin\javaw.exe";
                        }

                        return str;
                    }
                }

                if (directories.Length == 0)
                {
                    try
                    {
                        directories = System.IO.Directory.GetDirectories(str2 + @"Program Files (x86)\Java\");
                    }
                    catch
                    {
                    }
                }

                if ((directories.Length != 0) && (SLC.FileExist(directories[0] + @"\bin\javaw.exe") == null))
                {
                    str = directories[0] + @"\bin\javaw.exe";
                }
            }

            return str;
        }

        /// <summary>
        /// 取Liteloader列表
        /// </summary>
        /// <returns></returns>
        public async Task<LiteloaderList[]> GetLiteloaderList()
        {
            List<LiteloaderList> list = new List<LiteloaderList>();
            string text1 = null;

            await Task.Factory.StartNew(() =>
            {
                text1 = web.getHtml("https://bmclapi2.bangbang93.com/liteloader/list");
            });

            if (text1 == null)
            {
                throw new SquareMinecraftLauncherException("获取失败");
            }

            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(text1))
            {
                var item = new LiteloaderList();
                List<Lib> list3 = new List<Lib>();
                item.version = token["version"].ToString();
                item.mcversion = token["mcversion"].ToString();

                foreach (JToken token2 in token["build"]["libraries"])
                {
                    var lib = new Lib
                    {
                        name = token2["name"].ToString()
                    };

                    list3.Add(lib);
                }

                item.lib = list3.ToArray();
                item.tweakClass = token["build"]["tweakClass"].ToString();
                list.Add(item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取对应版本的安装Forge版本
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public string GetLocalForgeVersion(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);

                    if (strArray[0] == "net.minecraftforge")
                    {
                        return strArray[2];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 取对应版本最新的Forge
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public async Task<ForgeList> GetMaxForge(string version)
        {
            ForgeList[] forgeList = await GetForgeList(version);
            int index = 0;
            var forgeVersion = forgeList[0].ForgeVersion;

            for (int i = 1; forgeList.Length > i; i++)
            {
                char[] separator = new char[] { '.' };
                string[] strArray = forgeVersion.Split(separator);
                char[] chArray2 = new char[] { '.' };
                string[] strArray2 = forgeList[i].ForgeVersion.Split(chArray2);

                if (strArray.Length != 3)
                {
                    if (((Convert.ToInt32(strArray[0]) <= Convert.ToInt32(strArray2[0])) && (Convert.ToInt32(strArray[1]) <= Convert.ToInt32(strArray2[1]))) && ((Convert.ToInt32(strArray[2]) <= Convert.ToInt32(strArray2[2])) && (Convert.ToInt32(strArray[3]) <= Convert.ToInt32(strArray2[3]))))
                    {
                        index = i;
                        forgeVersion = forgeList[i].ForgeVersion;
                    }
                }
                else if (((Convert.ToInt32(strArray[0]) <= Convert.ToInt32(strArray2[0])) && (Convert.ToInt32(strArray[1]) <= Convert.ToInt32(strArray2[1]))) && (Convert.ToInt32(strArray[2]) <= Convert.ToInt32(strArray2[2])))
                {
                    index = i;
                    forgeVersion = forgeList[i].ForgeVersion;
                }
            }

            return forgeList[index];
        }

        /// <summary>
        /// 取MC列表
        /// </summary>
        /// <returns></returns>
        public async Task<MCVersionList[]> GetMCVersionList()
        {
            string text = null;

            await Task.Factory.StartNew(() =>
            {
                text = web.getHtml("https://launchermeta.mojang.com/mc/game/version_manifest.json");
            });

            if (text == null)
            {
                throw new SquareMinecraftLauncherException("请求失败");
            }

            List<MCVersionList> list = new List<MCVersionList>();

            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(new mcbbsnews().TakeTheMiddle(text, "\"versions\":", "]}") + "]"))
            {
                var str2 = SLC.MCVersionAnalysis(token["type"].ToString());

                var item = new MCVersionList
                {
                    type = str2,
                    id = token["id"].ToString(),
                    releaseTime = token["releaseTime"].ToString()
                };

                list.Add(item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取合适内存
        /// </summary>
        /// <returns></returns>
        public MemoryInformation GetMemorySize()
        {
            YESAPI.Tts();
            MEMORY_INFO meminfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref meminfo);

            var information = new MemoryInformation
            {
                TotalMemory = (int)(meminfo.dwTotalVirtual / 0x100000)
            };

            if (information.TotalMemory == 0)
            {
                information.AppropriateMemory = 512;
                return information;
            }

            if (GetOSBit() == 64)
            {
                if (GetJavaPath().IndexOf("x86") >= 0)
                {
                    information.AppropriateMemory = 512;
                    return information;
                }

                information.AppropriateMemory = ((1024 * information.TotalMemory) / 1024) / 2;
                return information;
            }

            if (information.TotalMemory <= 1024)
            {
                information.AppropriateMemory = 512;
                return information;
            }

            information.AppropriateMemory = 1024;
            return information;
        }

        /// <summary>
        /// 取缺少的Asset
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetMissingAsset(string version)
        {
            List<MCDownload> list = new List<MCDownload>();

            foreach (MCDownload download in GetAllTheAsset(version))
            {
                var path = download.path;

                if (SLC.FileExist(path) != null)
                {
                    list.Add(download);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取缺少的依赖库包括（Natives）
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetMissingFile(string version)
        {
            YESAPI.Tts();
            List<MCDownload> list = new List<MCDownload>();

            foreach (MCDownload download in GetAllFile(version))
            {
                if (SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取缺少的依赖库
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetMissingLibrary(string version)
        {
            List<MCDownload> list = new List<MCDownload>();

            foreach (MCDownload download in GetAllLibrary(version))
            {
                if (SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取缺少的Natives
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetMissingNatives(string version)
        {
            YESAPI.Tts();
            List<MCDownload> list = new List<MCDownload>();

            foreach (MCDownload download in GetAllNatives(version))
            {
                if (SLC.FileExist(download.path) != null)
                {
                    list.Add(download);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取OptiFine版本
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public async Task<OptiFineList[]> GetOptiFineList(string version)
        {
            AllTheExistingVersion[] allTheExistingVersion = GetAllTheExistingVersion();

            foreach (AllTheExistingVersion version2 in allTheExistingVersion)
            {
                if (version2.version == version)
                {
                    version = version2.IdVersion;
                    break;
                }

                if (version2.version == allTheExistingVersion[allTheExistingVersion.Length - 1].version)
                {
                    throw new SquareMinecraftLauncherException("未找到该版本");
                }
            }

            List<OptiFineList> list = new List<OptiFineList>();
            string str = null;

            await Task.Factory.StartNew(() =>
            {
                str = web.getHtml("https://bmclapi2.bangbang93.com/optifine/" + version);
            });

            switch (str)
            {
                case null:
                    throw new SquareMinecraftLauncherException("获取失败");

                case "[]":
                    throw new SquareMinecraftLauncherException("OptiFine不支持该版本");
            }

            foreach (JToken token in (JArray)JsonConvert.DeserializeObject(str))
            {
                var item = new OptiFineList
                {
                    mcversion = token["mcversion"].ToString(),
                    filename = token["filename"].ToString(),
                    type = token["type"].ToString(),
                    patch = token["patch"].ToString()
                };

                list.Add(item);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取系统位数
        /// </summary>
        /// <returns></returns>
        public int GetOSBit()
        {
            YESAPI.Tts();

            if (Environment.Is64BitOperatingSystem)
            {
                return 0x40;
            }

            return 0x20;
        }

        /// <summary>
        /// 取存在的依赖库
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public MCDownload[] GetTheExistingLibrary(string version)
        {
            List<MCDownload> list = new List<MCDownload>();

            foreach (MCDownload download in GetAllLibrary(version))
            {
                if (SLC.FileExist(download.path) == null)
                {
                    list.Add(download);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 统一通行证验证
        /// </summary>
        /// <param name="ID">服务器ID</param>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UnifiedPass GetUnifiedPass(string ID, string username, string password)
        {
            var pass = new UnifiedPass();
            var str = web.Post("https://auth2.nide8.com:233/" + ID + "/authserver/authenticate", "{\"agent\": {\"name\": \"Sika Deer Launcher\",\"version\": 2.23},\"username\": \"" + username + "\",\"password\": \"" + password + "\",\"clientToken\": \"htty\",\"requestUser\": true}");

            if (str == null)
            {
                throw new SquareMinecraftLauncherException("请求失败");
            }

            var root = JsonConvert.DeserializeObject<UPerror.Root>(str);

            if (root.errorMessage != null)
            {
                throw new SquareMinecraftLauncherException(Regex.Unescape(root.errorMessage));
            }

            var root2 = JsonConvert.DeserializeObject<UP.Root>(str);
            pass.accessToken = root2.accessToken;
            pass.id = root2.selectedProfile.id;
            pass.name = root2.selectedProfile.name;
            return pass;
        }

        /// <summary>
        /// 取统一通行证皮肤
        /// </summary>
        /// <param name="ID">服务器ID</param>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UnifiedPassesTheSkin[] GetUnifiedPassesTheSkin(string ID, string username, string password)
        {
            List<UnifiedPassesTheSkin> list = new List<UnifiedPassesTheSkin>();
            var pass = GetUnifiedPass(ID, username, password);
            var str = web.getHtml("https://auth2.nide8.com:233/" + ID + "/sessionserver/session/minecraft/profile/" + pass.id);

            if (str == null)
            {
                throw new SquareMinecraftLauncherException("请求失败");
            }

            foreach (UPSkin.PropertiesItem item in JsonConvert.DeserializeObject<UPSkin.Root>(str).properties)
            {
                var root = JsonConvert.DeserializeObject<UPSkinBase.Root>(Convert.ToBase64String(Encoding.Default.GetBytes(item.value)));
                var skin = new UnifiedPassesTheSkin();

                if (root.textures.SKIN != null)
                {
                    skin.Skin = root.textures.SKIN.url;
                }

                if (root.textures.CAPE != null)
                {
                    skin.Cape = root.textures.CAPE.url;
                }

                list.Add(skin);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 取Liteloader是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool LiteloaderExist(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);

                    if ((strArray[0] == "com.mumfrey") && (strArray[1] == "liteloader"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Liteloader是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="LiteloaderVersion">返回Liteloader版本</param>
        /// <returns></returns>
        public bool LiteloaderExist(string version, ref string LiteloaderVersion)
        {
            using (List<LibrariesItem>.Enumerator enumerator = SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);

                    if ((strArray[0] == "com.mumfrey") && (strArray[1] == "liteloader"))
                    {
                        LiteloaderVersion = strArray[2];
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Liteloader安装
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public async Task<bool> liteloaderInstall(string version)
        {
            var core = new LiteloaderCore();
            var t = await core.LiteloaderJson(version);
            SLC.wj(@".minecraft\versions\" + version + @"\" + version + ".json", t);
            return true;
        }

        /// <summary>
        /// 正版登录
        /// </summary>
        /// <param name="username">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Getlogin MinecraftLogin(string username, string password)
        {
            YESAPI.Tts();

            if (((username == "") || (password == "")) || ((username == null) || (password == null)))
            {
                throw new SquareMinecraftLauncherException("账号密码不得为空");
            }

            var str = web.Post("https://authserver.mojang.com/authenticate", "{\"agent\":{\"name\":\"Minecraft\",\"version\":\"1\"},\"username\":\"" + username + "\", \"password\":\"" + password + "\", \"requestUser\":\"true\"}");

            if ((str == null) || !(str != ""))
            {
                throw new SquareMinecraftLauncherException("请检查网络");
            }

            Console.WriteLine(str);
            var root = JsonConvert.DeserializeObject<json2.Root>(str);

            if (root.errorMessage == null)
            {
                var getlogin = new Getlogin
                {
                    uuid = root.selectedProfile.id,
                    token = root.accessToken,
                    name = root.selectedProfile.name
                };

                Console.WriteLine(getlogin.token);
                var str1 = web.Post("https://authserver.mojang.com/authenticate", getlogin.token, "Authorization");
                Console.WriteLine(str1);
                string[] textArray2 = new string[] { "{", root.user.properties[0].name, ":[", root.user.properties[0].value, "]}" };
                getlogin.twitch = string.Concat(textArray2);
                return getlogin;
            }

            if (root.error == "ForbiddenOperationException")
            {
                if (root.errorMessage == "Invalid credentials. Account migrated, use e-mail as username.")
                {
                    throw new SquareMinecraftLauncherException("凭证错误");
                }

                if (root.errorMessage == "Invalid credentials. Invalid username or password.")
                {
                    throw new SquareMinecraftLauncherException("密码账户错误");
                }
            }

            throw new SquareMinecraftLauncherException(root.error);
        }

        /// <summary>
        /// Optifine是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public bool OptifineExist(string version)
        {
            using (List<LibrariesItem>.Enumerator enumerator = SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);

                    if ((strArray[0] == "optifine") && (strArray[1] == "OptiFine"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Optifine是否存在
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="OptifineVersion">返回Optifine版本</param>
        /// <returns></returns>
        public bool OptifineExist(string version, ref string OptifineVersion)
        {
            using (List<LibrariesItem>.Enumerator enumerator = SLC.versionjson<Root1>(version).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = enumerator.Current.name.Split(separator);

                    if ((strArray[0] == "optifine") && (strArray[1] == "OptiFine"))
                    {
                        OptifineVersion = strArray[2];
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Optifine安装
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="patch">patch</param>
        /// <returns></returns>
        public async Task<bool> OptifineInstall(string version, string patch)
        {
            OptiFineList[] optiFineList = await GetOptiFineList(version);
            AllTheExistingVersion[] allTheExistingVersion = GetAllTheExistingVersion();
            var str = version;

            foreach (AllTheExistingVersion version2 in allTheExistingVersion)
            {
                if (version2.version == version)
                {
                    version = version2.IdVersion;
                    break;
                }

                if (version2.version == allTheExistingVersion[allTheExistingVersion.Length - 1].version)
                {
                    throw new SquareMinecraftLauncherException("未找到该版本");
                }
            }

            var list = new OptiFineList();

            foreach (OptiFineList list2 in optiFineList)
                if (list2.mcversion == version) list = list2;
            

            var core = new OptifineCore();
            SLC.wj(@".minecraft\versions\" + str + @"\" + str + ".json", await core.OptifineJson(str, list));
            return true;
        }

        /// <summary>
        /// 设置Minecraft目录路径
        /// </summary>
        /// <param name="Path">.minecraft路径</param>
        public void SetMinecraftFilesPath(string Path)
        {
            System.Directory.Files = Path != null ? Path : System.IO.Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// 卸载扩展包
        /// </summary>
        /// <param name="ExpansionPack">扩展包类型</param>
        /// <param name="version">版本</param>
        public void UninstallTheExpansionPack(ExpansionPack ExpansionPack, string version)
        {
            var str = version;
            var download = new MinecraftDownload();
            version = "";

            foreach (AllTheExistingVersion version2 in GetAllTheExistingVersion())
            {
                if (version2.version == str)
                {
                    version = version2.IdVersion;
                }
            }

            if (version == "")
            {
                throw new SquareMinecraftLauncherException("未找到该版本");
            }

            version = str;
            string[] strArray = new string[0];

            using (List<LibrariesItem>.Enumerator enumerator = SLC.versionjson<Root1>(str).libraries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray2 = enumerator.Current.name.Split(separator);

                    if ((strArray2[0] == "optifine") && (strArray2[1] == "OptiFine"))
                    {
                        char[] chArray2 = new char[] { '_' };
                        strArray = strArray2[2].Split(chArray2);
                    }
                }
            }

            bool flag = LiteloaderExist(version);
            var download2 = download.MCjsonDownload(version);
            var text = web.getHtml(download2.Url);

            switch (ExpansionPack)
            {
                case ExpansionPack.Forge:
                    if (!ForgeExist(version))
                    {
                        throw new SquareMinecraftLauncherException("没有安装Forge");
                    }

                    SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    if (strArray.Length != 0)
                    {
                        SLC.opKeep(version, strArray[strArray.Length - 1]);
                    }

                    if (!flag) break;

                    SLC.liKeep(str);
                    return;

                case ExpansionPack.Liteloader:
                    if (!LiteloaderExist(version))
                    {
                        throw new SquareMinecraftLauncherException("没有安装Liteloader");
                    }

                    if (!SLC.ForgeKeep(version, text))
                    {
                        SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    }

                    if (strArray.Length == 0) break;
                    SLC.opKeep(str, strArray[strArray.Length - 1]);
                    return;

                case ExpansionPack.Optifine:
                    if (!OptifineExist(version))
                    {
                        throw new SquareMinecraftLauncherException("没有安装Optifine");
                    }

                    if (!SLC.ForgeKeep(version, text))
                    {
                        SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", text);
                    }

                    if (!flag) break;
                    SLC.liKeep(version);
                    return;

                case ExpansionPack.Fabric:
                    if (FabricExist(version))
                    {
                        var fabricUninstall = new fabricUninstall();
                        var Uninstall = fabricUninstall.Uninstall(version);
                        SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + str + @"\" + str + ".json", Uninstall);
                    }
                    else
                    {
                        throw new SquareMinecraftLauncherException("没有安装Fabric");
                    }

                    break;

                default:
                    return;
            }
        }

        static string GetProductVersion(string filename)
        {
            var ver = FileVersionInfo.GetVersionInfo(filename);
            return ver.ProductVersion;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {

            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }
    }
} 