using System.Collections.Generic;

namespace json2
{
    internal class PropertiesItem
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    internal class Root
    {
        public string accessToken { get; set; }
        public string clientToken { get; set; }
        public string error { get; set; }
        public string errorMessage { get; set; }
        public SelectedProfile selectedProfile { get; set; }
        public User user { get; set; }
    }

    internal class SelectedProfile
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    internal class User
    {
        public string id { get; set; }
        public List<PropertiesItem> properties { get; set; }
    }
}