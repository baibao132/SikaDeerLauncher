using System.Collections.Generic;

namespace SquareMinecraftLauncherWPF
{
    public class DIYJson
    {
        public class DIYLoginItem
        {
            /// <summary>
            ///
            /// </summary>
            public string DIYname { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string L { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string s { get; set; }
        }

        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public List<DIYLoginItem> DIYLogin { get; set; }
        }
    }
}