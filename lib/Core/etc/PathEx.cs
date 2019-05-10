namespace XSing.Core.etc
{
    using System.IO;

    public static class PathEx
    {
        public static string WithCombine(this string path1, string path2) => Path.Combine(path1, path2);
    }
}