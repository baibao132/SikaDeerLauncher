using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public class MicrosoftLogin
    {
        Download web = new Download();

        public string GetToken(string code)
        {
            var json = web.Post("https://login.live.com/oauth20_token.srf", "client_id=00000000402b5328&code= " + code + " &grant_type=authorization_code&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf", "application/x-www-form-urlencoded");
            var jsonConvert = JsonConvert.DeserializeObject<MicrosoftToken.Root>(json);
            return jsonConvert.access_token;
        }

        public string Login()
        {
            var thd = new Thread(new ThreadStart(MicrosoftLoginFroms.start.Main));
            thd.SetApartmentState(ApartmentState.STA);
            thd.IsBackground = true;
            thd.Start();

            while (MicrosoftLoginFroms.Form1.url == "")
                Thread.Sleep(5000);
            

            return Regex.Split(MicrosoftLoginFroms.Form1.url, "code=")[1].Split('&')[0];
        }

        public string RefreshingTokens(string code)
        {
            var json = web.Post("https://login.live.com/oauth20_token.srf", "client_id=00000000402b5328&refresh_token=" + code + "&grant_type=refresh_token&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf", "application/x-www-form-urlencoded");
            var jsonConvert = JsonConvert.DeserializeObject<MicrosoftToken.Root>(json);
            return jsonConvert.access_token;
        }
    }
} 