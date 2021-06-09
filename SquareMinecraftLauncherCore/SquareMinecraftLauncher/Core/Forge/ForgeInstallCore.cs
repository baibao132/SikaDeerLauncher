namespace SquareMinecraftLauncher.Core
{
    using global::SquareMinecraftLauncher.Core.ForgeInstall;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public sealed class ForgeInstallCore
    {
        SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();

        public static void Delay(int milliSecond)
        {
            int tickCount = Environment.TickCount;

            while (Math.Abs((int)(Environment.TickCount - tickCount)) < milliSecond)
                Application.DoEvents();
            
        }

        internal string ArgAnalysis(string name)
        {
            char[] separator = new char[] { ':' };
            string[] strArray = name.Replace("[", "").Replace("]", "").Split(separator);

            if (strArray.Length == 1)
            {
                if (name[0] == '/')
                {
                    return (System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\Forge\" + name.Replace('/', '\\'));
                }

                return name;
            }

            if (strArray.Length == 3)
            {
                var str2 = strArray[2].Replace('@', '.');
                if (str2 == strArray[2]) str2 = str2 + ".jar";
                string[] textArray1 = new string[11];
                textArray1[0] = System.Directory.GetCurrentDirectory();
                textArray1[1] = @"\.minecraft\libraries\";
                textArray1[2] = strArray[0].Replace('.', '\\');
                textArray1[3] = @"\";
                textArray1[4] = strArray[1].Replace('.', '\\');
                textArray1[5] = @"\";
                char[] chArray2 = new char[] { '@' };
                textArray1[6] = strArray[2].Split(chArray2)[0];
                textArray1[7] = @"\";
                textArray1[8] = strArray[1];
                textArray1[9] = "-";
                textArray1[10] = str2;
                return string.Concat(textArray1);
            }

            var str = strArray[3].Replace('@', '.');
            if (str == strArray[3]) str = str + ".jar";
            string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\libraries\", strArray[0].Replace('.', '\\'), @"\", strArray[1].Replace('.', '\\'), @"\", strArray[2], @"\", strArray[1], "-", strArray[2], "-", str };
            return string.Concat(textArray2);
        }

        internal DATA[] data(string json)
        {
            List<DATA> list = new List<DATA>();
            var obj2 = (JObject)JsonConvert.DeserializeObject(json);

            foreach (JProperty property in JObject.Parse(json).Value<JObject>("data").Properties())
            {
                var item = new DATA
                {
                    name = "{" + property.Name + "}",
                    arg = ArgAnalysis(obj2["data"][property.Name]["client"].ToString())
                };

                list.Add(item);
            }

            return list.ToArray();
        }

        internal async Task ForgeInstall(string path, string version, string java)
        {
            await Task.Factory.StartNew(() =>
            {
                if (SLC.FileExist(java) != null)
                {
                    throw new SquareMinecraftLauncherException("java路径不存在");
                }

                var file = SLC.GetFile(path);
                var json = JsonConvert.DeserializeObject<ForgeJson.Root>(file);
                libraries(json);
                DATA[] dataArray = data(file);

                foreach (ForgeJson.ProcessorsItem item in json.processors)
                {
                    var str2 = ArgAnalysis(item.jar);
                    var str3 = "-Xmn128m -Xss1M -Xmx512M -cp \"" + str2 + ";";

                    foreach (string str5 in item.classpath)
                        str3 = str3 + ArgAnalysis(str5) + ";";
                    

                    char[] separator = new char[] { ':' };
                    string[] strArray = item.jar.Split(separator);
                    string str4 = null;

                    for (int i = 0; (strArray.Length - 1) > i; i++)
                        str4 = str4 + strArray[i] + ".";
                    

                    if (strArray[1] == "SpecialSource")
                    {
                        str4 = str4.Replace('-', '_').ToLower() + "SpecialSource";
                    }
                    else
                    {
                        str4 = str4 + "ConsoleTool";
                    }

                    str3 = str3 + "\" " + str4;

                    foreach (string str6 in item.args)
                    {
                        if (str6[0] == '{')
                        {
                            if (str6 == "{MINECRAFT_JAR}")
                            {
                                string[] textArray1 = new string[] { str3, " \"", System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".jar\"" };
                                str3 = string.Concat(textArray1);
                            }
                            else
                            {
                                foreach (DATA data in dataArray)
                                {
                                    if (str6 == data.name)
                                    {
                                        str3 = str3 + " \"" + data.arg + "\"";
                                        break;
                                    }
                                }
                            }
                        }
                        else if (str6[0] == '[')
                        {
                            str3 = str3 + " " + ArgAnalysis(str6.Replace("[", "").Replace("]", ""));
                        }
                        else
                        {
                            str3 = str3 + " " + str6;
                        }
                    }

                    Console.WriteLine(str3);

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = java,
                        Arguments = str3,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true
                    };

                    var process1 = Process.Start(startInfo);
                    process1.WaitForExit();
                    process1.Close();
                }
            });
        }

        internal void libraries(ForgeJson.Root json)
        {
            var GacDownload = new GacDownload();
            int num = 0;

            foreach (ForgeJson.LibrariesItem item in json.libraries)
            {
                if (!(item.downloads.artifact.url == "") && (SLC.FileExist(System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + item.downloads.artifact.path.Replace('/', '\\')) != null))
                {
                    num++;
                    GacDownload.Download(System.Directory.GetCurrentDirectory() + @"\.minecraft\libraries\" + item.downloads.artifact.path.Replace('/', '\\'), item.downloads.artifact.url);
                }
            }

            bool flag = true;

            while (flag)
            {
                if (GacDownload.Failure != 0)
                {
                    if ((GacDownload.Complete + GacDownload.Failure) == num)
                    {
                        if (!ping.CheckServeStatus())
                        {
                            throw new SquareMinecraftLauncherException("安装失败");
                        }

                        libraries(json);
                        return;
                    }
                }
                else if (GacDownload.Complete == num) flag = false;

                Thread.Sleep(500);
            }
        }
    }
} 