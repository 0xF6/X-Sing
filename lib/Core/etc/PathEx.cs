namespace XSing.Core.etc
{
    using System.IO;

    public static class PathEx
    {
        public static string WithCombine(this string path1, string path2) => Path.Combine(path1, path2);
        public static FileInfo AsFile(this string path1) => new FileInfo(path1);
        public static StreamReader OpenReader(this FileInfo inf) => new StreamReader(inf.OpenRead());

        public static StreamWriter PushLine(this StreamWriter inf, object o)
        {
            inf.WriteLine(o);
            return inf;
        }
    }
}