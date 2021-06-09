using System.Collections.Generic;

namespace SquareMinecraftLauncher
{
    internal class UPSkin
    {
        public class PropertiesItem
        {
            /// <summary>
            ///
            /// </summary>
            public string name { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string value { get; set; }
        }

        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public string id { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string name { get; set; }

            /// <summary>
            ///
            /// </summary>
            public List<PropertiesItem> properties { get; set; }
        }
    }

    internal class UPSkinBase
    {
        public class CAPE
        {
            /// <summary>
            /// 披风地址
            /// </summary>
            public string url { get; set; }
        }

        public class Metadata
        {
            /// <summary>
            ///
            /// </summary>
            public string model { get; set; }
        }

        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public Textures textures { get; set; }
        }

        public class SKIN
        {
            /// <summary>
            ///
            /// </summary>
            public Metadata metadata { get; set; }

            /// <summary>
            /// 皮肤地址
            /// </summary>
            public string url { get; set; }
        }
        public class Textures
        {
            /// <summary>
            ///
            /// </summary>
            public CAPE CAPE { get; set; }

            /// <summary>
            ///
            /// </summary>
            public SKIN SKIN { get; set; }
        }
    }
}