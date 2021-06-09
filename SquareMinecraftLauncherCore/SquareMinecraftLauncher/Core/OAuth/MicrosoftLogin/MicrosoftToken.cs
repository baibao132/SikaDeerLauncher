namespace SquareMinecraftLauncher.Core.OAuth
{
    internal class MicrosoftToken
    {
        public class Root
        {
            /// <summary>
            ///
            /// </summary>
            public string access_token { get; set; }

            /// <summary>
            ///
            /// </summary>
            public int expires_in { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string foci { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string refresh_token { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string scope { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string token_type { get; set; }
            /// <summary>
            ///
            /// </summary>
            public string user_id { get; set; }
        }
    }
}