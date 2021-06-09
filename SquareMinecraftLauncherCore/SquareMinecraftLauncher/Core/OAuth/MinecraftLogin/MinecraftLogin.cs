using Newtonsoft.Json;
using System;

namespace SquareMinecraftLauncher.Core.OAuth
{
    public class MinecraftLogin
    {
        Download web = new Download();

        public MinecraftLoginToken GetPossessionGame(string code)
        {
            var json = web.Post("https://api.minecraftservices.com/entitlements/profile", "Authorization:" + code);
            if (json == "") throw new SquareMinecraftLauncherException("Minecraft登录异常");
            try
            {
                var j = JsonConvert.DeserializeObject<PossessionGame.Root>(json);
                var minecraftLoginToken = new MinecraftLoginToken();
                minecraftLoginToken.name = j.name;
                minecraftLoginToken.SkinUrl = j.skins[0].url;
                minecraftLoginToken.uuid = j.id;
                return minecraftLoginToken;
            }
            catch (Exception ex)
            {
                throw new SquareMinecraftLauncherException("你未拥有该游戏");
            }
        }

        public string GetToken(string[] code, bool access_token)
        {
            var json = web.Post("https://api.minecraftservices.com/authentication/login_with_xbox", "{ \"identityToken\": \"XBL3.0x=" + code[0] + ";" + code[1] + " \" }");
            if (json == "") throw new SquareMinecraftLauncherException("Minecraft登录异常");
            try
            {
                var j = JsonConvert.DeserializeObject<MinecraftLoginResult.Root>(json);
                if (access_token) return j.access_token;
                else return j.token_type;
            }
            catch (Exception ex)
            {
                var j = JsonConvert.DeserializeObject<MinecraftLoginErr.Root>(json);
                throw new SquareMinecraftLauncherException(j.error);
            }
        }
    }
}