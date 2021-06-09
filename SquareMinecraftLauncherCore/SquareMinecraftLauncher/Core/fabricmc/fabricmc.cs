using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SquareMinecraftLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core.fabricmc
{
    public class fabricmc
    {
        Tools tools = new Tools();
        Download web = new Download();

        public async Task<string[]> FabricmcList(string version)
        {
            var MCversion = tools.GetAllTheExistingVersion();
            string mc = null;

            foreach (var i in MCversion)
            {
                if (i.version == version)
                {
                    string[] fv = await FabricmcVersion();

                    foreach (var t in fv)
                    {
                        if (t == i.IdVersion)
                        {
                            mc = i.IdVersion;
                            break;
                        }
                    }

                    if (mc == null)
                    {
                        throw new SquareMinecraftLauncherException("不支持该版本安装Fabricmc");
                    }

                    break;
                }
            }

            string xml = null;

            await Task.Factory.StartNew(() =>
            {
                xml = web.getHtml("https://maven.fabricmc.net/net/fabricmc/fabric-loader/maven-metadata.xml");
            });

            Console.WriteLine(xml + "\n\n\n\n\n");

            if (xml != null)
            {
                xml = xml.Replace(" ", "");
                xml = xml.Replace("\n", "");
                xml = xml.Replace("<version>", "|");
                xml = xml.Replace("</version>", "|");
                var xmlArray = xml.Split('|');
                List<string> xmlArray1 = new List<string>();

                for (int i = 1; i < xmlArray.Length - 1; i++)
                    if (xmlArray[i] != "") xmlArray1.Add(xmlArray[i]);
                

                return xmlArray1.ToArray();
            }

            throw new SquareMinecraftLauncherException("访问失败");
        }

        public async Task<bool> FabricmcVersionInstall(string version, string loaderVersion)
        {
            var MCversion = tools.GetAllTheExistingVersion();
            bool ret = false;

            await Task.Factory.StartNew(() =>
            {
                foreach (var i in MCversion)
                {
                    if (i.version == version)
                    {
                        var mc = i.IdVersion;
                        var fabricmcInstall = new fabricmcInstall();
                        fabricmcInstall.GetLoaderVersionJson(loaderVersion, version, mc);
                        ret = true;
                    }
                }
            });

            return ret;
        }

        internal async Task<string[]> FabricmcVersion()
        {
            List<string> Version = new List<string>();
            string json = null;

            await Task.Factory.StartNew(() =>
            {
                json = web.getHtml("https://meta.fabricmc.net/v2/versions/game");
            });

            if (json != null)
            {
                var jo = (JArray)JsonConvert.DeserializeObject(json);

                foreach (var i in jo)
                    Version.Add(i["version"].ToString());
                

                return Version.ToArray();
            }

            throw new SquareMinecraftLauncherException("访问失败");
        }
    }
} 