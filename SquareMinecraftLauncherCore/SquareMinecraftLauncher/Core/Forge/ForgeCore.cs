namespace SquareMinecraftLauncher.Core
{
    using global::SquareMinecraftLauncher.Core.Forge;
    using global::SquareMinecraftLauncher.Minecraft;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    internal class ForgeCore
    {
        MinecraftDownload download = new MinecraftDownload();
        Download Download = new Download();
        SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        Tools Tools = new Tools();

        public string ForgeJson(string version, string ForgePath)
        {
            string file = null;
            AllTheExistingVersion[] allTheExistingVersion = Tools.GetAllTheExistingVersion();

            foreach (AllTheExistingVersion version2 in allTheExistingVersion)
            {
                if (version2.version == version)
                {
                    var idVersion = version2.IdVersion;
                    break;
                }

                if (version2 == allTheExistingVersion[allTheExistingVersion.Length - 1])
                {
                    throw new SquareMinecraftLauncherException("未找到该版本");
                }
            }

            if (Tools.ForgeExist(version))
            {
                Tools.UninstallTheExpansionPack(ExpansionPack.Forge, version);
            }

            file = SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
            var root = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(file);
            var versionText = JsonConvert.DeserializeObject<ForgeY.Root>(file);
            string str2 = null;

            if (root.minecraftArguments != null)
            {
                var root3 = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(SLC.GetFile(ForgePath));
                str2 = str2 + "{" + ForgeJsonY(versionText, JsonConvert.DeserializeObject<ForgeY.Root>(SLC.GetFile(ForgePath)));

                if (Tools.OptifineExist(version))
                {
                    root3.minecraftArguments = root3.minecraftArguments + " --tweakClass optifine.OptiFineForgeTweaker";
                }

                if (Tools.LiteloaderExist(version))
                {
                    root3.minecraftArguments = root3.minecraftArguments + " --tweakClass com.mumfrey.liteloader.launch.LiteLoaderTweaker";
                }

                return (str2 + ",\"minecraftArguments\": \"" + root3.minecraftArguments + "\"}");
            }

            var obj2 = (JObject)JsonConvert.DeserializeObject(file);
            var obj3 = (JObject)JsonConvert.DeserializeObject(SLC.GetFile(ForgePath));
            str2 = str2 + "{\"arguments\": {\"game\": [";

            for (int i = 0; (obj2["arguments"]["game"].ToArray<JToken>().Length - 1) > 0; i++)
            {
                try
                {
                    obj2["arguments"]["game"][i].ToString();

                    if ((obj2["arguments"]["game"][i].ToString()[0] == '-') || (obj2["arguments"]["game"][i].ToString()[0] == '$'))
                    {
                        str2 = str2 + "\"" + obj2["arguments"]["game"][i].ToString() + "\",";
                        continue;
                    }

                    if (obj2["arguments"]["game"][i - 1].ToString()[0] == '-')
                    {
                        str2 = str2 + "\"" + obj2["arguments"]["game"][i].ToString() + "\",";
                        continue;
                    }
                }
                catch (Exception)
                {
                }

                break;
            }

            if (Tools.OptifineExist(version))
            {
                str2 = str2 + "\"--tweakClass\",\"optifine.OptiFineForgeTweaker\",";
            }

            if (Tools.LiteloaderExist(version))
            {
                str2 = str2 + " \"--tweakClass\",\"com.mumfrey.liteloader.launch.LiteLoaderTweaker\",";
            }

            for (int j = 0; (obj3["arguments"]["game"].ToArray().Length - 1) > 0; j++)
            {
                try
                {
                    obj3["arguments"]["game"][j].ToString();
                    str2 = str2 + "\"" + obj3["arguments"]["game"][j].ToString() + "\",";
                }
                catch (Exception)
                {
                    str2 = str2.Substring(0, str2.Length - 1) + "]},";
                    break;
                }
            }

            return (str2 + ForgeJsonY(versionText, JsonConvert.DeserializeObject<ForgeY.Root>(SLC.GetFile(ForgePath))) + "}");
        }

        public string ForgeJsonY(ForgeY.Root versionText, ForgeY.Root ForgeText)
        {
            string[] textArray1 = new string[] { "\"assetIndex\": {\"id\": \"", versionText.assetIndex.id, "\",\"size\":", versionText.assetIndex.size, ",\"url\": \"", versionText.assetIndex.url, "\"},\"assets\": \"", versionText.assets, "\",\"downloads\": {\"client\": {\"url\":\"", versionText.downloads.client.url, "\"}},\"id\": \"", versionText.id, "\",\"libraries\": [" };
            var str = string.Concat(textArray1);

            foreach (ForgeY.LibrariesItem item in ForgeText.libraries)
            {
                if (item.downloads == null)
                {
                    var downloads = new ForgeY.Downloads();
                    var artifact = new ForgeY.Artifact();
                    downloads.artifact = artifact;
                    item.downloads = downloads;
                }
                else if (item.downloads.artifact == null)
                {
                    var artifact2 = new ForgeY.Artifact();
                    item.downloads.artifact = artifact2;
                }

                if (item.downloads.artifact.url == null || item.downloads.artifact.url.IndexOf("files.minecraftforge.net") < 0)
                {
                    item.downloads.artifact.url = "";
                    versionText.libraries.Add(item);
                    continue;
                }

                item.downloads.artifact.url = "http://files.minecraftforge.net/maven/";
                versionText.libraries.Add(item);
            }

            for (int i = 0; versionText.libraries.ToArray().Length > i; i++)
            {
                str = str + "{\"name\":\"" + versionText.libraries[i].name + "\",";

                if (((versionText.libraries[i].downloads == null) || (versionText.libraries[i].downloads.artifact == null)) && (versionText.libraries[i].url == null))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                else if ((versionText.libraries[i].downloads != null) && (versionText.libraries[i].downloads.artifact != null))
                {
                    str = str + "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].downloads.artifact.url + "\"}}";
                }
                else if (versionText.libraries[i].url != null)
                {
                    str = str + "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].url + "\"}}";
                }

                if (versionText.libraries[i].natives != null)
                {
                    str = str + ",\"natives\": {";
                    string str2 = null;

                    if (versionText.libraries[i].natives.linux != null)
                    {
                        if (str2 != null) str2 = str2 + ",";
                        str2 = str2 + "\"linux\": \"natives - linux\"";
                    }

                    if (versionText.libraries[i].natives.osx != null)
                    {
                        if (str2 != null) str2 = str2 + ",";
                        str2 = str2 + "\"osx\": \"natives - osx\"";
                    }

                    if (versionText.libraries[i].natives.windows != null)
                    {
                        if (str2 != null) str2 = str2 + ",";
                        str2 = str2 + "\"windows\": \"" + versionText.libraries[i].natives.windows + "\"";
                    }

                    str = str + str2 + "}},";
                }
                else
                {
                    str = str + "},";
                }

                if (i == (versionText.libraries.ToArray().Length - 1))
                {
                    char[] chArray1 = str.ToCharArray();
                    chArray1[chArray1.Length - 1] = ']';
                    str = null;

                    foreach (char ch in chArray1)
                        str = str + ch.ToString();
                    
                }
            }

            return (str + ",\"mainClass\": \"" + ForgeText.mainClass + "\"");
        }

        internal string ForgeKeep(string FileText, ForgeY.Root ForgePath, string versionjson, string version)
        {
            var root = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(FileText);
            JsonConvert.DeserializeObject<ForgeY.Root>(FileText);
            string str = null;

            if (root.minecraftArguments != null)
            {
                JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(versionjson);
                str = str + "{";
                SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", versionjson);
                str = str + ForgeJsonY(JsonConvert.DeserializeObject<ForgeY.Root>(versionjson), ForgePath);
                char[] separator = new char[] { ' ' };
                string[] strArray = root.minecraftArguments.Split(separator);
                var str2 = "";

                for (int j = 1; strArray.Length > j; j += 2)
                {
                    if ((((strArray[j - 1][0] == '-') && (strArray[j] != "com.mumfrey.liteloader.launch.LiteLoaderTweaker")) && (strArray[j] != "optifine.OptiFineForgeTweaker")) || (strArray[j][0] == '$'))
                    {
                        if (j != (strArray.Length - 1))
                        {
                            string[] textArray4 = new string[] { str2, strArray[j - 1], " ", strArray[j], " " };
                            str2 = string.Concat(textArray4);
                        }
                        else
                        {
                            str2 = str2 + strArray[j - 1] + " " + strArray[j];
                        }
                    }
                }

                if (str2[str2.Length - 1] == ' ')
                {
                    str2 = str2.Substring(0, str2.Length - 1);
                }

                return (str + ",\"minecraftArguments\": \"" + str2 + "\"}");
            }

            var obj2 = (JObject)JsonConvert.DeserializeObject(FileText);
            str = str + "{\"arguments\": {\"game\": [";

            for (int i = 1; (obj2["arguments"]["game"].ToArray<JToken>().Length - 1) > 0; i += 2)
            {
                try
                {
                    obj2["arguments"]["game"][i].ToString();

                    if ((((obj2["arguments"]["game"][i - 1].ToString()[0] == '-') && (obj2["arguments"]["game"][i].ToString() != "com.mumfrey.liteloader.launch.LiteLoaderTweaker")) && (obj2["arguments"]["game"][i].ToString() != "optifine.OptiFineForgeTweaker")) || (obj2["arguments"]["game"][i - 1].ToString()[0] == '$'))
                    {
                        string[] textArray1 = new string[] { str, "\"", obj2["arguments"]["game"][i - 1].ToString(), "\",\"", obj2["arguments"]["game"][i].ToString(), "\"," };
                        str = string.Concat(textArray1);
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }

            str = str.Substring(0, str.Length - 1) + "]},";
            SLC.wj(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json", versionjson);
            return (str + ForgeJsonY(JsonConvert.DeserializeObject<ForgeY.Root>(versionjson), ForgePath) + "}");
        }
    }
} 