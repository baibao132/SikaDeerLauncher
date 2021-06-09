using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.OAuth
{
    internal class XboxToken
    {
        public class DisplayClaims
        {
            /// <summary>
            ///
            /// </summary>
            public List<XuiItem> xui { get; set; }
        }

        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public DisplayClaims DisplayClaims { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string IssueInstant { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string NotAfter { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string Token { get; set; }
        }

        public class XuiItem
        {
            /// <summary>
            ///
            /// </summary>
            public string uhs { get; set; }
        }
    }
}