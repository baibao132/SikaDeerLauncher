using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core
{
    internal class NewJson
    {
        internal string newJson(List<Forge.ForgeY.LibrariesItem> libraries, string Arguments, string mainClass, Forge.ForgeY.Root r)
        {
            var id = "\"id\":\"" + r.assetIndex.id;
            var size = "\"size\":\"" + r.assetIndex.size;
            var url = "\"url\":\"" + r.assetIndex.url;
            var ret = "{\"arguments\":{\"game\":[" + Arguments + "]},\"assetIndex\":{" + id + "\"," + size + "\"," + url + "\"},\"assets\":\"" + r.assets + "\",";
            ret += "\"downloads\":{\"Client\":{\"sha1\":\"" + r.downloads.client.sha1 + "\",\"size\":\"" + r.downloads.client.size + "\",\"url\":\"" + r.downloads.client.url + "\"}},";
            ret += "\"id\":\"" + r.id + "\",";
            ret += "\"libraries\":[";
            foreach (var i in libraries)//将新组成的libraries数组写成json
            {
                if (i.downloads == null)
                {
                    ret += "{\"downloads\":{\"artifact\":{\"url\":\"" + "https://libraries.minecraft.net/\"}},";
                }
                else
                {
                    ret += "{\"downloads\":{\"artifact\":{\"url\":\"" + i.downloads.artifact.url + "\"}},";
                }
                ret += "\"name\":\"" + i.name + "\"";
                if (i.natives != null)
                {
                    if (i.natives.windows != null)
                    {
                        ret += ",\"natives\":{\"windows\":\"natives - windows\"}";
                    }
                }
                ret += "},";
            }
            ret = ret.Substring(0, ret.Length - 1);
            ret += "],";
            ret += "\"mainClass\":\"" + mainClass + "\"}";
            return ret;
        }
    }
}