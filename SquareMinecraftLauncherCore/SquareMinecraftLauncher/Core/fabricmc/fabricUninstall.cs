using Newtonsoft.Json;
using SquareMinecraftLauncher.Core.JsonIntegration;
using SquareMinecraftLauncher.Minecraft;
using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.fabricmc
{
    internal class fabricUninstall : NewJson
    {
        SetJson SetJson = new SetJson();
        SquareMinecraftLauncherCore SMLC = new SquareMinecraftLauncherCore();
        Tools tools = new Tools();
        internal string Uninstall(string version)
        {
            var json = SMLC.GetFile(System.Directory.GetCurrentDirectory() + "\\.minecraft\\versions\\" + version + "\\" + version + ".json");
            var libraries = librariesJson(json);
            var Arg = ArgumentsJson(json);
            var mainClass = "net.minecraft.launchwrapper.Launch";
            return newJson(libraries, Arg, mainClass, JsonConvert.DeserializeObject<Forge.ForgeY.Root>(json));
        }

        string ArgumentsJson(string json)
        {
            var fabricmcInstall = new fabricmcInstall();
            var Arg = fabricmcInstall.ArgumentsJson(json);
            Arg = Arg.Substring(0, Arg.Length - 1);
            string[] ArgArray = Arg.Split(',');
            Arg = "";
            for (int i = 1; i < ArgArray.Length; i += 2)
            {
                if (ArgArray[i - 1] == "--tweakClass" ? ArgArray[i] == "net.fabricmc.loader.launch.FabricClientTweaker" ? true : false : false)
                {
                    continue;
                }
                Arg += ArgArray[i - 1] + "," + ArgArray[i] + ",";
            }
            return Arg.Substring(0, Arg.Length - 1);
        }

        string AssetIndex(string json)
        {
            var Json = JsonConvert.DeserializeObject<Forge.ForgeY.Root>(json);
            var assetIndex = SetJson.Setjson("id", Json.assetIndex.id) + "," + SetJson.Setjson("size", Json.assetIndex.size) + "," + SetJson.Setjson("url", Json.assetIndex.url);
            return SetJson.Setjson("assetIndex", "{" + assetIndex + "}") + "," + SetJson.Setjson("assets", Json.assets);
        }

        string Downloads(string json)
        {
            var Json = JsonConvert.DeserializeObject<Forge.ForgeY.Root>(json);
            var downloads = SetJson.Setjson("sha1", Json.downloads.client.sha1) + "," + SetJson.Setjson("size", Json.downloads.client.size) + "," + SetJson.Setjson("url", Json.downloads.client.url);
            return SetJson.Setjson("downloads", SetJson.Setjson("Client", "{" + downloads + "}")) + "," + SetJson.Setjson("id", Json.id);
        }

        List<Forge.ForgeY.LibrariesItem> librariesJson(string json)
        {
            var libraries = JsonConvert.DeserializeObject<Forge.ForgeY.Root>(json);
            List<Forge.ForgeY.LibrariesItem> librariesItems = new List<Forge.ForgeY.LibrariesItem>();
            foreach (var i in libraries.libraries)//筛选掉fabric
            {
                if (i.name.Split(':')[0] != "net.fabricmc" || i.downloads.artifact.url != "http://repo.maven.apache.org/maven2/" || i.downloads.artifact.url != "https://maven.fabricmc.net/")
                {
                    librariesItems.Add(i);
                }
            }
            return librariesItems;
        }
    }
}