namespace SquareMinecraftLauncher.Core.Forge
{
    using System.Collections.Generic;

    internal class ForgeY
    {
        public class Artifact
        {
            public string url { get; set; }
        }

        public class AssetIndex
        {
            public string id { get; set; }

            public string size { get; set; }

            public string url { get; set; }
        }

        public class Client
        {
            public string sha1 { get; set; }

            public string size { get; set; }

            public string url { get; set; }
        }

        public class Downloads
        {
            public ForgeY.Artifact artifact { get; set; }
        }

        public class Downloadsy
        {
            public ForgeY.Client client { get; set; }
        }

        public class LibrariesItem
        {
            public ForgeY.Downloads downloads { get; set; }

            public string name { get; set; }

            public ForgeY.Natives natives { get; set; }

            public string url { get; set; }
        }

        public class Natives
        {
            public string linux { get; set; }

            public string osx { get; set; }

            public string windows { get; set; }
        }

        public class Root
        {
            public ForgeY.AssetIndex assetIndex { get; set; }

            public string assets { get; set; }

            public ForgeY.Downloadsy downloads { get; set; }

            public string id { get; set; }

            public List<ForgeY.LibrariesItem> libraries { get; set; }

            public string mainClass { get; set; }
        }
    }
}