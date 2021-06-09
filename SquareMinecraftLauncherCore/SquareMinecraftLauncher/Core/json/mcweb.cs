namespace SquareMinecraftLauncher.Core.json
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class mcweb
    {
        public class Root
        {
            public List<mcweb.VersionsItem> versions { get; set; }
        }

        public class VersionsItem
        {
            public string id { get; set; }

            public string releaseTime { get; set; }

            public string time { get; set; }

            public string type { get; set; }

            public string url { get; set; }
        }
    }
}

