using System.Collections.Generic;

namespace json113
{
    internal class Arguments
    {
        /// <summary>
        ///
        /// </summary>
        public List<string> game { get; set; }
    }

    internal class Root
    {
        /// <summary>
        ///
        /// </summary>
        public Arguments arguments { get; set; }
    }
}