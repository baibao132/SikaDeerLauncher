using System.Collections.Generic;

namespace SquareMinecraftLauncher.Core.Forge
{
    internal class ForgeJson
    {
        public class Arguments
        {
            /// <summary>
            ///
            /// </summary>
            public List<string> game { get; set; }
        }

        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public Arguments arguments { get; set; }
        }
    }
}