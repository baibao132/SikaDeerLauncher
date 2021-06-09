namespace SquareMinecraftLauncher.Core
{
    using DES;
    using global::SquareMinecraftLauncher.Core.Forge;
    using global::SquareMinecraftLauncher.Core.json;
    using global::SquareMinecraftLauncher.Minecraft;
    using json3;
    using MD5;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    internal sealed class SquareMinecraftLauncherCore
    {
        internal static bool pz;
        Download web = new Download();

        public string Replace(string originalString, string strToBeReplaced, string strToReplace)
        {
            try
            {
                char[] source = originalString.ToCharArray();
                char[] chArray2 = strToBeReplaced.ToCharArray();
                char[] chArray3 = strToReplace.ToCharArray();
                List<char> values = new List<char>();

                for (int i = 0; i < source.Count<char>(); i++)
                {
                    if (source[i] == chArray2[0])
                    {
                        bool flag = false;

                        for (int j = 0; j < chArray2.Count<char>(); j++)
                        {
                            if (((i + j) < source.Count<char>()) && (source[i + j] == chArray2[j]))
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            i += chArray2.Count<char>() - 1;

                            for (int k = 0; k < chArray3.Count<char>(); k++)
                                values.Add(chArray3[k]);
                            
                        }
                        else
                        {
                            values.Add(source[i]);
                        }

                        continue;
                    }

                    values.Add(source[i]);
                }

                return string.Join<char>("", values);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal string app(string name, char Char, string exist)
        {
            char[] separator = new char[] { Char };
            string[] strArray = name.Split(separator);

            for (int i = 0; i < strArray.Length; i++)
                if (strArray[i] == exist) return strArray[i + 1];
            

            return "";
        }

        internal void DeleteFile(string path)
        {
            if (File.GetAttributes(path) == FileAttributes.Directory)
            {
                System.IO.Directory.Delete(path, true);
            }
            else
            {
                File.Delete(path);
            }
        }

        internal void DelPathOrFile(string fileFullPath)
        {
            if (File.GetAttributes(fileFullPath) == FileAttributes.Directory)
            {
                System.IO.Directory.Delete(fileFullPath, true);
            }
            else
            {
                File.Delete(fileFullPath);
            }

            try
            {
                File.Delete(fileFullPath);
            }
            catch (Exception)
            {
            }
        }

        internal string FileExist(string file)
        {
            if (File.Exists(file)) return null;
            return file;
        }

        internal bool ForgeKeep(string version, string versionjson)
        {
            var dSI = Tools.DSI;
            var tools = new Tools();
            tools.DownloadSourceInitialization(DownloadSource.MinecraftSource);
            if (!tools.ForgeExist(version)) return false;
            var forgePath = new ForgeY.Root();
            List<ForgeY.LibrariesItem> list = new List<ForgeY.LibrariesItem>();
            MCDownload[] allFile = tools.GetAllFile(version);

            foreach (MCDownload download in allFile)
            {
                if (download.name != "")
                {
                    char[] separator = new char[] { ':' };
                    string[] strArray = download.name.Split(separator);

                    if ((download.Url.IndexOf("files.minecraftforge.net") != -1) || (strArray[1] == "forge"))
                    {
                        var item = new ForgeY.LibrariesItem();
                        var artifact = new ForgeY.Artifact();
                        var downloads = new ForgeY.Downloads();
                        artifact.url = "http://files.minecraftforge.net/maven/";
                        downloads.artifact = artifact;
                        item.name = download.name;
                        item.downloads = downloads;
                        list.Add(item);
                    }
                }
            }

            Tools.DSI = dSI;
            forgePath.libraries = list;
            forgePath.mainClass = allFile[0].mainClass;
            var core = new ForgeCore();
            string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
            string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
            wj(string.Concat(textArray1), core.ForgeKeep(GetFile(string.Concat(textArray2)), forgePath, versionjson, version));
            return true;
        }

        internal string GetFile(string path)
        {
            if (FileExist(path) == null)
            {
                try
                {
                    var reader1 = new StreamReader(path);
                    var str = reader1.ReadToEnd();
                    reader1.Close();
                    return str;
                }
                catch (Exception)
                {
                    return "";
                }
            }

            return "";
        }

        internal string libAnalysis(string name, bool url) => libAnalysis(name, url, "");

        internal string libAnalysis(string name, bool url, string natives)
        {
            string str;
            var tools = new Tools();

            if (url) str = "/";
            else str = @"\";

            char[] separator = new char[] { ':' };
            string[] strArray = name.Split(separator);
            char[] chArray2 = new char[] { '.' };
            strArray[0].Split(chArray2);

            if (natives == "")
            {
                string[] textArray1 = new string[] { strArray[0].Replace('.', Convert.ToChar(str)), str, strArray[1], str, strArray[2], str, strArray[1], "-", strArray[2], ".jar" };
                return string.Concat(textArray1);
            }

            try
            {
                if (natives.IndexOf("${arch}") != -1)
                {
                    object[] objArray1 = new object[] { strArray[0].Replace('.', Convert.ToChar(str)), str, strArray[1], str, strArray[2], str, strArray[1], "-", strArray[2], "-natives-windows-", tools.GetOSBit(), ".jar" };
                    return string.Concat(objArray1);
                }

                string[] textArray2 = new string[] { strArray[0].Replace('.', Convert.ToChar(str)), str, strArray[1], str, strArray[2], str, strArray[1], "-", strArray[2], "-natives-windows.jar" };
                return string.Concat(textArray2);
            }
            catch (ArgumentNullException)
            {
            }

            string[] textArray3 = new string[] { strArray[0].Replace('.', Convert.ToChar(str)), str, strArray[1], str, strArray[2], str, strArray[1], "-", strArray[2], ".jar" };
            return string.Concat(textArray3);
        }

        internal async void liKeep(string version) => await new Tools().liteloaderInstall(version);

        internal void MCVersion()
        {
            if (!pz)
            {
                Control.CheckForIllegalCrossThreadCalls = false;

                if (FileExist(@".minecraft\version.Sika") != null)
                {
                    mcversion1();
                }

                List<mc> list = new List<mc>();
                var file = GetFile(@".minecraft\version.Sika");
                char[] separator = new char[] { '|' };
                string[] strArray = file.Split(separator);

                for (int i = 0; i < strArray.Length; i++)
                {
                    char[] chArray2 = new char[] { '&' };
                    string[] strArray2 = strArray[i].Split(chArray2);

                    var item = new mc
                    {
                        version = strArray2[0],
                        url = strArray2[1]
                    };

                    list.Add(item);
                }

                string str2 = null;
                var str3 = web.getHtml("https://launchermeta.mojang.com/mc/game/version_manifest.json");

                if (str3 != null)
                {
                    var root = JsonConvert.DeserializeObject<mcweb.Root>(str3);

                    for (int j = 0; j < root.versions.ToArray().Length; j++)
                    {
                        if (root.versions[j].id == list[0].version)
                        {
                            SetFile(".minecraft");
                            wj(@".minecraft\version.Sika", str2 + file);
                            Console.WriteLine("后台爬虫完成");
                            pz = true;
                            return;
                        }

                        var str4 = web.getHtml(root.versions[j].url);

                        if (str4 != null)
                        {
                            var root2 = JsonConvert.DeserializeObject<ForgeY.Root>(str4);
                            string[] textArray1 = new string[] { str2, root2.id, "&", root2.downloads.client.url, "|" };
                            str2 = string.Concat(textArray1);
                        }
                        else
                        {
                            mcversion1();
                            return;
                        }
                    }
                }
            }
        }

        internal void mcversion1()
        {
            SetFile(".minecraft");
            wj(@".minecraft\version.Sika", "1.14.4-pre7&https://launcher.mojang.com/v1/objects/53bf70e446ecfb4a88a7546ab3479c4cc868e143/client.jar|1.14.4-pre6&https://launcher.mojang.com/v1/objects/338cd13bb237252c59b59043b49340e545fa1722/client.jar|1.14.4-pre5&https://launcher.mojang.com/v1/objects/db91103c10795811477ec33589b2e1cf452f43f2/client.jar|1.14.4-pre4&https://launcher.mojang.com/v1/objects/ee6386897fd6d7f777d4159fd45b26deaff14cff/client.jar|1.14.4-pre3&https://launcher.mojang.com/v1/objects/4e7e41622e1fb083e093b071396dad50168c9613/client.jar|1.14.4-pre2&https://launcher.mojang.com/v1/objects/96f1d6989a86f1bdbf4cc1583a88e6a16f985d17/client.jar|1.14.4-pre1&https://launcher.mojang.com/v1/objects/bcabf6e9e9664796bd97e01c54d1dbf27aa47c39/client.jar|1.14.3&https://launcher.mojang.com/v1/objects/af100b34ec7ef2b8b9cf7775b544d21d690dddec/client.jar|1.14.3-pre4&https://launcher.mojang.com/v1/objects/7f35f02e03ad1b837d0302c874e8cbc662bf1b88/client.jar|1.14.3-pre3&https://launcher.mojang.com/v1/objects/360c54d41522541f6eb303771192433de4ea1235/client.jar|1.14.3-pre2&https://launcher.mojang.com/v1/objects/a9358d6b2ac6025923155b46dc26cc74523ac130/client.jar|1.14.3-pre1&https://launcher.mojang.com/v1/objects/c49ca8d28e3c64d39dc46d21dd92f421a34ec241/client.jar|1.14.2&https://launcher.mojang.com/v1/objects/ca6c5a139045967229975c0c0b7f93e78b4314c2/client.jar|1.14.2 Pre-Release 4&https://launcher.mojang.com/v1/objects/60783b7bab4125d55f187c859e4b41418ebc8719/client.jar|1.14.2 Pre-Release 3&https://launcher.mojang.com/v1/objects/e1490260d733b18dfc573ab181f9f2df8fb34303/client.jar|1.14.2 Pre-Release 2&https://launcher.mojang.com/v1/objects/d12a34d8584f5465a4851f77bc5a1ce05ac9d59c/client.jar|1.14.2 Pre-Release 1&https://launcher.mojang.com/v1/objects/b1802f2bbe126d3b9a524a187eab1be3f346eb1c/client.jar|1.14.1&https://launcher.mojang.com/v1/objects/55ba86ddcbc3579397f41910463ffd4056e1e523/client.jar|1.14.1 Pre-Release 2&https://launcher.mojang.com/v1/objects/4074da2da9e8207c022e5e12355d4fe87063b86b/client.jar|1.14.1 Pre-Release 1&https://launcher.mojang.com/v1/objects/bf5ada613755329f9d194c1e8a5b26b6bbf30cca/client.jar|1.14&https://launcher.mojang.com/v1/objects/7a762a59345c13af7d87111207a93f5a8607f6c0/client.jar|1.14 Pre-Release 5&https://launcher.mojang.com/v1/objects/3e61d082391ad8d25c40d5825cae8843cfeaf579/client.jar|1.14 Pre-Release 4&https://launcher.mojang.com/v1/objects/9d2f8dd80ddc2008ed87681186af5321cdb6e560/client.jar|1.14 Pre-Release 3&https://launcher.mojang.com/v1/objects/12f85e0b940f3649c9ecb5a3201811f56992e5c0/client.jar|1.14 Pre-Release 2&https://launcher.mojang.com/v1/objects/7d41669ddd780baecab2926d6ea6b08dac7d834f/client.jar|1.14 Pre-Release 1&https://launcher.mojang.com/v1/objects/eb144c0dbc03116c832cdd20d28a8a686327905a/client.jar|19w14b&https://launcher.mojang.com/v1/objects/a008dc3c45cb4f120ae5e0f124d5af2de1bafed5/client.jar|19w14a&https://launcher.mojang.com/v1/objects/d55493da62562de4c8005f6dd117adf78b287658/client.jar|3D Shareware v1.34&https://launcher.mojang.com/v1/objects/44db7d7bcd5a1bee6f54f6a623f26a1b3d1e293f/client.jar|19w13b&https://launcher.mojang.com/v1/objects/bf33e377d07eadd8989570e24d9fc484aa39c81f/client.jar|19w13a&https://launcher.mojang.com/v1/objects/dac80b7f57dee4b0deaa6c78821908c5a5cc7071/client.jar|19w12b&https://launcher.mojang.com/v1/objects/2fc0501773a90d120c9f6476037c4bfba554a549/client.jar|19w12a&https://launcher.mojang.com/v1/objects/15ee2d37e5ec79dbf51aa39b000616be94033d6a/client.jar|19w11b&https://launcher.mojang.com/v1/objects/fba44eb056f80480832482cf7788f725a727e3c3/client.jar|19w11a&https://launcher.mojang.com/v1/objects/03e795efb9c91f2c4994826c45f3a99d2c695517/client.jar|19w09a&https://launcher.mojang.com/v1/objects/8ea094f62c14d184bed801b9d7eb06120ffbd2b6/client.jar|19w08b&https://launcher.mojang.com/v1/objects/cc17b46768c36095f2bf2f621bea12dc307b57d0/client.jar|19w08a&https://launcher.mojang.com/v1/objects/8107282674163f5bb025548fef3ec26d6c5cd6ba/client.jar|19w07a&https://launcher.mojang.com/v1/objects/656fcedd90336e82678d260e4fa20df958926474/client.jar|19w06a&https://launcher.mojang.com/v1/objects/10c8f97e8d9d32a6d0a951a7db6f51818d5d258a/client.jar|19w05a&https://launcher.mojang.com/v1/objects/26f8cdc6354884a4564a6d4e9dceb4e4440dfb54/client.jar|19w04b&https://launcher.mojang.com/v1/objects/1db2550289499e9611fb03744a63adc50d8ac811/client.jar|19w04a&https://launcher.mojang.com/v1/objects/4a075e5ceae3fc15efd36222daeb832c94e16946/client.jar|19w03c&https://launcher.mojang.com/v1/objects/332bdc5f0f0505d86f1f7e5b0f5c7693b494a830/client.jar|19w03b&https://launcher.mojang.com/v1/objects/c23a6e04cc6dc2c3a139d80d17e005599e2243eb/client.jar|19w03a&https://launcher.mojang.com/v1/objects/7280ee42cce1c0371c96c95c22170d3aea7068bb/client.jar|19w02a&https://launcher.mojang.com/v1/objects/8664f5d1b428d5ba8a936ab9c097cc78821d06e6/client.jar|18w50a&https://launcher.mojang.com/v1/objects/865a610fe77eb9d2fea48de1a02229526a391249/client.jar|18w49a&https://launcher.mojang.com/v1/objects/2a0b27ad8bd6cd2bb09e3a2210170fd0b5424c54/client.jar|18w48b&https://launcher.mojang.com/v1/objects/4b1dc59156a888b2e20bbcbfdd5704fb4e7f6789/client.jar|18w48a&https://launcher.mojang.com/v1/objects/8c2c778a22201836bf482a8ed06e1b1f994c61e3/client.jar|18w47b&https://launcher.mojang.com/v1/objects/1174ad74afce51a9309f1293af5bf24f60609cee/client.jar|18w47a&https://launcher.mojang.com/v1/objects/1cac5e82265eef3a7670c8752f31f02c65116aef/client.jar|18w46a&https://launcher.mojang.com/v1/objects/342ece78f131c72c4669c9aa27a7be542b911171/client.jar|18w45a&https://launcher.mojang.com/v1/objects/e03e8d27b07cccc85ae669e666bc3e4e2036c374/client.jar|18w44a&https://launcher.mojang.com/v1/objects/20ea61f07e34b3e81ca356b6a71cc98cc4e571a9/client.jar|18w43c&https://launcher.mojang.com/v1/objects/a040369aa294a5a42048d828b03592b54db6a8d3/client.jar|18w43b&https://launcher.mojang.com/v1/objects/0b93502884da84e9d60532729eca59b70c96f281/client.jar|18w43a&https://launcher.mojang.com/v1/objects/6f0fa604e9c8b1996c985aceb9b589fa4e583671/client.jar|1.13.2&https://launcher.mojang.com/v1/objects/30bfe37a8db404db11c7edf02cb5165817afb4d9/client.jar|1.13.2-pre2&https://launcher.mojang.com/v1/objects/3ad1375091d9de67beb3197dcd173d05ff27dd0b/client.jar|1.13.2-pre1&https://launcher.mojang.com/v1/objects/843bc6377ff859b7744d12df0b62653dc318456b/client.jar|1.13.1&https://launcher.mojang.com/v1/objects/8de235e5ec3a7fce168056ea395d21cbdec18d7c/client.jar|1.13.1-pre2&https://launcher.mojang.com/v1/objects/7eaeaa37e0c7642d519c39de70a630119ad4929a/client.jar|1.13.1-pre1&https://launcher.mojang.com/v1/objects/7f7ae07cc319346b632f6c9f26ff8c67728b203c/client.jar|18w33a&https://launcher.mojang.com/v1/objects/b8fc21c1446df367ac02946f1b475bd72df84dd6/client.jar|18w32a&https://launcher.mojang.com/v1/objects/f4f53276150e295afc8cd300d87568f279607bfb/client.jar|18w31a&https://launcher.mojang.com/v1/objects/0a343ef39365d70908a3c59fbbf2d9c0ddd2ad75/client.jar|18w30b&https://launcher.mojang.com/v1/objects/e6bfb0dc26e29d80efb137c8e9359fd3c324cf1a/client.jar|18w30a&https://launcher.mojang.com/v1/objects/d368609e838cb1b22e1e1e9eee8f83dbe4847909/client.jar|1.13&https://launcher.mojang.com/v1/objects/c0b970952cdd279912da384cdbfc0c26e6c6090b/client.jar|1.13-pre10&https://launcher.mojang.com/v1/objects/72c4fbff2a1318a57bfb66e325055064a13378a8/client.jar|1.13-pre9&https://launcher.mojang.com/v1/objects/4dedcb718a3382496d19d13cfe5dc070528a15cd/client.jar|1.13-pre8&https://launcher.mojang.com/v1/objects/fbece4a24e47af57c3ee75e331f9390309f92ae5/client.jar|1.13-pre7&https://launcher.mojang.com/v1/objects/0b5d9df7bc2d0e4fd00d0bf7cf4409b999567497/client.jar|1.13-pre6&https://launcher.mojang.com/v1/objects/9405570c459c0803da2754b34e5ffeb74413a904/client.jar|1.13-pre5&https://launcher.mojang.com/v1/objects/f3262055c586a075fc84f9d4bc76b3cf1a72d69c/client.jar|1.13-pre4&https://launcher.mojang.com/v1/objects/c4a93fea1ea2a1a7886c8f5f66f6d929db53f021/client.jar|1.13-pre3&https://launcher.mojang.com/v1/objects/94f2e86f94d7d80c19ec1d3d637b1ef2d862be9e/client.jar|1.13-pre2&https://launcher.mojang.com/v1/objects/b833d32e1846989a61c6c3faa40232bb72bd59de/client.jar|1.13-pre1&https://launcher.mojang.com/v1/objects/f9b3302a997e52af71efc3904d805957430e4820/client.jar|18w22c&https://launcher.mojang.com/v1/objects/ec446c24c4842f6237ba7c560a5b0dae9ac87c22/client.jar|18w22b&https://launcher.mojang.com/v1/objects/a7db66a86e6696aef0fbfbf813293ff1aa01a64a/client.jar|18w22a&https://launcher.mojang.com/v1/objects/0a864719da82dd91bf5d7031cf9c40e5ec3dbfd7/client.jar|18w21b&https://launcher.mojang.com/v1/objects/77b9304fd8e97953de4c6334f56abe4bc2661fe1/client.jar|18w21a&https://launcher.mojang.com/v1/objects/9b1d1486518585537a0d9e608dd27e3946880d48/client.jar|18w20c&https://launcher.mojang.com/v1/objects/273595b95fff5080b74cefe18ce0c747d02663ed/client.jar|18w20b&https://launcher.mojang.com/v1/objects/91aa79a2aa3c656221defebf5310c1ffacfd81ed/client.jar|18w20a&https://launcher.mojang.com/v1/objects/89843d397a9b0fd42bea26ada6890ebc693ad288/client.jar|18w19b&https://launcher.mojang.com/v1/objects/6e5306869f9644e0a91f23345b4f445742daf5cc/client.jar|18w19a&https://launcher.mojang.com/v1/objects/d52b2e8f15a764000c1ca6dabab4440069ff97a4/client.jar|18w16a&https://launcher.mojang.com/v1/objects/8787bc29f2a1e151123f70e21698af23374d1b08/client.jar|18w15a&https://launcher.mojang.com/v1/objects/ba02c440e50e0197a13cef03ca2356c8cc51f058/client.jar|18w14b&https://launcher.mojang.com/v1/objects/d9b4fc98eb5242346ed0421f6ccd3c9032dc0514/client.jar|18w14a&https://launcher.mojang.com/v1/objects/73885c6bf4fdcca8ab1ca22b56f69c9945725770/client.jar|18w11a&https://launcher.mojang.com/v1/objects/e4abfe0c5e7f490a1071e7c99cb1cef81ffd89dc/client.jar|18w10d&https://launcher.mojang.com/v1/objects/062c014c0e8684ca554d3abbcfc31e3e56334a2a/client.jar|18w10c&https://launcher.mojang.com/v1/objects/cec68c82ce5824f74069e8ba84871786077da6ab/client.jar|18w10b&https://launcher.mojang.com/v1/objects/383e0396f585fcc5a487f04fcb77a9743a0e44c3/client.jar|18w10a&https://launcher.mojang.com/v1/objects/fb24610e6fca6f83e8b45e3a46224601d4ca6c27/client.jar|18w09a&https://launcher.mojang.com/v1/objects/701085c1b668d45b7c0ee5ea911b11d6691c01bc/client.jar|18w08b&https://launcher.mojang.com/v1/objects/665fd71175823d517cee81154b374bb27ac5c070/client.jar|18w08a&https://launcher.mojang.com/v1/objects/52cacb4730ddee013b67e10e49b99955b4ec10ea/client.jar|18w07c&https://launcher.mojang.com/v1/objects/3d5865b5bf013f84e44c3f096193aa45c672bb51/client.jar|18w07b&https://launcher.mojang.com/v1/objects/e1aeafe25aa454e35e53bd489523bed51aa5e826/client.jar|18w07a&https://launcher.mojang.com/v1/objects/fa2960642589fc7bd3e14049ffe7d84a4d2bac1d/client.jar|18w06a&https://launcher.mojang.com/v1/objects/78ed8d3359453614d8f69bd982111e8aa6c8f612/client.jar|18w05a&https://launcher.mojang.com/v1/objects/bc3c321006bd1864a9e3dfea8c351927c44519c6/client.jar|18w03b&https://launcher.mojang.com/v1/objects/39b958dd8e4ab25c6b6522ce9c83d4bd45173738/client.jar|18w03a&https://launcher.mojang.com/v1/objects/b9e9cfa4e6019fa1d4aa7b368366b9d8979db2a1/client.jar|18w02a&https://launcher.mojang.com/v1/objects/05932046cbefd34e42ba21530b96986435163bda/client.jar|18w01a&https://launcher.mojang.com/v1/objects/504f07f4a10f295522a57f2b1163298dc4a63103/client.jar|17w50a&https://launcher.mojang.com/v1/objects/ebb9600d0f33b6ffd1dec8fc3010677dc07accbc/client.jar|17w49b&https://launcher.mojang.com/v1/objects/bb28e11e5b6ca4c0678d00bf82d061d282ab6445/client.jar|17w49a&https://launcher.mojang.com/v1/objects/a58ce444458ba9267973c31877a57eac5a5cbeda/client.jar|17w48a&https://launcher.mojang.com/v1/objects/1b31d5152548eeb7dfa94cdeafd8d9321de8f8c3/client.jar|17w47b&https://launcher.mojang.com/v1/objects/049d118c3fc0d3fad544aaf387ca6c5cb2cf4e00/client.jar|17w47a&https://launcher.mojang.com/v1/objects/4485b04ad6358eef525aeeb246fe2b6b23d232ee/client.jar|17w46a&https://launcher.mojang.com/v1/objects/fd90b150fb461573b121a7e6b6de657ba382f14d/client.jar|17w45b&https://launcher.mojang.com/v1/objects/83e5c3728b473a8d5afda12906a3e581888489fd/client.jar|17w45a&https://launcher.mojang.com/v1/objects/42c93d04a82371094d9a4a844c932b8db45d3a40/client.jar|17w43b&https://launcher.mojang.com/v1/objects/d4316c6f1dda3211e8c990259d9be93fa1517055/client.jar|17w43a&https://launcher.mojang.com/v1/objects/e37ed76424b6d3f5ff08842ee2aec35619a00e99/client.jar|1.12.2&https://launcher.mojang.com/v1/objects/0f275bc1547d01fa5f56ba34bdc87d981ee12daf/client.jar|1.12.2-pre2&https://launcher.mojang.com/v1/objects/404877dbb91887a2b481972912f82e98c55cdea7/client.jar|1.12.2-pre1&https://launcher.mojang.com/v1/objects/97d35764e0cd59b163c765d422a8d7af919bb705/client.jar|1.12.1&https://launcher.mojang.com/v1/objects/83385d346cf7f97d1e447b888750fb88e9928b93/client.jar|1.12.1-pre1&https://launcher.mojang.com/v1/objects/c1448a29036f20c03557720eb1754feab15e7f5d/client.jar|17w31a&https://launcher.mojang.com/v1/objects/67bfebbd0af5e7b7b4d089a7e9b63dbed2eeacd7/client.jar|1.12&https://launcher.mojang.com/v1/objects/909823f9c467f9934687f136bc95a667a0d19d7f/client.jar|1.12-pre7&https://launcher.mojang.com/v1/objects/2439b69adbb6d5f2e8fee5145084603e033007c7/client.jar|1.12-pre6&https://launcher.mojang.com/v1/objects/12e23c0811c097ed63e50fad861fea297e1ae0be/client.jar|1.12-pre5&https://launcher.mojang.com/v1/objects/87f980355264f222daa292528116281d62231402/client.jar|1.12-pre4&https://launcher.mojang.com/v1/objects/a3c2de97aa317cc3d71614689d14592d555e02bd/client.jar|1.12-pre3&https://launcher.mojang.com/v1/objects/8929ff4a80fae0cf26e24bb05c6706f4d7638fdd/client.jar|1.12-pre2&https://launcher.mojang.com/v1/objects/16c7be111b1efe29f163abc16426705be51ca7b7/client.jar|1.12-pre1&https://launcher.mojang.com/v1/objects/451d983529e78b807c8f8479f7f542863b1b6ae0/client.jar|17w18b&https://launcher.mojang.com/v1/objects/d10320fe0a9bf0e86bbea792216bb0da4430b17a/client.jar|17w18a&https://launcher.mojang.com/v1/objects/9eda8162e92b2db820a140f4b4d8fc7b70d32b3a/client.jar|17w17b&https://launcher.mojang.com/v1/objects/37e3f7d47f6f528455bda16684ff00308f375b1f/client.jar|17w17a&https://launcher.mojang.com/v1/objects/fa78ab696aa02914171deb57e31cfb737506f272/client.jar|17w16b&https://launcher.mojang.com/v1/objects/339b2099d66421af8f194e76074ebda5694abdb3/client.jar|17w16a&https://launcher.mojang.com/v1/objects/2b7de5e297fda9fc35500c6db18627be00ad003c/client.jar|17w15a&https://launcher.mojang.com/v1/objects/243f42af3ecff89646d001d781ed42c7c736c74d/client.jar|17w14a&https://launcher.mojang.com/v1/objects/23500be7cee6c8a4519d475a7c8f2b8493a3336f/client.jar|17w13b&https://launcher.mojang.com/v1/objects/7104e16e9e02af09cee90cc95487502641e5bbbf/client.jar|17w13a&https://launcher.mojang.com/v1/objects/8a11edbdd3d8be019b3eb85089f2f5d97ec3ac4e/client.jar|17w06a&https://launcher.mojang.com/v1/objects/ace57ac6f4a661fc095241004c46d1305c573bc1/client.jar|1.11.2&https://launcher.mojang.com/v1/objects/db5aa600f0b0bf508aaf579509b345c4e34087be/client.jar|1.11.1&https://launcher.mojang.com/v1/objects/0935745a11806d6b4cf00221938e7560c23f9291/client.jar|16w50a&https://launcher.mojang.com/v1/objects/925650c3ed9e1e79dd3f846a49a9de3a7f8e700c/client.jar|1.11&https://launcher.mojang.com/v1/objects/780e46b3a96091a7f42c028c615af45974629072/client.jar|1.11-pre1&https://launcher.mojang.com/v1/objects/fdc07c099ad33f77c426464754841747a09ee8d7/client.jar|16w44a&https://launcher.mojang.com/v1/objects/fa3d0b4d577f475534e600a0ee1a62fb08ca29dc/client.jar|16w43a&https://launcher.mojang.com/v1/objects/70449cf6d3f1f068ef7269d49ccdecd18e3a4712/client.jar|16w42a&https://launcher.mojang.com/v1/objects/3742e2ccb78ef02f1820f7dc73f5aafcf8c213a8/client.jar|16w41a&https://launcher.mojang.com/v1/objects/89c23ff48bce1df436919147044be1668270e60b/client.jar|16w40a&https://launcher.mojang.com/v1/objects/6e5f07830bbd10ab10b6cccdaaef17c6b589a6d7/client.jar|16w39c&https://launcher.mojang.com/v1/objects/417fe811581f045d4f5de0381b08cd7dc3744c41/client.jar|16w39b&https://launcher.mojang.com/v1/objects/f04ca007b45c3fed9ba03c5ada8926320a4ddbd5/client.jar|16w39a&https://launcher.mojang.com/v1/objects/ebfc3681ad92d9d200232f913645411728f45dc6/client.jar|16w38a&https://launcher.mojang.com/v1/objects/e249ff8eded25695b921ccef066bc8a5db154a6d/client.jar|16w36a&https://launcher.mojang.com/v1/objects/f333065b34c818493736a90d5f559ee702957bd3/client.jar|16w35a&https://launcher.mojang.com/v1/objects/96bf2617491528e0a4d636fa08afe3c8c14282c7/client.jar|16w33a&https://launcher.mojang.com/v1/objects/e565f6a8dacc85b81faaf17e4a13125b2ad5c335/client.jar|16w32b&https://launcher.mojang.com/v1/objects/71afbf19a5e9d37cceb449743323acbb17895dae/client.jar|16w32a&https://launcher.mojang.com/v1/objects/198124cb36e4284feec3204f8e20f14f6a531cc3/client.jar|1.10.2&https://launcher.mojang.com/v1/objects/dc8e75ac7274ff6af462b0dcec43c307de668e40/client.jar|1.10.1&https://launcher.mojang.com/v1/objects/44b389fff90324c4ca18796d4428a7b8ec6c2eb0/client.jar|1.10&https://launcher.mojang.com/v1/objects/ba038efbc6d9e4a046927a7658413d0276895739/client.jar|1.10-pre2&https://launcher.mojang.com/v1/objects/c08e980eb1d79405c9213717df4cd11e509b0761/client.jar|1.10-pre1&https://launcher.mojang.com/v1/objects/33c87d0abcd90cfc694ef651e96e81689e14cffb/client.jar|16w21b&https://launcher.mojang.com/v1/objects/e0dd76d667fec04c875f3e8b6e9465c3f03da2ef/client.jar|16w21a&https://launcher.mojang.com/v1/objects/a48f966d00c5d9bf300c2b8407efb98df743bde8/client.jar|16w20a&https://launcher.mojang.com/v1/objects/73de5728b610cf31bcc9c497447856374893249d/client.jar|1.9.4&https://launcher.mojang.com/v1/objects/4a61c873be90bb1196d68dac7b29870408c56969/client.jar|1.9.3&https://launcher.mojang.com/v1/objects/b6985b0d3a0520dfb6f17eeb1e8ba58ce9577061/client.jar|1.9.3-pre3&https://launcher.mojang.com/v1/objects/364766592529c4a74397ea33ef1f10eb01cad872/client.jar|1.9.3-pre2&https://launcher.mojang.com/v1/objects/9f0ed8007fee5763a1d8c18e2e51eedb855b3e55/client.jar|1.9.3-pre1&https://launcher.mojang.com/v1/objects/ae942605669e2d25ab539b8c2ea45d469b022bea/client.jar|16w15b&https://launcher.mojang.com/v1/objects/7500b534d32a89f4e8de937323aabe6fd9778ea2/client.jar|16w15a&https://launcher.mojang.com/v1/objects/042a78b65cfd53a0dbf102bba24dfc7ec295905b/client.jar|16w14a&https://launcher.mojang.com/v1/objects/ac59f57ecf383113e3dd92dd90d2fd7b391252ef/client.jar|1.RV-Pre1&https://launcher.mojang.com/v1/objects/3843fae71dd283e68897ead618255fa1ddcf4c8d/client.jar|1.9.2&https://launcher.mojang.com/v1/objects/19106fd5e222dca0f2dde9f66db8384c9a7db957/client.jar|1.9.1&https://launcher.mojang.com/v1/objects/9bc7f02323d90b9385c1a5dbd47fb144a3fb8835/client.jar|1.9.1-pre3&https://launcher.mojang.com/v1/objects/54ad739a28758d411492aa2aa562d6604ba3227e/client.jar|1.9.1-pre2&https://launcher.mojang.com/v1/objects/3b1cd2f3720a2b4c48eb8b1d2505c875fb6c78d5/client.jar|1.9.1-pre1&https://launcher.mojang.com/v1/objects/23386d5d39a8376ee23e61d65f27fb52ed5bee2b/client.jar|1.9&https://launcher.mojang.com/v1/objects/2f67dfe8953299440d1902f9124f0f2c3a2c940f/client.jar|1.9-pre4&https://launcher.mojang.com/v1/objects/f5dc0169eb605cf06aa6db60a0a164c9c5009554/client.jar|1.9-pre3&https://launcher.mojang.com/v1/objects/81b626ebd0efa06f07b3f0dec1af34989ab61fd0/client.jar|1.9-pre2&https://launcher.mojang.com/v1/objects/c61b03c6a0cdc7ee87f2bc0b707ce27ded2fa066/client.jar|1.9-pre1&https://launcher.mojang.com/v1/objects/fa13948aa05aaa99b5f9d1700bbbdb2b6ecc59b1/client.jar|16w07b&https://launcher.mojang.com/v1/objects/5c048a4a9998e2efc05d3d46675be6ec43c7f28e/client.jar|16w07a&https://launcher.mojang.com/v1/objects/7dc58069a02ea8cdce3a8394aec8f33b5885cc11/client.jar|16w06a&https://launcher.mojang.com/v1/objects/544342f959159d63da205b23947ee9c10b73045c/client.jar|16w05b&https://launcher.mojang.com/v1/objects/3a07cea3cf6f1198a7db39a8bd3775883fb391be/client.jar|16w05a&https://launcher.mojang.com/v1/objects/21633205eb7bdbc2a0d2da901cf8e96ed5265650/client.jar|16w04a&https://launcher.mojang.com/v1/objects/1bb971fd5f636f4dca76410c1abd02abc1b32101/client.jar|16w03a&https://launcher.mojang.com/v1/objects/d8072c24af1c48dc7a206bbef74f342b4f48f038/client.jar|16w02a&https://launcher.mojang.com/v1/objects/efdeb1a9736db56e5ff319bdf62dfa6fa6395bee/client.jar|15w51b&https://launcher.mojang.com/v1/objects/001fafaef03804e8220367e1344b12fb596be5fd/client.jar|15w51a&https://launcher.mojang.com/v1/objects/764384c71c303b3e18146dd90ac8cad2550b6ffb/client.jar|15w50a&https://launcher.mojang.com/v1/objects/51c42f7a28ba70acd957c3963d5b3ad7da5f7ec1/client.jar|15w49b&https://launcher.mojang.com/v1/objects/52b24b872a1280023f2d9dc44a70ad9a052a11f1/client.jar|1.8.9&https://launcher.mojang.com/v1/objects/3870888a6c3d349d3771a3e9d16c9bf5e076b908/client.jar|15w49a&https://launcher.mojang.com/v1/objects/16e868a098239c16ecde3f06865eaf1995c11445/client.jar|15w47c&https://launcher.mojang.com/v1/objects/4fa6325dbcf710d3dbfd707acd4a541b9c30638d/client.jar|15w47b&https://launcher.mojang.com/v1/objects/517438322f119b3bec2490aada10ec3fcd2e8774/client.jar|15w47a&https://launcher.mojang.com/v1/objects/6af473660604bf20c99c2b8f04002666f0bcc53d/client.jar|15w46a&https://launcher.mojang.com/v1/objects/101e4c37464438ef7fe9dc12113a3ea828eac4f0/client.jar|15w45a&https://launcher.mojang.com/v1/objects/1cc965816c3c01b3b2226e15e884b74f3b04dd10/client.jar|15w44b&https://launcher.mojang.com/v1/objects/cfc7eba7e23bf76c81facf1ebf9dce7215c3141d/client.jar|15w44a&https://launcher.mojang.com/v1/objects/ab3e8b04d85b873a028ba7fd49a325c61d4ab360/client.jar|15w43c&https://launcher.mojang.com/v1/objects/f0437e3ac44089f3e604308041179ab2d3ed639b/client.jar|15w43b&https://launcher.mojang.com/v1/objects/fb00ac4550199a99a3976f185309fcfe7e1ed1a6/client.jar|15w43a&https://launcher.mojang.com/v1/objects/c7869840cb9c9acd152c0b8aafe74bef191a9d44/client.jar|15w42a&https://launcher.mojang.com/v1/objects/8205907ebd8bbb4e328e0db193ffe4b1727d191a/client.jar|15w41b&https://launcher.mojang.com/v1/objects/83b7a7c64fb3fd11c2923bda1574276c1bcdded7/client.jar|15w41a&https://launcher.mojang.com/v1/objects/edcecdfe258c36fcc857dfce1ee40167587d250d/client.jar|15w40b&https://launcher.mojang.com/v1/objects/2d0a8a24c90034d529277e20414080952a6c7e7b/client.jar|15w40a&https://launcher.mojang.com/v1/objects/582827b571e0039358b75e8552b9491c24737e5b/client.jar|15w39c&https://launcher.mojang.com/v1/objects/101e82c68b63be1f8c2dfc1bddcdb4a196442a84/client.jar|15w39b&https://launcher.mojang.com/v1/objects/39da2b441cd74782e6ee537dba7168ee9d483ab0/client.jar|15w39a&https://launcher.mojang.com/v1/objects/5a26b65ccea3c436a56673e3d59ddd1bc901a827/client.jar|15w38b&https://launcher.mojang.com/v1/objects/acf1e36ee1c02249a1e5f5927b0b5b2a52e882b9/client.jar|15w38a&https://launcher.mojang.com/v1/objects/befc74213863009297e81873644a48c52d94fde2/client.jar|15w37a&https://launcher.mojang.com/v1/objects/0480d8e610a57fe18c78cb40eb76be50469accee/client.jar|15w36d&https://launcher.mojang.com/v1/objects/ea3bbdb010acdbd558aacd67a48e3de83515b4e8/client.jar|15w36c&https://launcher.mojang.com/v1/objects/7ec3fd61065f2f82dd881b81645b66ce61c6b4b7/client.jar|15w36b&https://launcher.mojang.com/v1/objects/603273cbc881fdc012541e692d78c6f471349f56/client.jar|15w36a&https://launcher.mojang.com/v1/objects/132f019cf1bd9524451c83756440bd71468dac26/client.jar|15w35e&https://launcher.mojang.com/v1/objects/beb13ba79289289e231d0892c4a3a9a72c46f45f/client.jar|15w35d&https://launcher.mojang.com/v1/objects/6f28a53808e5ae69b4538580d35a235bd09eca93/client.jar|15w35c&https://launcher.mojang.com/v1/objects/c5928359f77e74892dd8d496e9de4f1fca6e685d/client.jar|15w35b&https://launcher.mojang.com/v1/objects/2c73745d684db77aa0dc4a85fa96902918c0ddc5/client.jar|15w35a&https://launcher.mojang.com/v1/objects/2880da12d8997ad4c669b7531a09e64c7e038f10/client.jar|15w34d&https://launcher.mojang.com/v1/objects/db8e9c2ed19d783588e9936fcd6a923629e883cd/client.jar|15w34c&https://launcher.mojang.com/v1/objects/afb42058a0f8432bc348071a0e0fe329e108aff4/client.jar|15w34b&https://launcher.mojang.com/v1/objects/60af6ff7ca2aded6080506096143010034aa11ea/client.jar|15w34a&https://launcher.mojang.com/v1/objects/e8320ed660bcdc6c80d94a5c4e068e783465a2f5/client.jar|15w33c&https://launcher.mojang.com/v1/objects/23237d415c9cf637a261e02f6759f4ab1d553f52/client.jar|15w33b&https://launcher.mojang.com/v1/objects/502082050bf59e72332afe508f8c19b379173551/client.jar|15w33a&https://launcher.mojang.com/v1/objects/337da43d1f130f57b69f295ad8a24b6851e111bf/client.jar|15w32c&https://launcher.mojang.com/v1/objects/160d90035f2032db8bced2c1490b7c47d274c382/client.jar|15w32b&https://launcher.mojang.com/v1/objects/1f448b05e56d3b36f0408eecd798b875ec230ef0/client.jar|15w32a&https://launcher.mojang.com/v1/objects/6f85d469a2f6e8d24cc71f1882eba1142ecc29bd/client.jar|15w31c&https://launcher.mojang.com/v1/objects/a903feeba306b3e2415ec97757a9a32b4a27859d/client.jar|15w31b&https://launcher.mojang.com/v1/objects/25f9d73567cb53e3d62675c297946ba8d47c4282/client.jar|15w31a&https://launcher.mojang.com/v1/objects/d702effa2e2ff9f79ffc74dc6becc3eeca376fee/client.jar|1.8.8&https://launcher.mojang.com/v1/objects/0983f08be6a4e624f5d85689d1aca869ed99c738/client.jar|1.8.7&https://launcher.mojang.com/v1/objects/d546a6a092060c85f1eb1d9213ff823c558b1255/client.jar|1.8.6&https://launcher.mojang.com/v1/objects/faa55e34ded35089d34fe921ea83d317fc152e93/client.jar|1.8.5&https://launcher.mojang.com/v1/objects/5d39957c61d19042f8bd669a6faa99989cf37083/client.jar|1.8.4&https://launcher.mojang.com/v1/objects/459204fabee3fd9976a6c942b24cbd8382d02d6e/client.jar|15w14a&https://launcher.mojang.com/v1/objects/1bea9340956b96f2b5452aa576e0cf460990efc6/client.jar|1.8.3&https://launcher.mojang.com/v1/objects/69d14463ddc22e581bc66c66ef5eb72a8b452c46/client.jar|1.8.2&https://launcher.mojang.com/v1/objects/a8e71f6c81acfa834d249579a242f5b0852075c1/client.jar|1.8.2-pre7&https://launcher.mojang.com/v1/objects/07e1062c3fadbf70b10a819bf5c3a9a12339be9f/client.jar|1.8.2-pre6&https://launcher.mojang.com/v1/objects/319e3ec8671db1bdafee732f27206f696a162e6d/client.jar|1.8.2-pre5&https://launcher.mojang.com/v1/objects/e94d11dc1b1174da7aa8341925de78a898738f5a/client.jar|1.8.2-pre4&https://launcher.mojang.com/v1/objects/b0dcb6a181d2977f88854db582d3972f34b09880/client.jar|1.8.2-pre3&https://launcher.mojang.com/v1/objects/672b075aa0fc7d4761aebc1ad68dc2c95e764f42/client.jar|1.8.2-pre2&https://launcher.mojang.com/v1/objects/153aa945c44ff00502d96b68ff5e0941794e39d9/client.jar|1.8.2-pre1&https://launcher.mojang.com/v1/objects/b4be275f3854908c5ed503f25a0f225b798253d1/client.jar|1.8.1&https://launcher.mojang.com/v1/objects/6edd2a3e3d76a7602c52f319f87cf11f7720b43b/client.jar|1.8.1-pre5&https://launcher.mojang.com/v1/objects/927b71686a743a5907c9875ffdcfd8ed156273d9/client.jar|1.8.1-pre4&https://launcher.mojang.com/v1/objects/128be924bcf81890ba024f1ddd2a259ff1895f36/client.jar|1.8.1-pre3&https://launcher.mojang.com/v1/objects/022134c463c189cac436a2381cf8be41dd2071fa/client.jar|1.8.1-pre2&https://launcher.mojang.com/v1/objects/e326dea1becc6e1372b05bfe2440531d13f76959/client.jar|1.8.1-pre1&https://launcher.mojang.com/v1/objects/650ed5ac903ef19ca76f0f46d6b6486880488802/client.jar|1.8&https://launcher.mojang.com/v1/objects/d722504db9de2b47f46cc592b8528446272ae648/client.jar|1.8-pre3&https://launcher.mojang.com/v1/objects/228f1f260f34350e37576aeb87fe9cb98b9a5137/client.jar|1.8-pre2&https://launcher.mojang.com/v1/objects/81fb8e9ddceca2cfdf453ff4755d1178b1de482e/client.jar|1.8-pre1&https://launcher.mojang.com/v1/objects/7f943ab9342e7d63d7857486ba964d8c763799cc/client.jar|14w34d&https://launcher.mojang.com/v1/objects/529703e7e0c5875d812e5457133f19f30b0eb9ef/client.jar|14w34c&https://launcher.mojang.com/v1/objects/5fa7f6102fc0f228fd8cfa39b92947b1834f5392/client.jar|14w34b&https://launcher.mojang.com/v1/objects/620147b7a1cf8a4de0c1bea530da621874730bf6/client.jar|14w34a&https://launcher.mojang.com/v1/objects/f17337ce79af702e78eff85a004569603d1e2435/client.jar|14w33c&https://launcher.mojang.com/v1/objects/98301d34581ac9c1274c34c62a01cf1be53ce887/client.jar|14w33b&https://launcher.mojang.com/v1/objects/fd2a3ea5778fff84f8580fd05952f14938351477/client.jar|14w33a&https://launcher.mojang.com/v1/objects/910421082420053a0e4fee72fa94017be6d68dbb/client.jar|14w32d&https://launcher.mojang.com/v1/objects/03188ae34f0fe7e75dd724882140a0cbe7a6e4c1/client.jar|14w32c&https://launcher.mojang.com/v1/objects/15b1ca66ba5fd17b8c76448eb769ea4ea853b06a/client.jar|14w32b&https://launcher.mojang.com/v1/objects/cfd33586f6033c2ddd092db137b83b85cc10733c/client.jar|14w32a&https://launcher.mojang.com/v1/objects/66b8ed45eb1e8d687d269f2f26e4d20edd07333a/client.jar|14w31a&https://launcher.mojang.com/v1/objects/76f1d68d032fdaa228c477fd927dc12745277e8a/client.jar|14w30c&https://launcher.mojang.com/v1/objects/2ab883131b1ee8ef13e945fd6d4fef5b6f2f44ae/client.jar|14w30b&https://launcher.mojang.com/v1/objects/3a0d41e74a18e2c26498a918724ff39d62af329c/client.jar|14w30a&https://launcher.mojang.com/v1/objects/e335e50b527fe15c82242a0a8ce8ad4e5fdcf513/client.jar|14w29b&https://launcher.mojang.com/v1/objects/60c4779dbb9bf70972ad45d204adf8d1c9dd1f0f/client.jar|14w29a&https://launcher.mojang.com/v1/objects/e8775d2214111c9600daffb5fcc15f8e6381f9a8/client.jar|14w28b&https://launcher.mojang.com/v1/objects/b27a6f1882728b61a71d68c9a1c2454ff802a21b/client.jar|14w28a&https://launcher.mojang.com/v1/objects/747bc2c39af6ea3c803709a91b49898466636f5b/client.jar|14w27b&https://launcher.mojang.com/v1/objects/11bed76015aaa2d673132d0c5330d3f14c17a988/client.jar|14w27a&https://launcher.mojang.com/v1/objects/7c9eac4e1b56e6a3583370d1f77111b111f986e5/client.jar|14w26c&https://launcher.mojang.com/v1/objects/c382a4a6f6fca726a398bf648e74c7d12b67b3ff/client.jar|14w26b&https://launcher.mojang.com/v1/objects/6b0d36e0ff0cd61fb2b4ba665e3b5070a2871e0f/client.jar|14w26a&https://launcher.mojang.com/v1/objects/20f0b48333cf446fe77317ab95012c595b65a8d0/client.jar|14w25b&https://launcher.mojang.com/v1/objects/43ddd4b49640512da54df98a33d7f863313993a7/client.jar|14w25a&https://launcher.mojang.com/v1/objects/9bfdb6456e3b9a2d93c5af37862a1174987794bd/client.jar|14w21b&https://launcher.mojang.com/v1/objects/02db91714546d082952fb70bb3e2ff9916135569/client.jar|14w21a&https://launcher.mojang.com/v1/objects/8d764e567c25ab9f5c1f1d5f16c0b673f3f06eb6/client.jar|14w20b&https://launcher.mojang.com/v1/objects/09edbf6233a54fcb30656cb6fd77f55209c37041/client.jar|14w20a&https://launcher.mojang.com/v1/objects/9f3b303c1de10d3229353471e63e834339f3acbe/client.jar|1.7.10&https://launcher.mojang.com/v1/objects/e80d9b3bf5085002218d4be59e668bac718abbc6/client.jar|1.7.10-pre4&https://launcher.mojang.com/v1/objects/fc8b2a30a28bbb45dd8a1b0257f51304c102ad13/client.jar|1.7.10-pre3&https://launcher.mojang.com/v1/objects/9e3d4d71e9827fa2024ec90c065b2120ba80a024/client.jar|1.7.10-pre2&https://launcher.mojang.com/v1/objects/aadaad3fa610c67fd96728fc2dfedd4239896965/client.jar|1.7.10-pre1&https://launcher.mojang.com/v1/objects/06110da00aa3cf4a6856a736422336dcb4640017/client.jar|14w19a&https://launcher.mojang.com/v1/objects/1d498b388c98ec363cfaa19cec3fcc9717d7ac35/client.jar|14w18b&https://launcher.mojang.com/v1/objects/75b92b48b4ae0cf2facd9d4a57ad94b22e608fa8/client.jar|14w18a&https://launcher.mojang.com/v1/objects/9f9457fa624e797ee8b7fd409abd4843548affc5/client.jar|14w17a&https://launcher.mojang.com/v1/objects/f0ffadfeae5064fc64a5e80aa6e6787dae419fc5/client.jar|14w11b&https://launcher.mojang.com/v1/objects/bd8b4cebafe218d14c0d1d9a83c2377e63020804/client.jar|1.7.9&https://launcher.mojang.com/v1/objects/fbbaae784b1de315a8d08a82c6c345a583fb676b/client.jar|1.7.8&https://launcher.mojang.com/v1/objects/83ae44189888a873f46f7509a87ebdb6dc785741/client.jar|1.7.7&https://launcher.mojang.com/v1/objects/e520f254a2b496d61839b816d712e238b1243f30/client.jar|1.7.6&https://launcher.mojang.com/v1/objects/6b2c5827994e17f904f0852f962dd6fccfbb8bdc/client.jar|14w11a&https://launcher.mojang.com/v1/objects/34ce7e9e1529560f8647599840cd33d6f107d6ef/client.jar|1.7.6-pre2&https://launcher.mojang.com/v1/objects/24e28d53aecff54ff9be2a1ba1ffe008b86f7ed6/client.jar|1.7.6-pre1&https://launcher.mojang.com/v1/objects/0a8b12ed910a51bf7099e2e2b3a279ec63073e5e/client.jar|14w10c&https://launcher.mojang.com/v1/objects/53fefd411c245f945c828f35aada4e5c0423d1a2/client.jar|14w10b&https://launcher.mojang.com/v1/objects/9520f823ddc8b0aa600f6cbbb72298d905c4c6ab/client.jar|14w10a&https://launcher.mojang.com/v1/objects/bc38e1cc8db9df50d0b5a1a68b74ec1d19450728/client.jar|14w08a&https://launcher.mojang.com/v1/objects/05b9cde52497d1c419ddece4f16784202e45c0f4/client.jar|1.7.5&https://launcher.mojang.com/v1/objects/77aa70ac5054cbf1140b6449b579e950d5050c4a/client.jar|14w07a&https://launcher.mojang.com/v1/objects/d6a73ccba25f4c30d56af73f43a2d3c9a5458a1e/client.jar|14w06b&https://launcher.mojang.com/v1/objects/6e5e5c7e131f241c0a830143d133d89815d13692/client.jar|14w06a&https://launcher.mojang.com/v1/objects/e6dd656dd57666a500079d6545a77b07b45d5a1b/client.jar|14w05b&https://launcher.mojang.com/v1/objects/3e925a7e4cd1bbfa12fdf1bd9efc92618bdaa644/client.jar|14w05a&https://launcher.mojang.com/v1/objects/3b7c6e09b60d4aecdddf1e453a8b10ebd90becd4/client.jar|14w04b&https://launcher.mojang.com/v1/objects/a45ca517635783db8d902e6dd4099383a3a8ca0b/client.jar|14w04a&https://launcher.mojang.com/v1/objects/3034b21f0091e779b38d43d687f98037b4ac76c9/client.jar|14w03b&https://launcher.mojang.com/v1/objects/2cdbdfb9ccfb79be03e2b352fbe088f0875d2d1c/client.jar|14w03a&https://launcher.mojang.com/v1/objects/c1932528d91f084be73651509804d2266fd4c24d/client.jar|14w02c&https://launcher.mojang.com/v1/objects/03dec58354935b687679b9e4f7f9b346415778d2/client.jar|14w02b&https://launcher.mojang.com/v1/objects/c595430ae9d3624bae57b6cb5719399d87a0fe61/client.jar|14w02a&https://launcher.mojang.com/v1/objects/16fd472f9634bf49d051e4d84676fe4a5498cd14/client.jar|1.7.4&https://launcher.mojang.com/v1/objects/900950d8e3217b3a42405d1ecf767dcc31239d69/client.jar|1.7.3&https://launcher.mojang.com/v1/objects/feaf10fa9da94c388f2c53a2cf31a14406d0c532/client.jar|13w49a&https://launcher.mojang.com/v1/objects/a200562ad380a768fc63ad6c51a05aa2363dffff/client.jar|13w48b&https://launcher.mojang.com/v1/objects/064dae8977232b5470127c689e0c9d19c1c16dcd/client.jar|13w48a&https://launcher.mojang.com/v1/objects/069a7c867e6ebac830c9f3fef42a942ebd1e2204/client.jar|13w47e&https://launcher.mojang.com/v1/objects/1f9aa6969b4f086bef1232241579312de5c51d7f/client.jar|13w47d&https://launcher.mojang.com/v1/objects/2e94ebcc798b810409e7b1ea8174ec3c060634d7/client.jar|13w47c&https://launcher.mojang.com/v1/objects/3ed9a5dae1cb4d5776d2e0df30aaeb045e32d7cb/client.jar|13w47b&https://launcher.mojang.com/v1/objects/a7e8a25c8940ac44805e0de9cb622c03ef696d62/client.jar|13w47a&https://launcher.mojang.com/v1/objects/e15cafae431bda45dfc2f41f400242247807a046/client.jar|1.7.2&https://launcher.mojang.com/v1/objects/0c8689f904922af71c7144dcfb81bce976cadd49/client.jar|1.7.1&https://launcher.mojang.com/v1/objects/c0a041784d05e89b556ab8be42bed6009355165a/client.jar|1.7&https://launcher.mojang.com/v1/objects/fe85ec7d2dee38ac978288a964bed44030287245/client.jar|13w43a&https://launcher.mojang.com/v1/objects/8012b0d70a55f7267f34d4fb414cfedce05defa7/client.jar|13w42b&https://launcher.mojang.com/v1/objects/8f61c8284d3fe53acc77f122420d07ff46038e4c/client.jar|13w42a&https://launcher.mojang.com/v1/objects/1021c0a06cc98db76dad2b19805ed21935c8d938/client.jar|13w41b&https://launcher.mojang.com/v1/objects/1464fe999b3f2f9488bdb53bef775a5e80753af0/client.jar|13w41a&https://launcher.mojang.com/v1/objects/e1577011742284794337ab4efeefd1414714fc45/client.jar|13w39b&https://launcher.mojang.com/v1/objects/17969ef3be3a8ca59d38cfc2e2401ceb1dd17abc/client.jar|13w39a&https://launcher.mojang.com/v1/objects/b091a8b92b9d0b1f23ee2fd1a048b751f437ff76/client.jar|13w38c&https://launcher.mojang.com/v1/objects/1424fb2795c4d86ea034b868a2fd56c6ce6912d9/client.jar|13w38b&https://launcher.mojang.com/v1/objects/fe9877ec3c63ebf262c22ce4a941f5b02273ced1/client.jar|13w38a&https://launcher.mojang.com/v1/objects/5eddd7089cf7535281927be7f183e62a6d9fdfd3/client.jar|1.6.4&https://launcher.mojang.com/v1/objects/1703704407101cf72bd88e68579e3696ce733ecd/client.jar|1.6.3&https://launcher.mojang.com/v1/objects/f9af8a0a0fe24c891c4175a07e9473a92dc71c1a/client.jar|13w37b&https://launcher.mojang.com/v1/objects/40850dff34422b17c4b6308d43277d3ebf5c5b0e/client.jar|13w37a&https://launcher.mojang.com/v1/objects/02a9c7237e5a2b68d62d3cacd36d23c0437a14d5/client.jar|13w36b&https://launcher.mojang.com/v1/objects/bd29267cb8bfc3dd3821a9e5995e49c0c7516e72/client.jar|13w36a&https://launcher.mojang.com/v1/objects/875b8f0cd1397b6eaaaef71090649382d46de2d6/client.jar|1.6.2&https://launcher.mojang.com/v1/objects/b6cb68afde1d9cf4a20cbf27fa90d0828bf440a4/client.jar|1.6.1&https://launcher.mojang.com/v1/objects/17e2c28fb54666df5640b2c822ea8042250ef592/client.jar|1.6&https://launcher.mojang.com/v1/objects/5ead8f822527ed5957245be136daad9e322cab4f/client.jar|13w26a&https://launcher.mojang.com/v1/objects/b9a5d053016efa035f1b424625e0fd4736ac2dd6/client.jar|13w25c&https://launcher.mojang.com/v1/objects/f0f3a912f7a929cc21b6340c5e552ed28f90fa94/client.jar|13w25b&https://launcher.mojang.com/v1/objects/9c43c2084d56b7fe4a4952e64a070ab7c5fa2b17/client.jar|13w25a&https://launcher.mojang.com/v1/objects/b397e5bcf74858ce8f527c62c973087a0ea518a8/client.jar|13w24b&https://launcher.mojang.com/v1/objects/5a48548ecebbcacd9cd4d9faee77c4b34a4e092d/client.jar|13w24a&https://launcher.mojang.com/v1/objects/440abbedd473bdf38821906db6ea825d042848ae/client.jar|13w23b&https://launcher.mojang.com/v1/objects/f374a05f667ed8509cea98ad8c905feb1bf5d9cf/client.jar|13w23a&https://launcher.mojang.com/v1/objects/5f4d786a78b0b9b169f048b849f71dbc2d4ac0b0/client.jar|13w22a&https://launcher.mojang.com/v1/objects/ae1cc0f5971d4b582791b34b5c4131ff324f91ac/client.jar|13w21b&https://launcher.mojang.com/v1/objects/a9217325242df6decb96b2f14e1796953d1ce0fb/client.jar|13w21a&https://launcher.mojang.com/v1/objects/c89ead145ca87737ca2d10901a4f3bc837937006/client.jar|13w19a&https://launcher.mojang.com/v1/objects/39ef2f27bada1fb1a6addd972a25427ec33bd39d/client.jar|13w18c&https://launcher.mojang.com/v1/objects/e228310436fe3f70919092b5faff1aef9321b9a5/client.jar|13w18b&https://launcher.mojang.com/v1/objects/8061f2ea34b71999048b78a8aefe0bac9d30f91e/client.jar|13w18a&https://launcher.mojang.com/v1/objects/a2c623ea7fb3bfc5cdcc402f8aadf22ff657b2d7/client.jar|13w17a&https://launcher.mojang.com/v1/objects/87ef559adfe7d7980c0158d3dbaa28792d4e62cd/client.jar|1.5.2&https://launcher.mojang.com/v1/objects/465378c9dc2f779ae1d6e8046ebc46fb53a57968/client.jar|13w16b&https://launcher.mojang.com/v1/objects/17bdf9c566c9ce6726122eb3866581e4bbfa8e16/client.jar|13w16a&https://launcher.mojang.com/v1/objects/0764eecadf08ffb7a5ec96594afbc2ce844f8716/client.jar|1.5.1&https://launcher.mojang.com/v1/objects/047136381a552f34b1963c43304a1ad4dc0d2d8e/client.jar|1.5&https://launcher.mojang.com/v1/objects/a3da981fc9b875a51975d8f8100cc0c201c2ce54/client.jar|1.4.7&https://launcher.mojang.com/v1/objects/53ed4b9d5c358ecfff2d8b846b4427b888287028/client.jar|1.4.6&https://launcher.mojang.com/v1/objects/116758f41b32e8d1a71a4ad6236579acd724bca7/client.jar|1.4.5&https://launcher.mojang.com/v1/objects/7a8a963ababfec49406e1541d3a87198e50604e5/client.jar|1.4.4&https://launcher.mojang.com/v1/objects/b9b2a9e9adf1bc834647febc93a4222b4fd6e403/client.jar|1.4.3&https://launcher.mojang.com/v1/objects/f7274b201219b5729055bf85683eb6ef4f8024b4/client.jar|1.4.2&https://launcher.mojang.com/v1/objects/42d6744cfbbd2958f9e6688dd6e78d86d658d0d4/client.jar|1.4.1&https://launcher.mojang.com/v1/objects/67604a9c206697032165fc067b6255e333e06275/client.jar|1.4&https://launcher.mojang.com/v1/objects/2007097b53d3eb43b2c1f3f78caab4a4ef690c7a/client.jar|1.3.2&https://launcher.mojang.com/v1/objects/c2efd57c7001ddf505ca534e54abf3d006e48309/client.jar|1.3.1&https://launcher.mojang.com/v1/objects/33167e71e85ab8e6ddbe168bc67f6ec19d708d62/client.jar|1.3&https://launcher.mojang.com/v1/objects/4dfb8098b39c122f2aad13768d3f0d04db910f12/client.jar|1.2.5&https://launcher.mojang.com/v1/objects/4a2fac7504182a97dcbcd7560c6392d7c8139928/client.jar|1.2.4&https://launcher.mojang.com/v1/objects/ad6d1fe7455857269d4185cb8f24e62cc0241aaf/client.jar|1.2.3&https://launcher.mojang.com/v1/objects/5134e433afeba375c00bbdcd8aead1d3222813ee/client.jar|1.2.2&https://launcher.mojang.com/v1/objects/1dadfc4de6898751f547f24f72c7271218e4e28f/client.jar|1.2.1&https://launcher.mojang.com/v1/objects/c7662ac43dd04bfd677694a06972a2aaaf426505/client.jar|1.1&https://launcher.mojang.com/v1/objects/f690d4136b0026d452163538495b9b0e8513d718/client.jar|1.0&https://launcher.mojang.com/v1/objects/b679fea27f2284836202e9365e13a82552092e5d/client.jar|b1.8.1&https://launcher.mojang.com/v1/objects/6b562463ccc2c7ff12ff350a2b04a67b3adcd37b/client.jar|b1.8&https://launcher.mojang.com/v1/objects/3139e9c29b2c74f59ea04de760ac2af5bc21b410/client.jar|b1.7.3&https://launcher.mojang.com/v1/objects/43db9b498cb67058d2e12d394e6507722e71bb45/client.jar|b1.7.2&https://launcher.mojang.com/v1/objects/7dc50cc5e2ff204a7283f0c7d38cd0370b49875b/client.jar|b1.7&https://launcher.mojang.com/v1/objects/ad7960853437bcab86bd72c4a1b95f6fe19f4258/client.jar|b1.6.6&https://launcher.mojang.com/v1/objects/f95fe05711d09553ca2a9089f981741c13d6b8c4/client.jar|b1.6.5&https://launcher.mojang.com/v1/objects/90ed9854b43c4d031ed07381ea3ae3071a8bba6f/client.jar|b1.6.4&https://launcher.mojang.com/v1/objects/b5d3bdb8a7b12d163651f4787ac6ca14689aab9e/client.jar|b1.6.3&https://launcher.mojang.com/v1/objects/924e36dbb7c64abb30a95fe35f5affb5176f6cbc/client.jar|b1.6.2&https://launcher.mojang.com/v1/objects/e8aa50949b077b672be2e651ea3f7b1bbd9020e1/client.jar|b1.6.1&https://launcher.mojang.com/v1/objects/63a66d6d145696296bdaaeaba0a42f738b87a362/client.jar|b1.6&https://launcher.mojang.com/v1/objects/ecc0288d218fd7479027a17c150cbf283fa950a1/client.jar|b1.5_01&https://launcher.mojang.com/v1/objects/e2a692e5e8160c84b29c834ecbf398618db9749c/client.jar|b1.5&https://launcher.mojang.com/v1/objects/f5ce1699cd728213c21054fa2f1490d162b002b4/client.jar|b1.4_01&https://launcher.mojang.com/v1/objects/6f157f26955c35006c1afa8b0479e0ce785fb864/client.jar|b1.4&https://launcher.mojang.com/v1/objects/f6dbca5223ea2a7e89806e93d0b18162b2d58c20/client.jar|b1.3_01&https://launcher.mojang.com/v1/objects/add3809d2c075e985d4b583632dac3d9c3872945/client.jar|b1.3b&https://launcher.mojang.com/v1/objects/e19cfb3a2043f185c44237ef05eac80e8ad2d8e7/client.jar|b1.2_02&https://launcher.mojang.com/v1/objects/093f371e1a05d89664cfb8068d607953687d5d94/client.jar|b1.2_01&https://launcher.mojang.com/v1/objects/f71a5b58c9bd0e458878d78a34c9fb35e97d5222/client.jar|b1.2&https://launcher.mojang.com/v1/objects/ba05d7a97926c61c03cf956f7ae92f3bede9474e/client.jar|b1.1_02&https://launcher.mojang.com/v1/objects/e1c682219df45ebda589a557aadadd6ed093c86c/client.jar|b1.1_01&https://launcher.mojang.com/v1/objects/6d778940f48389a2741f03c9f17f3c57476fb208/client.jar|b1.0.2&https://launcher.mojang.com/v1/objects/76d35cb452e739bd4780e835d17faf0785d755f9/client.jar|b1.0_01&https://launcher.mojang.com/v1/objects/4caf69885b64132e42d3ce49996dbdb1691d7111/client.jar|b1.0&https://launcher.mojang.com/v1/objects/93faf3398ebf8008d59852dc3c2b22b909ca8a49/client.jar|a1.2.6&https://launcher.mojang.com/v1/objects/a68c817afd6c05c253ba5462287c2c19bbb57935/client.jar|a1.2.5&https://launcher.mojang.com/v1/objects/f48c7b6442ad8d01099ecee1c7c7332f1b1a80da/client.jar|a1.2.4_01&https://launcher.mojang.com/v1/objects/7be6298b05d1b0832ab45467a87a425640bc6bf0/client.jar|a1.2.3_04&https://launcher.mojang.com/v1/objects/7f60cb9d0d40af20001d15287b78aa26a217a910/client.jar|a1.2.3_02&https://launcher.mojang.com/v1/objects/dc61158e1df763f87483abb6ab540dc1c42e63c4/client.jar|a1.2.3_01&https://launcher.mojang.com/v1/objects/1d46e65022f3a7cf4b8ad30ee5a8d52b3b2b9486/client.jar|a1.2.3&https://launcher.mojang.com/v1/objects/f4be258122cb62208b350cd2068685ad859bb447/client.jar|a1.2.2b&https://launcher.mojang.com/v1/objects/1c28c8431392641045b59e98a81877d7c94ff0ca/client.jar|a1.2.2a&https://launcher.mojang.com/v1/objects/7d9d85eaca9627d3a40e6d122182f2d22d39dbf9/client.jar|a1.2.1_01&https://launcher.mojang.com/v1/objects/e4226f9ba622634e3101681bc641eec7ee9e72fd/client.jar|a1.2.1&https://launcher.mojang.com/v1/objects/e4226f9ba622634e3101681bc641eec7ee9e72fd/client.jar|a1.2.0_02&https://launcher.mojang.com/v1/objects/b99da0a683e6dc1ade4df1bf159e021ad07d4fca/client.jar|a1.2.0_01&https://launcher.mojang.com/v1/objects/332bfe7bf26f6a5cc93ee85e6759ce33784409d0/client.jar|a1.2.0&https://launcher.mojang.com/v1/objects/8632ea716fd083c2975f16d612306fd80bee46db/client.jar|a1.1.2_01&https://launcher.mojang.com/v1/objects/daa4b9f192d2c260837d3b98c39432324da28e86/client.jar|a1.1.2&https://launcher.mojang.com/v1/objects/f9b4b66f9c18bf4800d80f1c8865a837f92c6105/client.jar|a1.1.0&https://launcher.mojang.com/v1/objects/d58d1db929994ff383bdbe6fed31887e04b965c3/client.jar|a1.0.17_04&https://launcher.mojang.com/v1/objects/61cb4c717981f34bf90e8502d2eb8cf2aa6db0cd/client.jar|a1.0.17_02&https://launcher.mojang.com/v1/objects/39f20ee472a40322e034643a8d1668836f5052bd/client.jar|a1.0.16&https://launcher.mojang.com/v1/objects/98ce80c7630ccb3bb38687ff98bfd18935d49a57/client.jar|a1.0.15&https://launcher.mojang.com/v1/objects/03edaff812bedd4157a90877e779d7b7ecf78e97/client.jar|a1.0.14&https://launcher.mojang.com/v1/objects/9b4b90d8def2a680b7c9eca40dd03e2266c8977a/client.jar|a1.0.11&https://launcher.mojang.com/v1/objects/d7ceb02909d0e1031a99ff4d8053d3f4abfbb2da/client.jar|a1.0.5_01&https://launcher.mojang.com/v1/objects/73f569bf5556580979606049204835ae1a54f04d/client.jar|a1.0.4&https://launcher.mojang.com/v1/objects/e5838277b3bb193e58408713f1fc6e005c5f3c0c/client.jar|inf-20100618&https://launcher.mojang.com/v1/objects/89eab2c1a353707cc00f074dffba9cb7a4f5e304/client.jar|c0.30_01c&https://launcher.mojang.com/v1/objects/54622801f5ef1bcc1549a842c5b04cb5d5583005/client.jar|c0.0.13a&https://launcher.mojang.com/v1/objects/936d575b1ab1a04a341ad43d76e441e88d2cd987/client.jar|c0.0.13a_03&https://launcher.mojang.com/v1/objects/7ba9e63aec8a15a99ecd47900c848cdce8a51a03/client.jar|c0.0.11a&https://launcher.mojang.com/v1/objects/3a799f179b6dcac5f3a46846d687ebbd95856984/client.jar|rd-161348&https://launcher.mojang.com/v1/objects/6323bd14ed7f83852e17ebc8ec418e55c97ddfe4/client.jar|rd-160052&https://launcher.mojang.com/v1/objects/b100be8097195b6c9112046dc6a80d326c8df839/client.jar|rd-20090515&https://launcher.mojang.com/v1/objects/6323bd14ed7f83852e17ebc8ec418e55c97ddfe4/client.jar|rd-132328&https://launcher.mojang.com/v1/objects/12dace5a458617d3f90337a7ebde86c0593a6899/client.jar|rd-132211&https://launcher.mojang.com/v1/objects/393e8d4b4d708587e2accd7c5221db65365e1075/client.jar|rd-132211&https://launcher.mojang.com/v1/objects/393e8d4b4d708587e2accd7c5221db65365e1075/client.jar");
        }

        internal string MCVersionAnalysis(string type)
        {
            string[,] textArray1 = { { "snapshot", "快照版" }, { "release", "正式版" }, { "old_beta", "基岩版" }, { "old_alpha", "远古版" } };
            string[,] strArray = textArray1;

            for (int i = 0; i < (strArray.Length / 2); i++)
                if (strArray[i, 0] == type) return strArray[i, 1];
            

            return null;
        }

        internal bool MD5Exists(string FilePath)
        {
            var text = new MD5().GetMD5HashFromFile(FilePath);
            var file = GetFile(@"SquareMinecraftLauncher\MD5.Sika");

            if (file != null)
            {
                char[] separator = new char[] { '\n' };
                string[] strArray = file.Split(separator);

                for (int i = 0; i < strArray.Length; i++)
                    if (strArray[i] == text) return true;
                

                wj(@"SquareMinecraftLauncher\MD5.Sika", file + "\n" + text);
            }
            else
            {
                wj(@"SquareMinecraftLauncher\MD5.Sika", text);
            }

            return false;
        }

        internal string nativeszip(string version)
        {
            var unzip = new Unzip();

            foreach (MCDownload download in new Tools().GetAllNatives(version))
            {
                string str;
                unzip.UnZipFile(download.path, System.Directory.GetCurrentDirectory() + @"\.minecraft\versions\" + version + @"\" + version + "-natives", out str);
            }

            string[] textArray2 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, "-natives" };
            return string.Concat(textArray2);
        }

        internal async void opKeep(string version, string patch)
        {
            await new Tools().OptifineInstall(version, patch);
        }

        internal void path(string path)
        {
            char[] separator = new char[] { '\\' };
            string str = null;

            foreach (string str2 in path.Split(separator))
            {
                if (str == null)
                {
                    str = str2;
                    SetFile(str2);
                }
                else
                {
                    str = str + @"\" + str2;
                    SetFile(str);
                }
            }
        }

        internal MCDownload[] screening(MCDownload[] Lib)
        {
            string path = null;
            List<MCDownload> list = new List<MCDownload>();

            for (int i = 0; i < Lib.Length; i++)
            {
                if (path != Lib[i].path) list.Add(Lib[i]);
                path = Lib[i].path;
            }

            return list.ToArray();
        }

        internal void SetFile(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        internal string token()
        {
            string[] strArray = new string[] {
                "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h",
                "j", "k", "l", "z", "x", "c", "v", "b", "n", "m", "Q", "W", "E", "R", "T", "Y",
                "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C",
                "V", "B", "N", "M", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
             };

            var random = new Random();
            string str = null;

            for (int i = 0; 0x20 > i; i++)
                str = str + strArray[random.Next(0, strArray.Length)];
            

            return str;
        }

        internal string uuid(string name)
        {
            var encrypt = new DESEncrypt();
            SetFile(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher");
            var file = GetFile(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\user.Sika");

            if (file != "")
            {
                file = encrypt.Decrypt(file, "zxttquws");
                char[] separator = new char[] { '\n' };
                string[] strArray = file.Split(separator);

                for (int i = 0; i < strArray.Length; i++)
                {
                    char[] chArray2 = new char[] { '|' };
                    string[] strArray2 = strArray[i].Split(chArray2);
                    if (strArray2[0] == name) return strArray2[1];
                }
            }

            var str2 = web.getHtml("https://api.mojang.com/users/profiles/minecraft/" + name);

            if ((str2 != null) && (str2 != ""))
            {
                var root = JsonConvert.DeserializeObject<Root>(str2);

                if (file != "")
                {
                    string[] textArray1 = new string[] { file, "\n", name, "|", root.id };
                    wj(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\user.Sika", encrypt.Encrypt(string.Concat(textArray1), "zxttquws"));
                }
                else
                {
                    wj(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\user.Sika", encrypt.Encrypt(name + "|" + root.id, "zxttquws"));
                }

                return root.id;
            }

            str2 = token();

            if (file != "")
            {
                string[] textArray2 = new string[] { file, "\n", name, "|", str2 };
                wj(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\user.Sika", encrypt.Encrypt(string.Concat(textArray2), "zxttquws"));
                return str2;
            }

            wj(System.Directory.GetCurrentDirectory() + @"\SquareMinecraftLauncher\user.Sika", encrypt.Encrypt(name + "|" + str2, "zxttquws"));
            return str2;
        }

        internal JObject versionjson(string version)
        {
            StreamReader reader = null;

            try
            {
                string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
                reader = new StreamReader(string.Concat(textArray1), Encoding.Default);
            }
            catch (DirectoryNotFoundException)
            {
                throw new SquareMinecraftLauncherException("未找到该版本");
            }

            return JObject.Parse(reader.ReadToEnd());
        }

        internal T versionjson<T>(string version)
        {
            StreamReader reader = null;

            try
            {
                string[] textArray1 = new string[] { System.Directory.GetCurrentDirectory(), @"\.minecraft\versions\", version, @"\", version, ".json" };
                reader = new StreamReader(string.Concat(textArray1), Encoding.Default);
            }
            catch (Exception)
            {
                throw new SquareMinecraftLauncherException("未找到该版本");
            }

            var local = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            reader.Close();
            return local;
        }

        internal void wj(string path, string text)
        {
            try
            {
                File.WriteAllText(path, text, Encoding.UTF8);
            }
            catch (Exception exception1)
            {
                throw new SquareMinecraftLauncherException(exception1.Message);
            }
        }
    }
} 