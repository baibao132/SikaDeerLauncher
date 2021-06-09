using System.Collections.Generic;

namespace json4
{
    internal class artifact
    {
        public string url { get; set; }
    }

    internal class AssetIndex
    {
        /// <summary>
        ///
        /// </summary>
        public string id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string sha1 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int size { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int totalSize { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string url { get; set; }
    }

    internal class downloads
    {
        public artifact artifact { get; set; }
    }

    internal class File
    {
        /// <summary>
        ///
        /// </summary>
        public string id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string sha1 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int size { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string url { get; set; }
    }

    internal class LibrariesItem
    {
        public downloads downloads { get; set; }
        public string name { get; set; }
        public natives natives { get; set; }
    }
    internal class natives
    {
        public string linux { get; set; }
        public string osx { get; set; }
        public string windows { get; set; }
    }
    internal class Root
    {
        public AssetIndex AssetIndex { get; set; }
        public string assets { get; set; }
        public string id { get; set; }
        public List<LibrariesItem> libraries { get; set; }
        public string mainClass { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string minecraftArguments { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int minimumLauncherVersion { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string releaseTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string time { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string type { get; set; }
    }

    internal class Root1
    {
        public List<LibrariesItem> libraries { get; set; }
    }
}