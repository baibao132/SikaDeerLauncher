namespace SquareMinecraftLauncher.Core
{
    using global::SquareMinecraftLauncher.Core.Forge;
    using global::SquareMinecraftLauncher.Minecraft;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    internal class OptifineCore
    {
        MinecraftDownload Minecraft = new MinecraftDownload();
        SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        Tools tools = new Tools();

        internal string liteloaderJsonY(ForgeY.Root versionText, string type, string patch, string version, string filename)
        {
            string[] textArray1 = new string[] { "\"assetIndex\": {\"id\": \"", versionText.assetIndex.id, "\",\"size\":", versionText.assetIndex.size, ",\"url\": \"", versionText.assetIndex.url, "\"},\"assets\": \"", versionText.assets, "\",\"downloads\": {\"client\": {\"url\":\"", versionText.downloads.client.url, "\"}},\"id\": \"", versionText.id, "\",\"libraries\": [" };
            var str = string.Concat(textArray1);
            var item = new ForgeY.LibrariesItem();
            string[] textArray2 = new string[] { "optifine:OptiFine:", versionText.id, "_", type, "_", patch };
            item.name = string.Concat(textArray2);
            var artifact = new ForgeY.Artifact();
            var downloads = new ForgeY.Downloads();
            artifact.url = Minecraft.DownloadOptifine(version, filename).Url;
            downloads.artifact = artifact;
            item.downloads = downloads;
            versionText.libraries.Add(item);

            for (int i = 0; versionText.libraries.ToArray().Length > i; i++)
            {
                str = str + "{\"name\":\"" + versionText.libraries[i].name + "\",";

                if ((versionText.libraries[i].downloads == null) || (versionText.libraries[i].downloads.artifact == null))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                else
                {
                    str = str + "\"downloads\":{\"artifact\":{\"url\":\"" + versionText.libraries[i].downloads.artifact.url + "\"}}";
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

            return (str + ",\"mainClass\": \"" + versionText.mainClass + "\"");
        }

        internal async Task<string> OptifineJson(string version, OptiFineList optiFineList)
        {
            if (tools.OptifineExist(version))
            {
                throw new SquareMinecraftLauncherException("已经安装过了,无需再次安装");
            }

            var file = SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
            var root = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(file);
            var versionText = JsonConvert.DeserializeObject<ForgeY.Root>(file);
            string str2 = null;

            if (root.minecraftArguments == null)
            {
                var obj2 = (JObject)JsonConvert.DeserializeObject(file);
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

                try
                {
                    await tools.GetCompareForgeVersions(version);
                    str2 = str2 + "\"--tweakClass\",";
                    str2 = str2 + "\"optifine.OptiFineForgeTweaker\"]},";
                }
                catch (SquareMinecraftLauncherException)
                {
                    str2 = str2 + "\"--tweakClass\"," + "\"optifine.OptiFineTweaker\"]},";
                }

                return (str2 + liteloaderJsonY(versionText, optiFineList.type, optiFineList.patch, version, optiFineList.filename) + "}");
            }

            str2 = str2 + "{" + liteloaderJsonY(versionText, optiFineList.type, optiFineList.patch, version, optiFineList.filename);
            var root3 = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(file);

            try
            {
                await tools.GetCompareForgeVersions(version);
                return (str2 + ",\"minecraftArguments\": \"" + root3.minecraftArguments + " --tweakClass optifine.OptiFineForgeTweaker\"}");
            }
            catch (SquareMinecraftLauncherException)
            {
                return (str2 + ",\"minecraftArguments\": \"" + root3.minecraftArguments + " --tweakClass optifine.OptiFineTweaker\"}");
            }
        }
    }
} 