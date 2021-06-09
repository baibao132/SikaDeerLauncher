namespace SquareMinecraftLauncher.Core.JsonIntegration
{
    internal class SetJson
    {
        public string Setjson(string key, string value) => "\"" + key + "\":\"" + value + "\"";

        public string SetjsonArray(string key, string[,,] Array)
        {
            var json = "\"" + key + "\":[";

            for (int i = 0; i < Array.GetLength(0); i++)
            {
                var forjson = "{";

                for (int t = 0; t < Array.GetLength(1); t++)
                {
                    if (Array[i, t, 0] == null) continue;
                    forjson += Setjson(Array[i, t, 0], Array[i, t, 1]) + ",";
                }

                forjson = forjson.Substring(0, forjson.Length - 1);
                json += forjson + "},";
            }

            return json.Substring(0, json.Length - 1) + "]";
        }

        public string SetjsonArray(string key, string[] Array)
        {
            var Json = "";

            foreach (var i in Array)
                Json += "\"" + i + "\",";
            

            return "[" + Json.Substring(0, Json.Length - 1) + "]";
        }
    }
} 