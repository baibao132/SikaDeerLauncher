namespace SquareMinecraftLauncher
{
    internal class UP
    {
        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public string accessToken { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string clientToken { get; set; }

            /// <summary>
            ///
            /// </summary>
            public SelectedProfile selectedProfile { get; set; }
        }

        public class SelectedProfile
        {
            /// <summary>
            ///
            /// </summary>
            public string id { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string name { get; set; }
        }
    }
}