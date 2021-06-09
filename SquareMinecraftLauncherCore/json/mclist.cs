using System.Collections.Generic;

namespace json
{
    internal class Artifact
    {
        /// <summary>
        ///
        /// </summary>
        public string path { get; set; }

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

    internal class Client
    {
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

    internal class Downloads
    {
        /// <summary>
        ///
        /// </summary>
        public Artifact artifact { get; set; }
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
        /// <summary>
        ///
        /// </summary>
        public Downloads downloads { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; }
    }

    internal class Logging
    {
        /// <summary>
        ///
        /// </summary>
        public Client client { get; set; }
    }

    internal class Root
    {
        /// <summary>
        ///
        /// </summary>
        public AssetIndex assetIndex { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string assets { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Downloads downloads { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<LibrariesItem> libraries { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Logging logging { get; set; }

        /// <summary>
        ///
        /// </summary>
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

    internal class Server
    {
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
}