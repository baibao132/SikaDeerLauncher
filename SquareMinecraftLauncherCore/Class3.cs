using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SikaDeerLauncher;
using SikaDeerLauncher.Minecraft;
using System.Text.RegularExpressions;

namespace mcbbs
{
    class Class3
    {
        static void Main(string[] args)
        {
            //SikaDeerLauncher.Minecraft.Tools tools = new Tools();
           // RedstoneMCSkin RedstoneMCSkin = tools.GetRedstoneMCSkin("2817541592@qq.com","zx3481133");
            //  Console.WriteLine("Token:"+ RedstoneMCSkin.accessToken);
         //     foreach (var bs in RedstoneMCSkin.NameItem)
            // {
          //       Console.WriteLine("游戏名："+bs.Name+"   uuid："+bs.uuid);
         //     }
            /*/
           game.IMG("1.12.2");
           /*/
          //  download[] downloads = tools.GetAllFile("1.14");
            //foreach (var d in downloads)
            //{
            //    Console.WriteLine(d.path +"  "+d.Url);
            //}
            //Console.WriteLine("\n\n\n\n");
            //downloads = tools.GetMissingFile("1.14");
            //foreach (var d in downloads)
            //{
            //    Console.WriteLine(d.path + "  " + d.Url);
            //}
            //Console.WriteLine("\n\n\n\n");
            //downloads = tools.GetTheExistingLibrary("1.14");
            //foreach (var d in downloads)
            //{
            //    Console.WriteLine(d.path + "  " + d.Url);
            //}
            //Console.WriteLine("\n\n\n\n");
            //downloads = tools.GetAllLibrary("1.14");
            //foreach (var d in downloads)
            //{
            //    Console.WriteLine(d.path + "  " + d.Url);
            //}
            //Console.WriteLine("\n\n\n\n");
            //downloads = tools.GetMissingLibrary("1.14");
            //foreach (var d in downloads)
            //{
            //    Console.WriteLine(d.path + "  " + d.Url);
            //}
            //Console.WriteLine("\n\n\n\n");
            //downloads = tools.GetMissingAsset("1.14");
            //foreach (var d in downloads)
            //{
            //    Console.WriteLine(d.path + "  " + d.Url);
            //}
            //Console.WriteLine("\n\n\n\n");
            //tools.DownloadSourceInitialization(DownloadSource.MinecraftSource);
            //downloads = tools.GetAllTheAsset("1.14");
            //foreach (var d in downloads)
            //{
            //    Console.WriteLine(d.path + "  " + d.Url);
            //}

            Tools tools = new Tools();
            //Console.WriteLine(mincraftDownload.ForgeInstallation(@"D:\forge-1.12.2-14.23.5.2836-installer.jar","1.12.2"));
            //Game Game = new Game();
            //Game.LogEvent += new Game.LogDel(Log);//事件
            //Game.ErrorEvent += new Game.ErrorDel(Error);
            //   Skin s = tools.GetAuthlib_Injector("https://mcskin.i-creator.cn/api/yggdrasil", "2817541592@qq.com","zx3481133");
            //     Game.StartGame("1.7.10", @"C:\Program Files (x86)\Java\jre1.8.0_201\bin\javaw.exe", 512, s.NameItem[0].Name,s.NameItem[0].uuid,s.accessToken, "https://mcskin.i-creator.cn/api/yggdrasil/","","");//启动
            ////  Console.WriteLine(tools.GetCompareForgeVersions("1.14"));
            //LiteloaderList[] liteloaderLists = mincraftDownload.GetLiteloaderList();
            //Console.WriteLine("liteloader列表");
            //foreach (var a in liteloaderLists)
            //{
            //    Console.WriteLine(a.lib[0].name);
            //}
            ////OptiFineList[] optiFine = mincraftDownload.GetOptiFineList("1.7.10");
            ////Console.WriteLine("optiFine列表");
            ////foreach (var a in optiFine)
            ////{
            ////    Console.WriteLine(a.filename);
            ////}
            //MinecraftDownload minecraft = new MinecraftDownload();
            //  SikaDeerLauncher.Core.LiteloaderCore core = new SikaDeerLauncher.Core.LiteloaderCore();
            //Console.WriteLine(minecraft.ForgeDownload("1.14.3", tools.GetForgeList("1.14.3")[0].ForgeVersion).url);
            //      Console.WriteLine(core.LiteloaderJson("1.7.10"));
            // tools.liteloaderInstall("1.7.10");
            //OptiFineList[] list = tools.GetOptiFineList("1.7.10"); 
            // SikaDeerLauncher.Core.SikaDeerLauncherCore core = new SikaDeerLauncher.Core.SikaDeerLauncherCore();
            //Console.WriteLine(core.OptifineJson("1.7.10",list[0]));
            AllTheExistingVersion[] a = tools.GetAllTheExistingVersion();
            Console.WriteLine(a[0].IdVersion);
            tools.GetUnifiedPass("4c6b282ea6e011e89251525400b59b6a","YHC","Z3481133");
            //core.MCVersion();
            Console.ReadKey();
            //73c12e43d4d449819681dee329cd0d4b   AccessToken   /refresh
        }
        private static void Log(Game.Log error)
        {
            Console.WriteLine(error.Message);
        }
        private static void Error(Game.Error error)
        {
            Console.WriteLine(error.Message);
        }
    }
}
