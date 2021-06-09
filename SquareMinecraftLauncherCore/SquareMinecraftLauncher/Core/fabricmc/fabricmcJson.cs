using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.fabricmc
{
    internal class fabricmcJson
    {
        public class Arguments
        {
            /// <summary>
            ///
            /// </summary>
            public List<string> client { get; set; }

            /// <summary>
            ///
            /// </summary>
            public List<string> common { get; set; }

            /// <summary>
            ///
            /// </summary>
            public List<string> server { get; set; }
        }

        public class CommonItem
        {
            /// <summary>
            ///
            /// </summary>
            public string name { get; set; }

            public string url { get; set; }
        }

        public class Launchwrapper
        {
            /// <summary>
            ///
            /// </summary>
            public Tweakers tweakers { get; set; }
        }

        public class Libraries
        {
            /// <summary>
            ///
            /// </summary>
            public List<string> client { get; set; }

            /// <summary>
            ///
            /// </summary>
            public List<CommonItem> common { get; set; }

            /// <summary>
            ///
            /// </summary>
            public List<string> server { get; set; }
        }
        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public Arguments arguments { get; set; }

            /// <summary>
            ///
            /// </summary>
            public Launchwrapper launchwrapper { get; set; }

            /// <summary>
            ///
            /// </summary>
            public Libraries libraries { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string mainClass { get; set; }

            /// <summary>
            ///
            /// </summary>
            public int version { get; set; }
        }

        public class Tweakers
        {
            /// <summary>
            ///
            /// </summary>
            public List<string> client { get; set; }

            /// <summary>
            ///
            /// </summary>
            public List<string> common { get; set; }

            /// <summary>
            ///
            /// </summary>
            public List<string> server { get; set; }
        }
    }
}