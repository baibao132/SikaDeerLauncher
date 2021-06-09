namespace SquareMinecraftLauncher.Core
{
    using global::SquareMinecraftLauncher.Core.Forge;
    using global::SquareMinecraftLauncher.Minecraft;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    internal class LiteloaderCore
    {
        MinecraftDownload Minecraft = new MinecraftDownload();
        SquareMinecraftLauncherCore SLC = new SquareMinecraftLauncherCore();
        Tools tools = new Tools();

        public string liteloaderJsonY(ForgeY.Root versionText, LiteloaderList libraries, string version)
        {
            string[] textArray1 = new string[] { "\"assetIndex\": {\"id\": \"", versionText.assetIndex.id, "\",\"size\":", versionText.assetIndex.size, ",\"url\": \"", versionText.assetIndex.url, "\"},\"assets\": \"", versionText.assets, "\",\"downloads\": {\"client\": {\"url\":\"", versionText.downloads.client.url, "\"}},\"id\": \"", versionText.id, "\",\"libraries\": [" };
            var str = string.Concat(textArray1);

            foreach (Lib lib in libraries.lib)
            {
                var item2 = new ForgeY.LibrariesItem
                {
                    name = lib.name
                };

                var downloads2 = new ForgeY.Downloads
                {
                    artifact = new ForgeY.Artifact()
                };

                downloads2.artifact.url = " ";
                item2.downloads = downloads2;
                versionText.libraries.Add(item2);
            }

            var item = new ForgeY.LibrariesItem
            {
                name = "com.mumfrey:liteloader:" + libraries.version
            };

            var downloads = new ForgeY.Downloads
            {
                artifact = new ForgeY.Artifact()
            };

            downloads.artifact.url = Minecraft.DownloadLiteloader(libraries.version).Url;
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

        internal async Task<string> LiteloaderJson(string version)
        {
            if (tools.LiteloaderExist(version))
            {
                throw new SquareMinecraftLauncherException("已经安装过了，无需再次安装");
            }

            string idVersion = null;

            foreach (AllTheExistingVersion version2 in tools.GetAllTheExistingVersion())
            {
                if (version2.version == version)
                {
                    idVersion = version2.IdVersion;
                }
            }

            LiteloaderList[] liteloaderList = await tools.GetLiteloaderList();
            var libraries = new LiteloaderList();

            foreach (LiteloaderList list2 in liteloaderList)
            {
                if (list2.mcversion == idVersion)
                {
                    libraries = list2;
                    break;
                }

                if (list2.mcversion == liteloaderList[liteloaderList.Length - 1].mcversion)
                {
                    throw new SquareMinecraftLauncherException("该版本不支持安装");
                }
            }

            var file = SLC.GetFile(System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + ".json");
            var versionText = JsonConvert.DeserializeObject<ForgeY.Root>(file);
            string str2 = null;
            str2 = str2 + "{" + liteloaderJsonY(versionText, libraries, version);
            var root2 = JsonConvert.DeserializeObject<ForgeJsonEarly.Root>(file);
            string[] textArray2 = new string[] { str2, ",\"minecraftArguments\": \"", root2.minecraftArguments, " --tweakClass ", libraries.tweakClass, "\"}" };
            return string.Concat(textArray2);
        }
    }
} 