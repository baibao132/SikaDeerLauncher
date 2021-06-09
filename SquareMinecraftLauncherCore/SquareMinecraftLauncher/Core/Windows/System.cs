namespace System
{
    internal class Directory
    {
        internal static string Files { get; set; }

        internal static string GetCurrentDirectory()
        {
            if (Files != null) return Files;
            return System.IO.Directory.GetCurrentDirectory();
        }
    }
}