namespace SquareMinecraftLauncher.Core.ForgeInstall
{
    using System.Collections.Generic;

    internal class ForgeJson
    {
        public class Artifact
        {
            public string path { get; set; }

            public string sha1 { get; set; }

            public int size { get; set; }

            public string url { get; set; }
        }

        public class Downloads
        {
            public ForgeJson.Artifact artifact { get; set; }
        }

        public class LibrariesItem
        {
            public ForgeJson.Downloads downloads { get; set; }

            public string name { get; set; }
        }

        public class ProcessorsItem
        {
            public List<string> args { get; set; }

            public List<string> classpath { get; set; }

            public string jar { get; set; }
        }

        public class Root
        {
            public List<ForgeJson.LibrariesItem> libraries { get; set; }

            public List<ForgeJson.ProcessorsItem> processors { get; set; }
        }
    }
}