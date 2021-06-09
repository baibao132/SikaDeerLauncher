namespace SquareMinecraftLauncher
{
    using global::SquareMinecraftLauncher.Core;
    using System;

    public sealed class SquareMinecraftLauncherException : Exception
    {
        public SquareMinecraftLauncherException(string message) : base(message)
        {
            var core = new SquareMinecraftLauncherCore();
            core.SetFile("SquareMinecraftLauncher");
            string file = null;
            if (core.FileExist(@"SquareMinecraftLauncher\Error.Log") == null)
            {
                file = core.GetFile(@"SquareMinecraftLauncher\Error.Log");
                core.wj(@"SquareMinecraftLauncher\Error.Log", file + "\n[" + DateTime.Now.ToString() + "] " + message);
            }
            else
            {
                core.wj(@"SquareMinecraftLauncher\Error.Log", "[" + DateTime.Now.ToString() + "] " + message);
            }
        }
    }
}